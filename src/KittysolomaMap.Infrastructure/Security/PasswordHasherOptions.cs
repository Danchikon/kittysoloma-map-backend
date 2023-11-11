namespace KittysolomaMap.Infrastructure.Security;

public record PasswordHasherOptions
{
    public const string Section = "PasswordHasher";
    
    public required string Salt { get; init; }
    public required int IterationCount { get; init; }
}