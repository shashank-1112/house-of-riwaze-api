using HouseOfRiwaze.Shared.Abstractions;

namespace Modules.Catalog.Core.Entities;

public class ProductStone : BaseEntity
{
    public string StoneType { get; set; } = string.Empty;

    public decimal Carat { get; set; }

    public string Clarity { get; set; } = string.Empty;

    public string Cut { get; set; } = string.Empty;

    public string Color { get; set; } = string.Empty;

    public decimal Cost { get; set; }

    public Product Product { get; set; } = default!;
}