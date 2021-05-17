## What is CoreBits?
It's a collection of reusable components that are often required when building applications. Currently CoreBits solution consists of these components:

* **Coderful.Core** - basic utilities used within the CoreBits.
* **Coderful.Events** - simple implementation of an event mechanism that can be used for implementing domain events. This library's goal is to be lightweight and easy to use, while still being flexible enough for various scenarios.
* **Coderful.Web** - collection of components that are often needed when developing ASP.NET (both WebForms and MVC) web applications.
* **Coderful.Web.Mvc** - common constructs used in building web applications with ASP.NET MVC.
* **Coderful.Layouts** - collection of Razor view layouts.


## How to run the tests in this solution
1. Install SpecFlow extension.
2. Istall xunit test runner extension.
3. If you are using ReSharper, install xunitcontrib-resharper to allow running xUnit tests from ReSharper.
4. Install all packages in ".nuget/packages.config" (by rebuilding the solution).
5. Run "Coderful.TestConfiguration/configure-tools.ps1" in Package Manager Console.


## Generate test reports
Run "build-reports" inside Package Manager Console. Example:

	PM> cd .\Coderful.TestConfiguration
	PM> .\build-reports.ps1

The resulting reports will be located under Coderful.TestConfiguration/Reports.

## Build & publish NuGet packages
Run the needed script from the Package Manager Console. Resulting packages will end up in the NugetPackages folder.

	PM> cd .\NugetPackages
	PM> .\build-nuget-packages.ps1

To push the packages to the nuget server, do

	PM> cd .\NugetPackages
	PM> .\push-nuget-packages.ps1 -ApiKey 'your-api-key'

When running push-nuget-packages.ps1 you will need to provide your API Key. If you are pushing nuget.org (by default), the API Key can be found in your [account details page](https://www.nuget.org/account).

## Development guidelines
##### Folders starting with underscore
The folders starting with the underscore "_" are not namespace containers, which means that they should not define a new namespace. The reason to have them is to simply make it easier to navigate within the project, whilst still grouping related functionality in related namespaces.

## License
The MIT License (MIT)

Copyright (c) 2013-2014 coderful.com

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.