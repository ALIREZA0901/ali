from __future__ import annotations

import argparse
from pathlib import Path

from umbra.core.app_launcher import AppLauncher, DEFAULT_IMPORTANT_APPS
from umbra.core.models import AppProfile, UmbraConfig
from umbra.core.network_profiles import NetworkProfileManager
from umbra.core.storage import ConfigStore
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

    launch = subparsers.add_parser("launch", help="Launch configured apps")
    launch.add_argument("names", nargs="+")

    init = subparsers.add_parser("init", help="Initialize config with defaults")
    init.add_argument("--force", action="store_true")

    return parser


def ensure_defaults(config: UmbraConfig) -> UmbraConfig:
    if not config.apps:
        for app_name in DEFAULT_IMPORTANT_APPS:
            config.apps[app_name.lower()] = AppProfile(
                name=app_name,
                executable=Path(app_name),
                arguments=[],
            )
    return config


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
    launcher = AppLauncher(config.apps.values())
    network_manager = NetworkProfileManager(config.networks)
    vpn_manager = VpnManager(config.vpn_profiles)

    if args.command == "list-apps":
        running = launcher.running_apps() if args.running else set()
        for app in launcher.list_apps():
            status = "running" if app.executable.name.lower() in running else "stopped"
            print(f"{app.name} -> {app.executable} ({status})")
        return

    if args.command == "add-app":
        profile = AppProfile(
            name=args.name,
            executable=Path(args.executable),
            arguments=args.arg,
        )
        config.apps[args.name.lower()] = profile
        store.save(config)
        print(f"Saved {args.name}.")
        return

    if args.command == "launch":
        processes = launcher.launch_apps(args.names)
        print(f"Launched {len(processes)} app(s).")
        return

    parser.error("Unsupported command.")


if __name__ == "__main__":
    main()
