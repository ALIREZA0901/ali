param(
    [string]$ZipPath = "..\Umbra_Final.zip"
)

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $PSScriptRoot

Write-Host "Packaging Umbra..."
if (Test-Path $ZipPath) { Remove-Item $ZipPath -Force }

Compress-Archive -Path (Join-Path $root "UmbraPortable"), (Join-Path $root "Source"), (Join-Path $root "BuildScripts"), (Join-Path $root "README_FA.md") -DestinationPath $ZipPath
Write-Host "Created $ZipPath"
