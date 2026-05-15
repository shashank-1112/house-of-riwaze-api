using Microsoft.EntityFrameworkCore;
using Modules.Store.Core.Entities;

namespace Modules.Store.Infrastructure;

public class StoreDbContext : DbContext
{
    public StoreDbContext(DbContextOptions<StoreDbContext> options)
        : base(options)
    {
    }

    public DbSet<MetalRate> MetalRates => Set<MetalRate>();

    public DbSet<StoreSetting> StoreSettings => Set<StoreSetting>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("store");

        modelBuilder.Entity<MetalRate>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Gold24kPerGram).HasPrecision(18, 4);
            entity.Property(x => x.Gold22kPerGram).HasPrecision(18, 4);
            entity.Property(x => x.Gold18kPerGram).HasPrecision(18, 4);

            entity.Property(x => x.Silver999PerGram).HasPrecision(18, 4);
            entity.Property(x => x.PlatinumPerGram).HasPrecision(18, 4);

            entity.Property(x => x.McxGoldPerGram).HasPrecision(18, 4);
            entity.Property(x => x.McxSilverPerGram).HasPrecision(18, 4);
            entity.Property(x => x.IbjaGoldPerGram).HasPrecision(18, 4);

            entity.Property(x => x.Currency).HasMaxLength(10).IsRequired();
            entity.Property(x => x.Unit).HasMaxLength(10).IsRequired();
            entity.Property(x => x.Source).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<StoreSetting>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.StoreName)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.Tagline)
                .HasMaxLength(300);

            entity.Property(x => x.LogoUrl)
                .HasMaxLength(1000);

            entity.Property(x => x.Address)
                .HasMaxLength(1000);

            entity.Property(x => x.Whatsapp)
                .HasMaxLength(50);

            entity.Property(x => x.Email)
                .HasMaxLength(200);

            entity.Property(x => x.Instagram)
                .HasMaxLength(500);

            entity.Property(x => x.Facebook)
                .HasMaxLength(500);

            entity.Property(x => x.DefaultMakingChargesJson)
                .HasColumnType("jsonb")
                .HasDefaultValue("{}");
        });
    }
}