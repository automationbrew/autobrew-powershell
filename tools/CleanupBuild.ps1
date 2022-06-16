[CmdletBinding()]
Param
(
    [Parameter()]
    [string]$BuildConfig ="Debug"
)

$output = Join-Path (Get-Item $PSScriptRoot).Parent.FullName "artifacts\$BuildConfig"
Write-Verbose "The output folder is set to $output"


Get-ChildItem -Path "$output\NetCorePreloadAssemblies" | ForEach-Object { 
    if(Test-Path "$output\$($_.NameString)") {
        Remove-Item "$output\$($_.NameString)" -Force
    }
}