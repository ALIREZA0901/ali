from __future__ import annotations

import argparse
from pathlib import Path

from umbra.core.app_launcher import AppLauncher, DEFAULT_IMPORTANT_APPS
from umbra.core.app_profiles import AppProfileManager
from umbra.core.dns_profiles import DnsProfileManager
from umbra.core.models import AppProfile, DnsProfile, NetworkProfile, UmbraConfig
from umbra.core.network_detection import detect_interfaces
from umbra.core.network_profiles import NetworkProfileManager
from umbra.core.storage import ConfigStore
from umbra.core.vpn_detection import detect_listening_ports
from umbra.core.vpn_manager import VpnManager

DEFAULT_CONFIG_PATH = Path.home() / ".umbra" / "config.json"


def build_parser() -> argparse.ArgumentParser:
    parser = argparse.ArgumentParser(description="Umbra v0.2 scaffold")
    subparsers = parser.add_subparsers(dest="command", required=True)

    list_apps = subparsers.add_parser("list-apps", help="List configured apps")
    list_apps.add_argument("--running", action="store_true", help="Show running apps")

    add_app = subparsers.add_parser("add-app", help="Add or update an app")
    add_app.add_argument("name")
    add_app.add_argument("executable")
    add_app.add_argument("--arg", action="append", default=[])

    set_network = subparsers.add_parser("set-app-network", help="Set preferred network for an app")
    set_network.add_argument("name")
    set_network.add_argument("network")

    set_dns = subparsers.add_parser("set-app-dns", help="Set preferred DNS profile for an app")
    set_dns.add_argument("name")
    set_dns.add_argument("dns")

    set_vpn = subparsers.add_parser("set-app-vpn", help="Set VPN usage for an app")
    set_vpn.add_argument("name")
    set_vpn.add_argument("mode", choices=["on", "off", "inherit"])

    launch = subparsers.add_parser("launch", help="Launch configured apps")
    launch.add_argument("names", nargs="+")

    stop = subparsers.add_parser("stop", help="Stop running apps")
    stop.add_argument("names", nargs="+")
    stop.add_argument("--force", action="store_true")

    relaunch = subparsers.add_parser("relaunch", help="Stop and relaunch apps")
    relaunch.add_argument("names", nargs="+")
    relaunch.add_argument("--force", action="store_true")

    list_networks = subparsers.add_parser("list-networks", help="List detected and stored networks")
    list_networks.add_argument("--detect", action="store_true", help="Include detected interfaces")

    add_network = subparsers.add_parser("add-network", help="Add or update a network profile")
    add_network.add_argument("name")
    add_network.add_argument("--interface")
    add_network.add_argument("--gateway")
    add_network.add_argument("--subnet")

    list_dns = subparsers.add_parser("list-dns", help="List DNS profiles")

    add_dns = subparsers.add_parser("add-dns", help="Add or update DNS profile")
    add_dns.add_argument("name")
    add_dns.add_argument("servers", nargs="+")

    list_vpn = subparsers.add_parser("list-vpn", help="List VPN profiles")

    add_vpn = subparsers.add_parser("add-vpn", help="Add VPN profile from raw config")
    add_vpn.add_argument("name")
    add_vpn.add_argument("config")

    detect_vpn_ports = subparsers.add_parser("detect-vpn-ports", help="Detect listening VPN ports")

    status = subparsers.add_parser("status", help="Show app status and profiles")

    init = subparsers.add_parser("init", help="Initialize config with defaults")
    init.add_argument("--force", action="store_true")

    return parser


def ensure_defaults(config: UmbraConfig) -> UmbraConfig:
    if not config.apps:
        for app_name in DEFAULT_IMPORTANT_APPS:
            config.apps[app_name] = AppProfile(
                name=app_name,
                executable=Path(app_name),
                arguments=[],
            )
    return config


def persist_config(
    store: ConfigStore,
    config: UmbraConfig,
    app_manager: AppProfileManager,
    dns_manager: DnsProfileManager,
    network_manager: NetworkProfileManager,
    vpn_manager: VpnManager,
) -> None:
    config.apps = app_manager.to_dict()
    config.dns_profiles = dns_manager.to_dict()
    config.networks = network_manager.to_dict()
    config.vpn_profiles = {profile.name: profile.raw_config for profile in vpn_manager.list_profiles()}
    store.save(config)


