using KittysolomaMap.Domain.Geo;

namespace KittysolomaMap.Infrastructure.Paths;

public class DijkstraAlgorithm
{
    private Dictionary<NodeEntity, Dictionary<NodeEntity, decimal>> Graph { get; set; } = new();

    public void AddVertex(NodeEntity vertex)
    {
        if (Graph.ContainsKey(vertex) is false)
        {
            Graph[vertex] = new Dictionary<NodeEntity, decimal>();
        }
    }

    public void AddEdge(NodeEntity fromVertex, NodeEntity toVertex, decimal weight)
    {
        Graph[fromVertex][toVertex] = weight;
    }

    public Dictionary<NodeEntity, (decimal, List<NodeEntity>)> Dijkstra(NodeEntity startVertex)
    {
        var distances = new Dictionary<NodeEntity, (decimal Distance, List<NodeEntity> Nodes)>();
        var priorityQueue = new PriorityQueue<NodeEntity, decimal>();

        foreach (var vertex in Graph.Keys)
        {
            distances[vertex] = (decimal.MaxValue, new List<NodeEntity>());
            priorityQueue.Enqueue(vertex, decimal.MaxValue);
        }

        distances[startVertex] = (0m, new List<NodeEntity> { startVertex });
        priorityQueue.Enqueue(startVertex, 0m);

        while (priorityQueue.Count is not 0)
        {
            var currentVertex = priorityQueue.Dequeue();

            foreach (var neighbor in Graph[currentVertex])
            {
                var altDistance = distances[currentVertex].Distance + neighbor.Value;
                
                if (altDistance < distances[neighbor.Key].Distance)
                {
                    var newPath = new List<NodeEntity>(distances[currentVertex].Nodes) { neighbor.Key };
                    
                    distances[neighbor.Key] = (altDistance, newPath);
                    priorityQueue.Enqueue(neighbor.Key, altDistance);
                }
            }
        }

        return distances;
    }
}