using System;
using System.Collections.Generic;

namespace Umbra.Models;

public sealed class AppConfig
{
    public bool FirstRunCompleted { get; set; }
    public string CopilotLevel { get; set; } = "Helpful";
    public bool EnableIrDefaults { get; set; } = true;
    public bool EnableGlobalDefaults { get; set; } = true;
    public bool LightChecksAllowed { get; set; }
    public string Theme { get; set; } = "Dark";
    public string Language { get; set; } = "FA";

    public DiagnosticsStatus Diagnostics { get; set; } = new();
    public StatusSnapshot Status { get; set; } = new();

    public Dictionary<string, bool> SuppressedConfirmations { get; set; } = new();
}

public sealed class DiagnosticsStatus
{
    public DateTimeOffset? LastSpeedTest { get; set; }
    public DateTimeOffset? LastStreamingAparat { get; set; }
    public DateTimeOffset? LastStreamingKick { get; set; }
    public DateTimeOffset? LastDnsOptimize { get; set; }
    public DateTimeOffset? LastReachability { get; set; }
}

public sealed class StatusSnapshot
{
    public string ActiveProfileName { get; set; } = "Daily";
    public string DnsStatus { get; set; } = "Off";
    public string VpnStatus { get; set; } = "Off";
    public string RoutingStatus { get; set; } = "Off";
}
