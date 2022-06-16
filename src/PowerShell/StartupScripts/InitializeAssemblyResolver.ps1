if($PSEdition -eq 'Desktop') {
    try {
        [AutoBrew.PowerShell.Utilities.AssemblyResolver]::Initialize()
    } catch {
        Write-Warning $_
    }
} else {
    try {
        Add-Type -Path ([System.IO.Path]::Combine($PSScriptRoot, "..", "AutoBrew.PowerShell.AssemblyLoadContext.dll")) | Out-Null
        [AutoBrew.PowerShell.AssemblyLoadContext.AssemblyLoadContextInitializer]::RegisterSharedAssemblyLoadContext()
    } catch {
        Write-Warning $_
    }
}