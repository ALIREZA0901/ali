from __future__ import annotations

from typing import Dict, List

from .models import AppProfile


class AppProfileManager:
    def __init__(self, apps: Dict[str, AppProfile]) -> None:
        self._apps = {name.lower(): app for name, app in apps.items()}

    def list_apps(self) -> List[AppProfile]:
        return sorted(self._apps.values(), key=lambda app: app.name.lower())

    def upsert(self, profile: AppProfile) -> None:
        self._apps[profile.name.lower()] = profile

    def get(self, name: str) -> AppProfile | None:
        return self._apps.get(name.lower())

    def to_dict(self) -> Dict[str, AppProfile]:
        return {profile.name: profile for profile in self._apps.values()}
