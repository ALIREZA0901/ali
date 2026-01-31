from __future__ import annotations

import platform
import subprocess
from dataclasses import dataclass
from typing import List


@dataclass
class NetworkInterfaceInfo:
    name: str
    address: str | None = None
    gateway: str | None = None
    subnet: str | None = None


def detect_interfaces() -> List[NetworkInterfaceInfo]:
    system = platform.system().lower()
    if system == "windows":
        return _detect_windows()
    return _detect_unix()


def _detect_windows() -> List[NetworkInterfaceInfo]:
    output = subprocess.check_output(["ipconfig"], text=True, errors="ignore")
    interfaces: List[NetworkInterfaceInfo] = []
    current: NetworkInterfaceInfo | None = None
    for line in output.splitlines():
        line = line.strip()
        if not line:
            continue
        if line.endswith(":") and "adapter" in line.lower():
            name = line.split("adapter", 1)[-1].strip(" :")
            current = NetworkInterfaceInfo(name=name)
            interfaces.append(current)
            continue
        if current is None:
            continue
        if "IPv4 Address" in line:
            current.address = line.split(":", 1)[-1].strip()
        if "Subnet Mask" in line:
            current.subnet = line.split(":", 1)[-1].strip()
        if "Default Gateway" in line:
            current.gateway = line.split(":", 1)[-1].strip()
    return interfaces


def _detect_unix() -> List[NetworkInterfaceInfo]:
    output = subprocess.check_output(["ip", "-4", "addr", "show"], text=True, errors="ignore")
    interfaces: List[NetworkInterfaceInfo] = []
    current: NetworkInterfaceInfo | None = None
    for line in output.splitlines():
        if not line:
            continue
        if line[0].isdigit():
            name = line.split(":", 2)[1].strip()
            current = NetworkInterfaceInfo(name=name)
            interfaces.append(current)
            continue
        if current is None:
            continue
        if "inet " in line:
            address = line.strip().split()[1]
            current.address = address
            current.subnet = address.split("/")[-1]
    return interfaces
