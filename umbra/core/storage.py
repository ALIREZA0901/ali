from __future__ import annotations

import json
from dataclasses import asdict
from pathlib import Path
from typing import Any, Dict

from .models import AppProfile, DnsProfile, NetworkProfile, UmbraConfig


class ConfigStore:
    def __init__(self, path: Path) -> None:
        self.path = path

    def load(self) -> UmbraConfig:
        if not self.path.exists():
            return UmbraConfig()
        raw = json.loads(self.path.read_text(encoding="utf-8"))
        return self._deserialize(raw)

    def save(self, config: UmbraConfig) -> None:
        data = self._serialize(config)
        self.path.parent.mkdir(parents=True, exist_ok=True)
        self.path.write_text(json.dumps(data, indent=2, ensure_ascii=False), encoding="utf-8")

    def _serialize(self, config: UmbraConfig) -> Dict[str, Any]:
        data = asdict(config)
        data["apps"] = {
            name: {
                **app,
                "executable": str(app["executable"]),
            }
            for name, app in data["apps"].items()
        }
        return data

    def _deserialize(self, raw: Dict[str, Any]) -> UmbraConfig:
        apps = {
            name: AppProfile(
                name=payload["name"],
                executable=Path(payload["executable"]),
                arguments=payload.get("arguments", []),
                preferred_network=payload.get("preferred_network"),
                preferred_dns=payload.get("preferred_dns"),
                use_vpn=payload.get("use_vpn"),
            )
            for name, payload in raw.get("apps", {}).items()
        }
        networks = {
            name: NetworkProfile(
                name=payload["name"],
                interface=payload.get("interface"),
                gateway=payload.get("gateway"),
                subnet=payload.get("subnet"),
            )
            for name, payload in raw.get("networks", {}).items()
        }
        dns_profiles = {
            name: DnsProfile(name=payload["name"], servers=payload.get("servers", []))
            for name, payload in raw.get("dns_profiles", {}).items()
        }
        return UmbraConfig(
            apps=apps,
            networks=networks,
            dns_profiles=dns_profiles,
            vpn_profiles=raw.get("vpn_profiles", {}),
            refresh_interval_s=raw.get("refresh_interval_s", 60),
        )
