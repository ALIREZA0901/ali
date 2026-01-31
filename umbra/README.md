# Umbra v0.2 Scaffold

این پوشه یک اسکلت اولیه برای Umbra است تا بتوانید توسعه‌ی UI و هسته‌ی برنامه را از همین‌جا شروع کنید.

## شروع سریع

```bash
python -m umbra.main init
python -m umbra.main list-apps --running
python -m umbra.main add-app OBS "C:\\Program Files\\obs-studio\\bin\\64bit\\obs64.exe"
python -m umbra.main launch OBS
```

## پوشه‌ها

- `core/`: منطق پایه (مدیریت اپ‌ها، پروفایل شبکه، VPN و ذخیره‌سازی تنظیمات)
- `ui/`: جایگاه کد UI (فعلاً placeholder)
