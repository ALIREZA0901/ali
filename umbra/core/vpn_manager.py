from __future__ import annotations

from dataclasses import dataclass
from typing import Dict, List, Optional


@dataclass
class VpnProfile:
    name: str
    raw_config: str


class VpnManager:
    def __init__(self, profiles: Dict[str, str] | None = None) -> None:
        self._profiles: Dict[str, VpnProfile] = {}
        if profiles:
            for name, raw in profiles.items():
                self._profiles[name.lower()] = VpnProfile(name=name, raw_config=raw)

    def list_profiles(self) -> List[VpnProfile]:
        return sorted(self._profiles.values(), key=lambda profile: profile.name.lower())

    def add_profile(self, name: str, raw_config: str) -> None:
        self._profiles[name.lower()] = VpnProfile(name=name, raw_config=raw_config)

    def get(self, name: str) -> Optional[VpnProfile]:
        return self._profiles.get(name.lower())
