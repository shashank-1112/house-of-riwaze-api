using HouseOfRiwaze.Shared.Abstractions;

namespace Modules.Catalog.Core.Entities;

public class ProductImage : BaseEntity
{
    public string Url { get; set; } = string.Empty;

    public int SortOrder { get; set; }

    public bool IsPrimary { get; set; }

    public Product Product { get; set; } = default!;
}