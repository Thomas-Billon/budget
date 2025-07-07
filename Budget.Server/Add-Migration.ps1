# Prompt the user for the migration name
$migrationName = Read-Host "Enter the migration name"

# Check if a name was entered
if ([string]::IsNullOrWhiteSpace($migrationName)) {
    Write-Host "Migration name cannot be empty." -ForegroundColor Red
    exit 1
}

# Define the output directory
$outputDir = "Data/Migrations"

# Run the dotnet ef command
$command = "dotnet ef migrations add $migrationName -o `"$outputDir`""
Write-Host "Running command: $command"

Invoke-Expression $command