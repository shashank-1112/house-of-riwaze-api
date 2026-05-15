using HouseOfRiwaze.Shared.Abstractions;

namespace Modules.Store.Core.Entities;

public class StoreSetting : BaseEntity
{
    public string StoreName { get; set; } = "House of Riwaze";

    public string Tagline { get; set; } = string.Empty;

    public string LogoUrl { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string Whatsapp { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Instagram { get; set; } = string.Empty;

    public string Facebook { get; set; } = string.Empty;

    public string DefaultMakingChargesJson { get; set; } = "{}";
}