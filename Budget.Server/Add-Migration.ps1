$migrationName = Read-Host "Enter the migration name"

if ([string]::IsNullOrWhiteSpace($migrationName)) {
    Write-Host "Migration name cannot be empty." -ForegroundColor Red
    exit 1
}

$outputDir = "Data/Migrations"
$command = "dotnet ef migrations add $migrationName -o `"$outputDir`""

Write-Host "Running command: $command"
Invoke-Expression $command

Write-Host "Done" -ForegroundColor Green