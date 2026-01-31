using System.Collections.Generic;

namespace Umbra.Models;

public sealed class Profile
{
    public string Name { get; set; } = "";
    public bool IsBuiltIn { get; set; }
    public ProfileDnsSettings Dns { get; set; } = new();
    public ProfileVpnSettings Vpn { get; set; } = new();
    public ProfileRoutingSettings Routing { get; set; } = new();
    public ProfileLoadBalanceSettings LoadBalance { get; set; } = new();
}

public sealed class ProfileDnsSettings
{
    public string SelectedServer { get; set; } = "";
    public List<string> EnabledPacks { get; set; } = new();
    public bool PendingChanges { get; set; }
}

public sealed class ProfileVpnSettings
{
    public string SelectedProfile { get; set; } = "";
    public bool PendingChanges { get; set; }
}

public sealed class ProfileRoutingSettings
{
    public bool Enabled { get; set; }
    public bool PendingChanges { get; set; }
}

public sealed class ProfileLoadBalanceSettings
{
    public string ConnectionName { get; set; } = "";
    public bool PendingChanges { get; set; }
}
