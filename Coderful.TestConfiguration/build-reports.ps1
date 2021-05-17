# Configure the tools.
. .\configure-tools.ps1

$projects = ("Coderful.Events.Test")

# Get solution directory.
$SolutionDir = Split-Path $dte.Solution.FileName -Parent

foreach ($project in $projects)
{
	$testResultsDir = "$SolutionDir\Coderful.TestConfiguration\Reports\$project"
	$projectDir = "$SolutionDir\$project"

	# We need to have the directory first, otherwise the xUnit and SpecFlow tools will throw exception.
	New-Item -ItemType Directory -Force -Path $testResultsDir

	# Run xUnit tests.
	xunit.console "$projectDir\bin\debug\$project.dll" /xml "$testResultsDir\TestResult-xunit.xml"
	xunit.console "$projectDir\bin\debug\$project.dll" /nunit "$testResultsDir\TestResult-nunit.xml"

	# Generate SpecFlow report.
	specflow nunitexecutionreport "$SolutionDir\$project\$project.csproj" `
		/xmlTestResult:"$testResultsDir\TestResult-nunit.xml" `
		/out:"$testResultsDir\SpecFlowReport.html"

	# Generate Pickle report.
	Pickle-Features `
		-FeatureDirectory "$projectDir" `
		-OutputDirectory "$testResultsDir\Pickle-Report" `
		-TestResultsFile "$testResultsDir\TestResult-xunit.xml" `
		-TestResultsFormat xunit
}