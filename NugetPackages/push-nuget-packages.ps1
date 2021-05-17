param (
	[Parameter(Mandatory=$false)]
	[string]$ApiKey
)

$solutionDir = Split-Path $dte.Solution.FileName -Parent
$packagesDir = "$solutionDir/NugetPackages"

# Get NuGet handle.
$nuget = "$solutionDir\.nuget\NuGet.exe"
	
$packages = Get-ChildItem -Path $packagesDir -Filter '*.nupkg' -Exclude '*.symbols.nupkg' -Recurse

foreach ($package in $packages)
{
	Write-Host "`r`nPushing '$package' package..." -ForegroundColor 'green' -BackgroundColor 'black'

	if ($ApiKey) 
	{
		&$nuget push $package -ApiKey $ApiKey -Source https://www.nuget.org/api/v2/package
	}
	else 
	{
		&$nuget push $package
	}
}