def main() -> None:
    parser = build_parser()
    args = parser.parse_args()

    store = ConfigStore(DEFAULT_CONFIG_PATH)
    config = store.load()

    if args.command == "init":
        if DEFAULT_CONFIG_PATH.exists() and not args.force:
            parser.error("Config already exists. Use --force to overwrite.")
        config = ensure_defaults(UmbraConfig())
        store.save(config)
        print(f"Initialized config at {DEFAULT_CONFIG_PATH}")
        return

    config = ensure_defaults(config)
    app_manager = AppProfileManager(config.apps)
    dns_manager = DnsProfileManager(config.dns_profiles)
    dns_manager.ensure_defaults()
    network_manager = NetworkProfileManager(config.networks)
    vpn_manager = VpnManager(config.vpn_profiles)
    launcher = AppLauncher(app_manager.list_apps())

    if args.command == "list-apps":
        running = launcher.running_apps() if args.running else set()
        for app in app_manager.list_apps():
            status = "running" if app.executable.name.lower() in running else "stopped"
            network = app.preferred_network or "inherit"
            dns = app.preferred_dns or "inherit"
            vpn = "inherit" if app.use_vpn is None else ("on" if app.use_vpn else "off")
            print(f"{app.name} -> {app.executable} ({status}) [net={network}, dns={dns}, vpn={vpn}]")
        return

    if args.command == "add-app":
        profile = AppProfile(
            name=args.name,
            executable=Path(args.executable),
            arguments=args.arg,
        )
        app_manager.upsert(profile)
        persist_config(store, config, app_manager, dns_manager, network_manager, vpn_manager)
        print(f"Saved {args.name}.")
        return

    if args.command == "set-app-network":
        profile = app_manager.get(args.name)
        if not profile:
            parser.error(f"Unknown app: {args.name}")
        profile.preferred_network = args.network
        app_manager.upsert(profile)
        persist_config(store, config, app_manager, dns_manager, network_manager, vpn_manager)
        print(f"Set network for {args.name} -> {args.network}")
        return

    if args.command == "set-app-dns":
        profile = app_manager.get(args.name)
        if not profile:
            parser.error(f"Unknown app: {args.name}")
        profile.preferred_dns = args.dns
        app_manager.upsert(profile)
        persist_config(store, config, app_manager, dns_manager, network_manager, vpn_manager)
        print(f"Set DNS for {args.name} -> {args.dns}")
        return

    if args.command == "set-app-vpn":
        profile = app_manager.get(args.name)
        if not profile:
            parser.error(f"Unknown app: {args.name}")
        if args.mode == "inherit":
            profile.use_vpn = None
        else:
            profile.use_vpn = args.mode == "on"
        app_manager.upsert(profile)
        persist_config(store, config, app_manager, dns_manager, network_manager, vpn_manager)
        print(f"Set VPN for {args.name} -> {args.mode}")
        return

    if args.command == "launch":
        processes = launcher.launch_apps(args.names)
        print(f"Launched {len(processes)} app(s).")
        return

    if args.command == "stop":
        launcher.stop_apps(args.names, force=args.force)
        print(f"Stopped {len(args.names)} app(s).")
        return

    if args.command == "relaunch":
        launcher.stop_apps(args.names, force=args.force)
        processes = launcher.launch_apps(args.names)
        print(f"Relaunched {len(processes)} app(s).")
        return

    if args.command == "list-networks":
        for profile in network_manager.list_profiles():
            print(
                f"{profile.name} -> {profile.interface or '-'} gateway={profile.gateway or '-'} subnet={profile.subnet or '-'}"
            )
        if args.detect:
            print("--- detected interfaces ---")
            for interface in detect_interfaces():
                print(
                    f"{interface.name} addr={interface.address or '-'} gateway={interface.gateway or '-'} subnet={interface.subnet or '-'}"
                )
        return

    if args.command == "add-network":
        profile = NetworkProfile(
            name=args.name,
            interface=args.interface,
            gateway=args.gateway,
            subnet=args.subnet,
        )
        network_manager.upsert_profile(profile)
        persist_config(store, config, app_manager, dns_manager, network_manager, vpn_manager)
        print(f"Saved network {args.name}.")
        return

    if args.command == "list-dns":
        for profile in dns_manager.list_profiles():
            servers = ", ".join(profile.servers)
            print(f"{profile.name}: {servers}")
        return

    if args.command == "add-dns":
        profile = DnsProfile(name=args.name, servers=args.servers)
        dns_manager.upsert_profile(profile)
        persist_config(store, config, app_manager, dns_manager, network_manager, vpn_manager)
        print(f"Saved DNS profile {args.name}.")
        return

    if args.command == "list-vpn":
        for profile in vpn_manager.list_profiles():
            print(profile.name)
        return

    if args.command == "add-vpn":
        vpn_manager.add_profile(args.name, args.config)
        persist_config(store, config, app_manager, dns_manager, network_manager, vpn_manager)
        print(f"Saved VPN profile {args.name}.")
        return

    if args.command == "detect-vpn-ports":
        ports = detect_listening_ports()
        for port in ports:
            print(f"{port.protocol.upper()} {port.port}")
        return

    if args.command == "status":
        running = launcher.running_apps()
        for app in app_manager.list_apps():
            status = "running" if app.executable.name.lower() in running else "stopped"
            network = app.preferred_network or "inherit"
            dns = app.preferred_dns or "inherit"
            vpn = "inherit" if app.use_vpn is None else ("on" if app.use_vpn else "off")
            print(f"{app.name}: {status} | net={network} dns={dns} vpn={vpn}")
        return

    parser.error("Unsupported command.")


if __name__ == "__main__":
    main()
