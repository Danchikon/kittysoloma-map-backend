using KittysolomaMap.Domain.Geo;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;
using KittysolomaMap.Infrastructure.Overpass.Enums;
using KittysolomaMap.Infrastructure.Overpass.Refit;
using KittysolomaMap.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KittysolomaMap.Api.Rest.Controllers;

[ApiController]
[Route("api/overpass")]
public class OverpassController : ControllerBase
{
   
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<OverpassController> _logger;

    public OverpassController(
        IServiceScopeFactory serviceScopeFactory, 
        ILogger<OverpassController> logger
        )
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }
    
    [HttpPost]
    public async Task<ActionResult> QueryAsync(
        [FromBody] string data,
        CancellationToken cancellationToken
        )
    {
        await Task.Factory.StartNew(async () =>
        {
            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var overpassRefitOverpassRefitApi = scope.ServiceProvider.GetRequiredService<IOverpassRefitApi>();
            var dbContext = scope.ServiceProvider.GetRequiredService<KittysolomaMapDbContext>();
            
            var response = await overpassRefitOverpassRefitApi.QueryAsync(data, cancellationToken);

            var result = response.Content!.Elements;
            
            var nodes = new List<NodeEntity>();
            
            foreach (var element in result)
            {
                if (element.Type is not OverpassElementType.Node)
                {
                    continue;
                }

                var nodeEntity = new NodeEntity
                {
                    Id = element.Id,
                    Location = new Point(Convert.ToDouble(element.Lon!.Value), Convert.ToDouble(element.Lat!.Value)),
                };
                
                
                foreach (var tag in element.Tags)
                {
                    nodeEntity.Tags.Add(new NodeTagEntity
                    {
                        NodeId = nodeEntity.Id,
                        Value = tag.Value,
                        Name = tag.Key
                    });
                }
                
                nodes.Add(nodeEntity);
            }
            
            var ways = new List<WayEntity>();
            var nodeWays = new List<NodeWayEntity>();
            
            foreach (var element in result)
            {
                if (element.Type is not OverpassElementType.Way)
                {
                    continue;
                }
                
                var wayEntity = new WayEntity
                {
                    Id = element.Id,
                };

                nodeWays.AddRange(element.Nodes!.ToHashSet().Select(node => new NodeWayEntity { NodeId = node, WayId = element.Id }));

                foreach (var tag in element.Tags)
                {
                    wayEntity.Tags.Add(new WayTagEntity
                    {
                        WayId = wayEntity.Id,
                        Value = tag.Value,
                        Name = tag.Key
                    });
                }
                
                ways.Add(wayEntity);
            }
            
            _logger.LogInformation("Saving ways | ways count - {WaysCount}", ways.Count);
            
            foreach (var waysChunk in ways.Chunk(300_000))
            {
                await dbContext.Ways.AddRangeAsync(waysChunk, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            
            _logger.LogInformation("Saving nodes | nodes count - {NodesCount}", nodes.Count);
            
            foreach (var nodesChunk in nodes.Chunk(300_000))
            {
                await dbContext.Nodes.AddRangeAsync(nodesChunk, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            
            _logger.LogInformation("Saving nodeWays | nodeWays count - {NodesCount}", nodeWays.Count);
            
            foreach (var nodeWaysChunk in nodeWays.Chunk(300_000))
            {
                await dbContext.NodeWays.AddRangeAsync(nodeWaysChunk, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }, cancellationToken);
        
        return NoContent();
    } 
}