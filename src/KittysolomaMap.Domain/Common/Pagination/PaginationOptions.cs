namespace KittysolomaMap.Domain.Common.Pagination;

public record PaginationOptions
{
    public const int DefaultPage = 0;
    public const int DefaultPageSize = 10;

    public static PaginationOptions Default => new PaginationOptions { Page = DefaultPage, PageSize = DefaultPageSize };

    public int Page { get; init; } = DefaultPage;
    public int PageSize { get; init; } = DefaultPageSize;
};