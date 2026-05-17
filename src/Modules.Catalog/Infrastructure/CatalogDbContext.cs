using Microsoft.EntityFrameworkCore;
using Modules.Catalog.Core.Entities;

namespace Modules.Catalog.Infrastructure;

public class CatalogDbContext : DbContext
{
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductImage> ProductImages => Set<ProductImage>();
    public DbSet<ProductStone> ProductStones => Set<ProductStone>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("catalog");

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            entity.HasIndex(x => x.Sku)
                .IsUnique();

            entity.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.Sku)
                .HasMaxLength(80)
                .IsRequired();

            entity.Property(x => x.Category)
                .HasMaxLength(80)
                .IsRequired();

            entity.Property(x => x.SubCategory)
                .HasMaxLength(80);

            entity.Property(x => x.MetalType)
                .HasMaxLength(40)
                .IsRequired();

            entity.Property(x => x.JewelleryType)
                .HasMaxLength(60)
                .IsRequired()
                .HasDefaultValue("None");

            entity.Property(x => x.MetalColor)
                .HasMaxLength(60)
                .IsRequired()
                .HasDefaultValue("None");

            entity.Property(x => x.AccessoryType)
                .HasMaxLength(60)
                .IsRequired()
                .HasDefaultValue("None");

            entity.Property(x => x.Purity)
                .HasMaxLength(40)
                .IsRequired();

            entity.Property(x => x.MakingChargesType)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(x => x.Visibility)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(x => x.Gender)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(x => x.Occasion)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.GrossWeight)
                .HasPrecision(18, 3);

            entity.Property(x => x.NetWeight)
                .HasPrecision(18, 3);

            entity.Property(x => x.MakingCharges)
                .HasPrecision(18, 2);

            entity.Property(x => x.PriceOverride)
                .HasPrecision(18, 2);

            entity.Property(x => x.TryOnScale)
                .HasPrecision(10, 2);

            entity.Property(x => x.TryOnOffsetX)
                .HasPrecision(10, 2);

            entity.Property(x => x.TryOnOffsetY)
                .HasPrecision(10, 2);

            entity.Property(x => x.TryOnRotation)
                .HasPrecision(10, 2);

            entity.HasMany(x => x.Images)
                .WithOne(x => x.Product)
                .HasForeignKey("ProductId")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(x => x.Stones)
                .WithOne(x => x.Product)
                .HasForeignKey("ProductId")
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.ToTable("ProductImages");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            entity.Property<int>("ProductId");

            entity.Property(x => x.Url)
                .IsRequired();

            entity.Property(x => x.SortOrder)
                .IsRequired();

            entity.Property(x => x.IsPrimary)
                .IsRequired()
                .HasDefaultValue(false);
        });

        modelBuilder.Entity<ProductStone>(entity =>
        {
            entity.ToTable("ProductStones");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            entity.Property<int>("ProductId");

            entity.Property(x => x.StoneType)
                .HasMaxLength(80);

            entity.Property(x => x.Clarity)
                .HasMaxLength(80);

            entity.Property(x => x.Cut)
                .HasMaxLength(80);

            entity.Property(x => x.Color)
                .HasMaxLength(80);

            entity.Property(x => x.Carat)
                .HasPrecision(18, 3);

            entity.Property(x => x.Cost)
                .HasPrecision(18, 2);
        });
    }
}