from __future__ import annotations

import platform
import subprocess
from pathlib import Path
from typing import Iterable, List, Set

from .models import AppProfile

DEFAULT_IMPORTANT_APPS = [
    "OBS",
    "Discord",
    "Chrome",
    "Steam",
    "Spotify",
    "TeamSpeak",
    "Telegram",
]


class AppLauncher:
    def __init__(self, apps: Iterable[AppProfile]) -> None:
        self._apps = {app.name.lower(): app for app in apps}

    def list_apps(self) -> List[AppProfile]:
        return sorted(self._apps.values(), key=lambda app: app.name.lower())

    def add_app(self, app: AppProfile) -> None:
        self._apps[app.name.lower()] = app

    def running_apps(self) -> Set[str]:
        system = platform.system().lower()
        if system == "windows":
            return self._running_apps_windows()
        return self._running_apps_unix()

    def launch_apps(self, names: Iterable[str]) -> List[subprocess.Popen[bytes]]:
        processes: List[subprocess.Popen[bytes]] = []
        for name in names:
            app = self._apps.get(name.lower())
            if not app:
                continue
            command = [str(app.executable), *app.arguments]
            processes.append(subprocess.Popen(command))
        return processes

    def _running_apps_windows(self) -> Set[str]:
        output = subprocess.check_output(["tasklist"], text=True, errors="ignore")
        return {line.split()[0].lower() for line in output.splitlines()[3:] if line.strip()}

    def _running_apps_unix(self) -> Set[str]:
        output = subprocess.check_output(["ps", "-eo", "comm"], text=True, errors="ignore")
        return {Path(line.strip()).name.lower() for line in output.splitlines()[1:] if line.strip()}
