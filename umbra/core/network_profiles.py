from __future__ import annotations

from typing import Dict, List

from .models import NetworkProfile


class NetworkProfileManager:
    def __init__(self, profiles: Dict[str, NetworkProfile]) -> None:
        self._profiles = {name.lower(): profile for name, profile in profiles.items()}

    def list_profiles(self) -> List[NetworkProfile]:
        return sorted(self._profiles.values(), key=lambda profile: profile.name.lower())

    def upsert_profile(self, profile: NetworkProfile) -> None:
        self._profiles[profile.name.lower()] = profile

    def get(self, name: str) -> NetworkProfile | None:
        return self._profiles.get(name.lower())

    def to_dict(self) -> Dict[str, NetworkProfile]:
        return {profile.name: profile for profile in self._profiles.values()}
