param(
    [string]$Configuration = "Release",
    [string]$OutputDir = "..\UmbraPortable"
)

$ErrorActionPreference = "Stop"

$root = Split-Path -Parent $PSScriptRoot
$solution = Join-Path $root "Source\Umbra.sln"

Write-Host "Building Umbra..."
& dotnet publish $solution -c $Configuration -r win-x64 --self-contained true -p:PublishSingleFile=true -o $OutputDir

Write-Host "Build output: $OutputDir"
