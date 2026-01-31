# Umbra v0.2 — Feature List (Complete)

## 1) App Launcher

* **List of “Important apps”** (پیش‌فرض‌هایی که دادی: OBS, Discord, Chrome, Steam, Spotify, TeamSpeak, Telegram)
* **Add app manually** (Browse EXE + optional arguments)
* **Auto-detect running apps** + **Refresh button**
* **Memory / Cache apps**: دفعه بعد دوباره نیاز به ست کردن از صفر نباشه
* **Start selected apps together**
* **If app is running → option to close and relaunch with selected rules**
* **Per-app profile** (هر برنامه تنظیمات جدا)

## 2) Network Binding (Per App Routing)

* انتخاب اینکه هر برنامه از کدوم شبکه بره:

  * **Modem (LAN/Wi-Fi)**
  * **Mobile tether (USB/Wi-Fi hotspot)**
  * **(Future) VDSL modem**
* “Select network for OBS” به‌صورت جدا و سریع
* نمایش Gateway / Subnet / Interface برای هر شبکه
* ذخیره انتخاب‌ها برای هر اپ
* (اختیاری) Auto-suggest بهترین شبکه برای Streaming profile (بدون تست‌های سنگین)

> نکته: “Bonding واقعی دو اینترنت با هم” معمولاً با مودم/روتر Multi-WAN یا سیستم‌عامل (policy routing) انجام می‌شه؛ ما اینجا در سطح Windows “per-app routing/binding” رو می‌زنیم.

## 3) DNS Settings (Per App)

* **Set DNS per app** (دستی)
* Preset DNS list (مثل Cloudflare/Google/Shecan/…)
* Apply / Reset per app
* ذخیره شدن DNS انتخابی برای هر اپ

## 4) UI / UX

* UI Dark و User-friendly (نزدیک به Discord / TeamSpeak)
* **Menu bar** برای تغییر سریع تنظیمات (همون “بخش ۴” که گفتی)
* **Tray icon**:

  * Minimize to tray
  * گزینه “Pause refresh when minimized” برای فشار کمتر
  * Quick actions: Launch selected / Stop VPN / Switch network profile

## 5) Auto Refresh / Monitoring

* Refresh interval preset (پیش‌فرض **60s**)
* Manual refresh button
* Toggle ON/OFF refresh
* وقتی minimize شد: refresh خاموش یا سبک‌تر بشه
* نمایش وضعیت شبکه‌ها و اپ‌ها (Running/Not running)

## 6) VPN Manager (Inside Umbra)

### هدف: “اختیاری” بودن VPN برای همه اپ‌ها

* VPN toggle کلی (ON/OFF)
* VPN per-app: هر برنامه می‌تونه “Use VPN” یا “No VPN”
* Import config:

  * **Import config from clipboard**
  * پشتیبانی از لینک‌هایی مثل:

    * `vless://...`
    * (قابل توسعه به vmess/trojan/ss و…)
* Profile manager (چند کانفیگ ذخیره و انتخاب)
* Run/Stop core from inside Umbra

### Protocol support targets (طبق حرف خودت)

* SOCKS / HTTP proxy modes
* WireGuard (اگر بعداً اضافه شد)
* Hysteria 2 (اگر بعداً اضافه شد)

> (این‌ها در عمل نیاز به engine/bridge درست دارند؛ ولی در UI و ساختار برنامه جای همه‌ش هست.)

## 7) VPN Port / Process Detection

* Auto-detect VPN ports (بدون اینکه اینترنت رو خراب کنه)
* Detect which local port is listening (مثل 10808/7890/…)
* تبدیل “روش تشخیص” به route method داخلی Umbra
* (اختیاری) نمایش نتیجه به کاربر + امکان override دستی

## 8) Safety & “No Network-Impacting Tests”

طبق چیزی که خودت قبلاً گفتی:

* **هیچ ping/speedtest/upload** انجام نشه مگر خودت صراحتاً بخوای
* همه چیز “Optional” با Accept/Deny
* هر اتومیشن شبکه قابل خاموش/روشن کردن

## 9) Streaming Helpers (OBS-focused)

* Streaming profile:

  * انتخاب شبکه برای OBS
  * نمایش bitrate پیشنهادی بر اساس وضعیت کلی (بدون تست سنگین)
  * پلتفرم‌ها: Twitch/YouTube + **Kick** + **Aparat**
* Auto bitrate suggestion (فقط با اطلاعات سیستم/تنظیمات OBS در حدی که بدون تست فعال بشه)

## 10) Future Expandability

* اضافه کردن مودم سوم (VDSL) بعداً
* افزودن VPN engineهای دیگر (Open-source):

  * OpenConnect / AnyConnect-style (با احتیاط چون بعضی‌ها proprietary هستند)
  * Cisco/OpenConnect (اگر نصبش روی سیستم باشه، Umbra فقط launcher/manager بشه)

---

## چی کم داریم که “واقعاً دقیقِ دقیق” بشه؟

برای اینکه همین لیست رو **دقیقاً مطابق همین نسخه‌ی Umbra_v2 که داری اجرا می‌کنی** بنویسم (یعنی دقیقاً چه تب‌ها، چه دکمه‌ها، چه engineهایی)، باید این‌ها رو داشته باشم:

* `main.py`
* کل پوشه‌های `ui/` و `core/` (همین نسخه Umbra_v2)
