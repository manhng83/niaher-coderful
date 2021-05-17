param (
	[Parameter(Mandatory=$false)]
	[string]$Project
)

function Build-Packages
{
	param (
		[Parameter(Mandatory=$false)]
		[string]$Project
	)

	$projects = (
		"Coderful.Events", 
		"Coderful.Web", 
		"Coderful.Web.Optimization", 
		"Coderful.Web.Mvc", 
		"Coderful.Core", 
		"Coderful.Logging", 
		"Coderful.Permissions",
		"Coderful.EntityFramework.Testing")

	# Remove previously built packages.
	Remove-Item *.nupkg

	# Get solution directory.
	$solutionDir = Split-Path $dte.Solution.FileName -Parent
	$currentDir = "$solutionDir\NugetPackages"

	# Get NuGet handle.
	$nuget = "$solutionDir\.nuget\NuGet.exe"

	foreach ($project in $projects | where {$_ -like "*$Project*"})
	{
		Write-Host "`r`nBuilding '$project' package..." -ForegroundColor 'green' -BackgroundColor 'black'

		$projectDir = "$solutionDir\$project"

		# Make sure .nuspec file exists.
		cd $projectDir
		&$nuget spec -Verbosity quiet
		cd $currentDir

		# Build package.
		&$nuget pack "$projectDir\$project.csproj" `
			-OutputDirectory "$currentDir" `
			-Build `
			-Symbols `
			-Properties "Configuration=Release;Platform=AnyCPU"
	}
}

Build-Packages -Project $Project