from __future__ import annotations

import platform
import subprocess
from dataclasses import dataclass
from typing import List


@dataclass
class ListeningPort:
    port: int
    protocol: str
    process: str | None = None


def detect_listening_ports() -> List[ListeningPort]:
    system = platform.system().lower()
    if system == "windows":
        return _detect_windows()
    return _detect_unix()


def _detect_windows() -> List[ListeningPort]:
    output = subprocess.check_output(["netstat", "-ano"], text=True, errors="ignore")
    ports: List[ListeningPort] = []
    for line in output.splitlines():
        if not line.strip().lower().startswith("tcp"):
            continue
        parts = line.split()
        if len(parts) < 4:
            continue
        local = parts[1]
        state = parts[3]
        if state.upper() != "LISTENING":
            continue
        port = _extract_port(local)
        if port:
            ports.append(ListeningPort(port=port, protocol="tcp"))
    return ports


def _detect_unix() -> List[ListeningPort]:
    output = subprocess.check_output(["ss", "-ltn"], text=True, errors="ignore")
    ports: List[ListeningPort] = []
    for line in output.splitlines()[1:]:
        parts = line.split()
        if len(parts) < 4:
            continue
        local = parts[3]
        port = _extract_port(local)
        if port:
            ports.append(ListeningPort(port=port, protocol="tcp"))
    return ports


def _extract_port(value: str) -> int | None:
    if ":" not in value:
        return None
    try:
        return int(value.rsplit(":", 1)[-1])
    except ValueError:
        return None
