﻿# Script: Generate-AllFeatureMigrations.ps1
# Purpose: Generate EF Core migrations for each DbContext, assuming each feature folder is the project folder.

$projectDir = Resolve-Path "../"
$contextSuffix = "DbContext.cs"
$migrationName = "IgnoringDomainEvents"
$startupProject = "Invoicing.API"  # 🔁 Replace with your actual startup project folder name

Write-Host "🚀 Starting EF Core migration generation..."
Write-Host "🔍 Looking for DbContext files in: $projectDir"
Write-Host ""

# Recursively find all DbContext files
$contextFiles = Get-ChildItem -Path $projectDir -Recurse -Filter "*$contextSuffix"

if ($contextFiles.Count -eq 0) {
    Write-Host "⚠️  No DbContext files found — exiting."
    return
}

foreach ($contextFile in $contextFiles) {
    $contextPath = $contextFile.FullName
    $contextName = [System.IO.Path]::GetFileNameWithoutExtension($contextPath)
    Write-Host "🔎 Found DbContext: $contextName at $contextPath"

    # 🔼 Traverse up until we hit a folder named Invoicing.Features.*
    $currentDir = Split-Path $contextPath -Parent
    $featureDir = $null

    while ($currentDir -ne $null -and $currentDir -ne [System.IO.Path]::GetPathRoot($currentDir)) {
        $currentFolderName = Split-Path $currentDir -Leaf
        if ($currentFolderName -like "Invoicing.Features.*") {
            $featureDir = $currentDir
            break
        }
        $currentDir = Split-Path $currentDir -Parent
    }

    if (-not $featureDir) {
        Write-Host "❌ Could not determine feature folder for $contextName — skipping."
        continue
    }

    $projectPath = $featureDir  # ✅ Feature folder is the EF Core project folder
    $migrationDir = Join-Path $featureDir "Migrations"

    Write-Host "📦 Feature folder (used as project): $featureDir"
    Write-Host "📁 Migrations output directory: $migrationDir"

    # 🛠️ Build the EF migration command
    $migrationCommand = @(
        "dotnet ef migrations add $migrationName",
        "--context $contextName",
        "--output-dir `"$migrationDir`"",
        "--project `"$projectPath`"",
        "--startup-project `"$projectDir\$startupProject`"",
        "--verbose"
    ) -join " "

    Write-Host "⚙️  Running EF command:"
    Write-Host $migrationCommand
    Write-Host ""

    $output = & cmd /c "$migrationCommand" 2>&1

    if ($LASTEXITCODE -eq 0) {
        if ($output -match "No changes were detected in the model") {
            Write-Host "✅ No model changes detected for $contextName — skipping migration."
        } else {
            Write-Host "✅ Migration created for $contextName"
        }
    } else {
        Write-Host "❌ Failed to create migration for $contextName"
        Write-Host "🪵 EF Output:"
        Write-Host $output
    }

    Write-Host "--------------------------------------------------------"
}

Write-Host ""
Write-Host "🏁 All migrations attempted. Done."
