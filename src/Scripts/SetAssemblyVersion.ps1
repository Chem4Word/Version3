# SetAssemblyVersion.ps1
#
# http://www.luisrocha.net/2009/11/setting-assembly-version-with-windows.html
# http://blogs.msdn.com/b/dotnetinterop/archive/2008/04/21/powershell-script-to-batch-update-assemblyinfo-cs-with-new-version.aspx
# http://jake.murzy.com/post/3099699807/how-to-update-assembly-version-numbers-with-teamcity
# https://github.com/ferventcoder/this.Log/blob/master/build.ps1#L6-L19
# https://blogs.msdn.microsoft.com/sonam_rastogi_blogs/2014/05/14/update-xml-file-using-powershell/

# Set-ExecutionPolicy -Scope CurrentUser -ExecutionPolicy Unrestricted

Param(
	[string]$version,
	[string]$path,
	[string]$released,
	[string]$name
)

function Help {
	"Sets the AssemblyVersion and AssemblyFileVersion of AssemblyInfo.cs files`n"
	".\SetAssemblyVersion.ps1 [VersionNumber] -path [SearchPath] -released [Released] -name [Name]`n"
	"   [VersionNumber]     The version number to set, for example: 1.2.3"
	"   [Released]          The release date, for example: 01-Jan-2019"
	"   [Name]              The name, for example: Beta 2"
	"   [SearchPath]        The path to search for AssemblyInfo files.`n"
}

function Update-SourceVersion
{
	Param ([string]$Version)
	#$NewVersion = 'AssemblyVersion("' + $Version + '")';
	#$NewFileVersion = 'AssemblyFileVersion("' + $Version + '")';

	foreach ($o in $input) 
	{
		Write-Host "Updating  '$($o.FullName)' -> $Version"

		$assemblyVersionPattern = 'AssemblyVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
		$fileVersionPattern = 'AssemblyFileVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
		$companyPattern = 'AssemblyCompany\(".*"\)'
		$copyrightPattern = 'AssemblyCopyright\(".*"\)'
		$trademarkPattern = 'AssemblyTrademark\(".*"\)'

		$assemblyVersion = 'AssemblyVersion("' + $version + '")';
		$fileVersion = 'AssemblyFileVersion("' + $version + '")';
		$company = 'AssemblyCompany("Chem4Word")'
		$copyright = 'AssemblyCopyright("© Chem4Word 2019. All rights reserved.")'
		$trademark = 'AssemblyTrademark("Chem4Word")'

		(Get-Content $o.FullName -encoding UTF8) | ForEach-Object  { 
		   % {$_ -replace $assemblyVersionPattern, $assemblyVersion } |
		   % {$_ -replace $fileVersionPattern, $fileVersion } |
		   % {$_ -replace $companyPattern, $company } |
		   % {$_ -replace $copyrightPattern, $copyright } |
		   % {$_ -replace $trademarkPattern, $trademark }
		} | Out-File $o.FullName -encoding UTF8 -force
	}
}

function Update-AllAssemblyInfoFiles ($version)
{
   #Write-Host "Searching '$path'"
   foreach ($file in "AssemblyInfo.cs", "AssemblyInfo.vb" ) 
   {
		get-childitem $path -recurse |? {$_.Name -eq $file} | Update-SourceVersion $version ;
   }
}

# ---------------------------------------------------------- #

# Validate arguments 
if ($args -ne $null) {
	if ($version -eq '/?')
	{
		Help
		exit 1;
	}
}

if ($version)
{
	$parts = $version.Split('.')

	if ($parts.Length -ne 3)
	{
		Write-Host "Version is not Major.Minor.Revision" -ForegroundColor Red
		Help
		exit 1;
	}

	$reject = $false
	foreach ($part in $parts)
	{
		if ($part -notmatch "^[0-9]+$")
		{
			$reject = $true
			Write-Host "'$($part)' is not an integer" -ForegroundColor Red
		}
	}
	if ($reject)
	{
		Help
		exit 1;
	}
}

if (!$path -and !$version -and !$name -and !$released)
{
	Help
	exit 1;
}

# Calculate number of days since 01-Jan-2000
[TimeSpan]$delta = [DateTime]$released - [DateTime]"01-Jan-2000"

if ($delta -eq $null)
{
	Help
	exit 1;
}

# Arguments OK - Continue

$pwd = Split-Path -Path $MyInvocation.MyCommand.Path
CD "$($pwd)"

# ---------------------------------------------------------- #

# Update Assembly Info files
Write-Host "Searching for files in $($pwd)\$($path)" -ForegroundColor Green
Update-AllAssemblyInfoFiles "$($version).$($delta.Days)"

$number = "$($version) $($name)"

# ---------------------------------------------------------- #

# Update This-Version.xml
Write-Host " Updating 'This-Version.xml'" -ForegroundColor Yellow
$thisVersionFile = "$($pwd)\..\Chem4Word.V3\Data\This-Version.xml"
Write-Host "$($thisVersionFile)" -ForegroundColor Green

$xml = [xml](Get-Content $thisVersionFile)
$xml.Version.Number = $number
$xml.Version.Released = $released

$xml.Save($thisVersionFile)

# ---------------------------------------------------------- #

# Update Product.wxs
Write-Host " Updating 'Product.wxs'" -ForegroundColor Yellow
$wixFile = "$($pwd)\..\Installer\WiXInstaller\Product.wxs"
Write-Host "$($wixFile)" -ForegroundColor Green

$xml = [xml](Get-Content $wixFile)
$xml.Wix.Product.Version = "$($version).$($delta.Days)"
$xml.Wix.Product.Name = "Chemistry Add-In for Microsoft Word $($version) $($name)"
$xml.Save($wixFile)

# ---------------------------------------------------------- #

# Update Wix-Installer.wixproj
Write-Host " Updating 'Wix-Installer.wixproj'" -ForegroundColor Yellow
$wixProj = "$($pwd)\..\Installer\WiXInstaller\Wix-Installer.wixproj"
Write-Host "$($wixProj)" -ForegroundColor Green

$xml = [xml](Get-Content $wixProj)
$dottedname = $name.Replace(" ", ".")
$xml.Project.PropertyGroup[0].OutputName = "Chem4Word-Setup.$($version).$($dottedname)"
$xml.Save($wixProj)

# ---------------------------------------------------------- #

Write-Host " Updating 'SignFiles.cmd'" -ForegroundColor Yellow

$file = "$($pwd)\..\Scripts\SignFiles.cmd"
Write-Host "$($file)" -ForegroundColor Green

$findPattern = 'set release=Chem4Word-Setup.*'
$replaceWith = "set release=Chem4Word-Setup.$($version).$($dottedname).msi"
#(Get-Content $file).Replace($findPattern, $replaceWith) | Set-Content $file

(Get-Content $file) | ForEach-Object { $_ -replace $findPattern, $replaceWith } | Set-Content $file

#CD "$($pwd)"