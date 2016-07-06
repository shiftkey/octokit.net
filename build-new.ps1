$repositoryRoot = split-path $MyInvocation.MyCommand.Definition
$toolsPath = join-path $repositoryRoot ".dotnet"
$getDotNet = join-path $toolsPath "dotnet-install.ps1"
$nugetExePath = join-path $toolsPath "nuget.exe"

write-host "Download latest install script from CLI repo"

New-Item -type directory -f -path $toolsPath | Out-Null

Invoke-WebRequest https://raw.githubusercontent.com/dotnet/cli/rel/1.0.0-preview2/scripts/obtain/dotnet-install.ps1 -OutFile $getDotNet

$env:DOTNET_INSTALL_DIR="$repositoryRoot\.dotnet\win7-x64"

if (!(Test-Path $env:DOTNET_INSTALL_DIR)) {
    New-Item -type directory -path $env:DOTNET_INSTALL_DIR | Out-Null
}

& $getDotNet -arch x64

$env:PATH = "$env:DOTNET_INSTALL_DIR;$env:PATH"

Write-Host "Building Octokit" -ForegroundColor Green
.\build-octokit
