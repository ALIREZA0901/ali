namespace Umbra.Models;

public sealed class DnsCatalogEntry
{
    public string Name { get; set; } = "";
    public string Primary { get; set; } = "";
    public string Secondary { get; set; } = "";
    public string Protocols { get; set; } = "";
    public string RegionTag { get; set; } = "Unknown";
    public string Tags { get; set; } = "";
    public string Notes { get; set; } = "";
}
