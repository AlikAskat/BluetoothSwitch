$ErrorActionPreference = "Stop"

if (-not (Test-Path ".git")) {
    throw "Запусти этот скрипт из корня репозитория WinUnburySite."
}

Write-Host "=== WinUnbury: BluetoothSwitch product page ===" -ForegroundColor Cyan

New-Item -ItemType Directory -Path ".\bluetoothswitch" -Force | Out-Null

@'
<!DOCTYPE html>
<html lang="en">
<head>
<meta charset="UTF-8">
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<title>BluetoothSwitch — Bluetooth Toggle for Windows 11 | WinUnbury</title>
<meta name="description" content="BluetoothSwitch is a tiny Windows 11 tray utility for quickly turning Bluetooth on and off. Free and open source.">
<meta name="robots" content="index, follow">
<link rel="canonical" href="https://winunbury-site.pages.dev/bluetoothswitch/">
<meta property="og:title" content="BluetoothSwitch — Bluetooth Toggle for Windows 11">
<meta property="og:description" content="A tiny Windows 11 tray utility for quickly turning Bluetooth on and off.">
<meta property="og:type" content="website">
<meta property="og:url" content="https://winunbury-site.pages.dev/bluetoothswitch/">
<script type="application/ld+json">
{
  "@context":"https://schema.org",
  "@type":"SoftwareApplication",
  "name":"BluetoothSwitch",
  "applicationCategory":"UtilitiesApplication",
  "operatingSystem":"Windows 11",
  "description":"A lightweight Windows 11 system tray utility for quickly toggling Bluetooth on and off.",
  "url":"https://winunbury-site.pages.dev/bluetoothswitch/",
  "license":"https://opensource.org/license/mit/"
}
</script>
<link rel="stylesheet" href="../style.css">
<style>
.product-hero{padding:100px 0 80px}.product-hero h1{max-width:800px;margin:18px 0 20px;font-size:clamp(44px,7vw,72px);line-height:1.02;letter-spacing:-3px}.product-hero p{max-width:700px;color:var(--muted);font-size:20px}.product-actions{display:flex;flex-wrap:wrap;gap:14px;margin-top:30px}.button.secondary{background:transparent;border:1px solid var(--line)}.feature-grid{display:grid;grid-template-columns:repeat(2,minmax(0,1fr));gap:16px;margin-top:28px}.feature{padding:24px;border:1px solid var(--line);border-radius:14px;background:var(--surface)}.feature h2{margin:0 0 8px;font-size:20px}.feature p{margin:0;color:var(--muted)}.back{display:inline-block;margin-top:28px;color:var(--accent);font-weight:700}@media(max-width:650px){.feature-grid{grid-template-columns:1fr}.product-hero{padding:70px 0 60px}.product-hero h1{letter-spacing:-2px}}
</style>
</head>
<body>
<header class="site-header"><div class="container header-inner">
<a class="logo" href="../">WinUnbury</a>
<nav><a href="../#products" data-en="Products" data-ru="Программы">Products</a><a href="../#about" data-en="About" data-ru="О проекте">About</a><button id="languageButton" type="button">RU</button></nav>
</div></header>
<main>
<section class="product-hero"><div class="container">
<div class="eyebrow" data-en="WINDOWS 11 UTILITY" data-ru="УТИЛИТА ДЛЯ WINDOWS 11">WINDOWS 11 UTILITY</div>
<h1>BluetoothSwitch</h1>
<p data-en="A tiny system tray utility that lets you turn Bluetooth on and off with a single click." data-ru="Небольшая утилита в системном трее, которая позволяет включать и выключать Bluetooth одним нажатием.">A tiny system tray utility that lets you turn Bluetooth on and off with a single click.</p>
<div class="product-actions"><a class="button" href="https://github.com/AlikAskat/BluetoothSwitch" target="_blank" rel="noopener" data-en="View on GitHub →" data-ru="Открыть на GitHub →">View on GitHub →</a><a class="button secondary" href="../" data-en="Back to WinUnbury" data-ru="Вернуться в WinUnbury">Back to WinUnbury</a></div>
</div></section>
<section class="products"><div class="container"><div class="section-label" data-en="WHAT IT DOES" data-ru="ЧТО ОНА ДЕЛАЕТ">WHAT IT DOES</div><div class="feature-grid">
<div class="feature"><h2 data-en="One-click toggle" data-ru="Переключение одним кликом">One-click toggle</h2><p data-en="Left-click the tray icon to switch Bluetooth on or off." data-ru="Левый клик по значку в трее включает или выключает Bluetooth.">Left-click the tray icon to switch Bluetooth on or off.</p></div>
<div class="feature"><h2 data-en="Always visible status" data-ru="Статус всегда под рукой">Always visible status</h2><p data-en="The icon shows whether Bluetooth is on, off, or unavailable." data-ru="Значок показывает, включён Bluetooth, выключен или его состояние недоступно.">The icon shows whether Bluetooth is on, off, or unavailable.</p></div>
<div class="feature"><h2 data-en="Windows settings" data-ru="Настройки Windows">Windows settings</h2><p data-en="Open Bluetooth settings directly from the tray menu." data-ru="Открывайте настройки Bluetooth прямо из меню значка.">Open Bluetooth settings directly from the tray menu.</p></div>
<div class="feature"><h2 data-en="No admin rights required" data-ru="Без прав администратора">No admin rights required</h2><p data-en="Normal operation does not require administrator privileges." data-ru="Для обычной работы права администратора не требуются.">Normal operation does not require administrator privileges.</p></div>
</div></div></section>
<section class="about"><div class="container"><div class="section-label" data-en="HOW IT WORKS" data-ru="КАК ЭТО РАБОТАЕТ">HOW IT WORKS</div>
<h2 data-en="Uses the Windows Radio API." data-ru="Использует Windows Radio API.">Uses the Windows Radio API.</h2>
<p data-en="BluetoothSwitch uses the Windows Runtime Windows.Devices.Radios.Radio API to control the Bluetooth radio. It does not disable or enable the Bluetooth device through Device Manager or Plug and Play." data-ru="BluetoothSwitch использует Windows Runtime API Windows.Devices.Radios.Radio для управления Bluetooth. Устройство не отключается и не включается через Диспетчер устройств или Plug and Play.">BluetoothSwitch uses the Windows Runtime Windows.Devices.Radios.Radio API to control the Bluetooth radio. It does not disable or enable the Bluetooth device through Device Manager or Plug and Play.</p>
<a class="back" href="https://github.com/AlikAskat/BluetoothSwitch" target="_blank" rel="noopener" data-en="Source code and build instructions →" data-ru="Исходный код и инструкция по сборке →">Source code and build instructions →</a>
</div></section>
</main>
<footer><div class="container footer-inner"><span>© 2026 WinUnbury</span><a href="https://github.com/AlikAskat" target="_blank" rel="noopener">GitHub</a></div></footer>
<script>
const languageButton=document.getElementById("languageButton");
function setLanguage(language){document.documentElement.lang=language;document.querySelectorAll("[data-en][data-ru]").forEach(e=>e.textContent=e.dataset[language]);languageButton.textContent=language==="en"?"RU":"EN";document.title=language==="en"?"BluetoothSwitch — Bluetooth Toggle for Windows 11 | WinUnbury":"BluetoothSwitch — Переключатель Bluetooth для Windows 11 | WinUnbury";}
languageButton.addEventListener("click",()=>setLanguage(document.documentElement.lang==="en"?"ru":"en"));
</script>
</body></html>
'@ | Set-Content -Path ".\bluetoothswitch\index.html" -Encoding UTF8

@'
<?xml version="1.0" encoding="UTF-8"?>
<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
  <url><loc>https://winunbury-site.pages.dev/</loc></url>
  <url><loc>https://winunbury-site.pages.dev/bluetoothswitch/</loc></url>
</urlset>
'@ | Set-Content -Path ".\sitemap.xml" -Encoding UTF8

Write-Host "Проверка Git..." -ForegroundColor Yellow
git diff --check
if ($LASTEXITCODE -ne 0) { throw "git diff --check обнаружил ошибку." }

git status

git add ".\bluetoothswitch\index.html" ".\sitemap.xml"
git commit -m "Add BluetoothSwitch product page"
git push origin main

Write-Host "" 
Write-Host "ГОТОВО: изменения отправлены на GitHub." -ForegroundColor Green
Write-Host "Cloudflare Pages автоматически запустит новый deploy." -ForegroundColor Green
Write-Host "Страница: https://winunbury-site.pages.dev/bluetoothswitch/" -ForegroundColor Cyan
