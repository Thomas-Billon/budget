$certificateName = 'budget.client'
$baseFolder = Join-Path $env:APPDATA 'ASP.NET\https'
$certFilePath = Join-Path $baseFolder "$certificateName.pem"
$keyFilePath = Join-Path $baseFolder "$certificateName.key"

Write-Host "Removing stale certificate files"
foreach ($path in @($certFilePath, $keyFilePath)) {
    if (Test-Path $path) {
        Remove-Item $path -Force
        Write-Host "- Removed: $path"
    } else {
        Write-Host "- Not found: $path"
    }
}

Write-Host "Cleaning existing dev certificates"
dotnet dev-certs https --clean

Write-Host "Generating and trusting a new dev certificate"
dotnet dev-certs https --trust

Write-Host "Done" -ForegroundColor Green
