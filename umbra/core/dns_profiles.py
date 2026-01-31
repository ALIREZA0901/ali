from __future__ import annotations

from typing import Dict, List

from .models import DnsProfile

DEFAULT_DNS_PROFILES = {
    "cloudflare": ["1.1.1.1", "1.0.0.1"],
    "google": ["8.8.8.8", "8.8.4.4"],
    "shecan": ["178.22.122.100", "185.51.200.2"],
    "quad9": ["9.9.9.9", "149.112.112.112"],
}


class DnsProfileManager:
    def __init__(self, profiles: Dict[str, DnsProfile]) -> None:
        self._profiles = {name.lower(): profile for name, profile in profiles.items()}

    def ensure_defaults(self) -> None:
        for name, servers in DEFAULT_DNS_PROFILES.items():
            if name not in self._profiles:
                self._profiles[name] = DnsProfile(name=name, servers=servers)

    def list_profiles(self) -> List[DnsProfile]:
        return sorted(self._profiles.values(), key=lambda profile: profile.name.lower())

    def upsert_profile(self, profile: DnsProfile) -> None:
        self._profiles[profile.name.lower()] = profile

    def get(self, name: str) -> DnsProfile | None:
        return self._profiles.get(name.lower())

    def to_dict(self) -> Dict[str, DnsProfile]:
        return {profile.name: profile for profile in self._profiles.values()}
