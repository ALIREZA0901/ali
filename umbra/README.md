# Umbra v0.2 Scaffold

این پوشه یک اسکلت اولیه برای Umbra است تا بتوانید توسعه‌ی UI و هسته‌ی برنامه را از همین‌جا شروع کنید.

## شروع سریع

```bash
python -m umbra.main init
python -m umbra.main list-apps --running
python -m umbra.main add-app OBS "C:\\Program Files\\obs-studio\\bin\\64bit\\obs64.exe"
python -m umbra.main set-app-network OBS Modem
python -m umbra.main set-app-dns OBS cloudflare
python -m umbra.main set-app-vpn OBS on
python -m umbra.main launch OBS
python -m umbra.main status
```

## دستورات تکمیلی

```bash
python -m umbra.main list-networks --detect
python -m umbra.main add-network Modem --interface "Wi-Fi" --gateway 192.168.1.1 --subnet 255.255.255.0
python -m umbra.main list-dns
python -m umbra.main add-dns Streaming 1.1.1.1 1.0.0.1
python -m umbra.main list-vpn
python -m umbra.main add-vpn Main "vless://..."
python -m umbra.main detect-vpn-ports
python -m umbra.main stop OBS --force
python -m umbra.main relaunch OBS --force
```

## پوشه‌ها

- `core/`: منطق پایه (مدیریت اپ‌ها، پروفایل شبکه، VPN و ذخیره‌سازی تنظیمات)
- `ui/`: جایگاه کد UI (فعلاً placeholder)
