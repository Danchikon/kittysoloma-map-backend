using KittysolomaMap.Application.Users;

namespace KittysolomaMap.Infrastructure.Users;

public class AvatarGenerator : IAvatarGenerator
{
    public string GenerateUrl()
    {
        return $"https://api.dicebear.com/7.x/thumbs/svg?seed={Guid.NewGuid().ToString()}";
    }

    public string GetBaseUrl()
    {
        throw new NotImplementedException();
    }
}