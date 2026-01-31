using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Umbra.Models;

namespace Umbra.Services;

public sealed class ConfigService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    public string RootPath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Umbra");
    public string ConfigPath => Path.Combine(RootPath, "config.json");
    public string ProfilesPath => Path.Combine(RootPath, "profiles.json");
    public string RoutingRulesPath => Path.Combine(RootPath, "routing_rules.json");
    public string DnsCatalogPath => Path.Combine(RootPath, "dns_catalog.json");

    public AppConfig Load()
    {
        Directory.CreateDirectory(RootPath);
        Directory.CreateDirectory(Path.Combine(RootPath, "logs"));
        Directory.CreateDirectory(Path.Combine(RootPath, "cache"));

        var config = LoadJson(ConfigPath, CreateDefaultConfig);
        EnsureProfiles();
        EnsureDnsCatalog();
        return config;
    }

    public void Save(AppConfig config)
    {
        Directory.CreateDirectory(RootPath);
        var json = JsonSerializer.Serialize(config, JsonOptions);
        File.WriteAllText(ConfigPath, json);
    }

    public List<Profile> LoadProfiles()
    {
        EnsureProfiles();
        return LoadJson(ProfilesPath, CreateDefaultProfiles);
    }

    public void SaveProfiles(List<Profile> profiles)
    {
        var json = JsonSerializer.Serialize(profiles, JsonOptions);
        File.WriteAllText(ProfilesPath, json);
    }

    public List<DnsCatalogEntry> LoadDnsCatalog()
    {
        EnsureDnsCatalog();
        return LoadJson(DnsCatalogPath, CreateDefaultDnsCatalog);
    }

    private void EnsureProfiles()
    {
        if (File.Exists(ProfilesPath))
        {
            return;
        }

        var defaults = CreateDefaultProfiles();
        File.WriteAllText(ProfilesPath, JsonSerializer.Serialize(defaults, JsonOptions));
    }

    private void EnsureDnsCatalog()
    {
        if (File.Exists(DnsCatalogPath))
        {
            return;
        }

        var seedPath = Path.Combine(AppContext.BaseDirectory, "Resources", "dns_catalog.json");
        if (File.Exists(seedPath))
        {
            File.Copy(seedPath, DnsCatalogPath, true);
            return;
        }

        var defaults = CreateDefaultDnsCatalog();
        File.WriteAllText(DnsCatalogPath, JsonSerializer.Serialize(defaults, JsonOptions));
    }

    private static AppConfig CreateDefaultConfig()
    {
        return new AppConfig();
    }

    private static List<Profile> CreateDefaultProfiles()
    {
        return new List<Profile>
        {
            new() { Name = "Streaming", IsBuiltIn = true },
            new() { Name = "Gaming", IsBuiltIn = true },
            new() { Name = "Daily", IsBuiltIn = true }
        };
    }

    private static List<DnsCatalogEntry> CreateDefaultDnsCatalog()
    {
        return new List<DnsCatalogEntry>
        {
            new() { Name = "403.online", Primary = "10.202.10.202", Secondary = "10.202.10.102", Protocols = "UDP/TCP", RegionTag = "IR", Tags = "Safe", Notes = "IR defaults" },
            new() { Name = "Shecan", Primary = "178.22.122.100", Secondary = "185.51.200.2", Protocols = "UDP/TCP", RegionTag = "IR", Tags = "Streaming", Notes = "IR defaults" },
            new() { Name = "Cloudflare", Primary = "1.1.1.1", Secondary = "1.0.0.1", Protocols = "UDP/TCP/DoH", RegionTag = "Global", Tags = "Global", Notes = "Global defaults" },
            new() { Name = "Google", Primary = "8.8.8.8", Secondary = "8.8.4.4", Protocols = "UDP/TCP/DoH", RegionTag = "Global", Tags = "Global", Notes = "Global defaults" }
        };
    }

    private static T LoadJson<T>(string path, Func<T> fallback)
    {
        try
        {
            if (!File.Exists(path))
            {
                return fallback();
            }

            var json = File.ReadAllText(path);
            var result = JsonSerializer.Deserialize<T>(json, JsonOptions);
            return result ?? fallback();
        }
        catch (JsonException)
        {
            BackupInvalidFile(path);
            return fallback();
        }
        catch (IOException)
        {
            return fallback();
        }
    }

    private static void BackupInvalidFile(string path)
    {
        if (!File.Exists(path))
        {
            return;
        }

        var backupPath = path + ".bak_" + DateTimeOffset.Now.ToUnixTimeSeconds();
        File.Copy(path, backupPath, true);
        File.Delete(path);
    }
}
