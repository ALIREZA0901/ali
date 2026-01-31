from __future__ import annotations

from dataclasses import dataclass, field
from pathlib import Path
from typing import Dict, List, Optional


@dataclass
class NetworkProfile:
    name: str
    interface: Optional[str] = None
    gateway: Optional[str] = None
    subnet: Optional[str] = None


@dataclass
class DnsProfile:
    name: str
    servers: List[str]


@dataclass
class AppProfile:
    name: str
    executable: Path
    arguments: List[str] = field(default_factory=list)
    preferred_network: Optional[str] = None
    preferred_dns: Optional[str] = None
    use_vpn: Optional[bool] = None


@dataclass
class UmbraConfig:
    apps: Dict[str, AppProfile] = field(default_factory=dict)
    networks: Dict[str, NetworkProfile] = field(default_factory=dict)
    dns_profiles: Dict[str, DnsProfile] = field(default_factory=dict)
    vpn_profiles: Dict[str, str] = field(default_factory=dict)
    refresh_interval_s: int = 60
