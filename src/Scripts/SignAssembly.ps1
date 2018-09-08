#
# Source: https://www.reddit.com/r/csharp/comments/9d8uxb/vs2017_msbuild_helpful_tricks/
#
# TIP: Use [IO.Path]::GetDirectoryName($YOUR_PATH_VARIABLE) to retrieve the directory of a path.
# TIP: Use [IO.Path]::GetFileName($YOUR_PATH_VARIABLE) to retrieve the filename of a path.
#
# -- Post-build Event :-
#
# powershell.exe -NoLogo -NonInteractive -ExecutionPolicy Unrestricted -Command ^
#   .'$(ProjectDir)Scripts\SignAssembly.ps1' ^
#   -TargetFileName $(TargetFileName) ^
#   -TargetPath $(TargetPath)

# .\SignAssemby.ps1 -TargetFileName "Chem4Word-Setup.exe" -TargetPath "C:\Dev\vso\chem4word\Version3\src\Installer\Chem4WordSetup\bin\Debug\Chem4Word-Setup.exe"

param
(
	[string]$TargetFileName,
	[string]$TargetPath
)

try
{
	$signClientPath = "C:\Tools\Azure\SignClient"
	$signClientExe = "$($signClientPath)\SignClient.exe"
	$signClientSettings = "$($signClientPath)\appsettings.json"

	if (Test-Path $signClientExe)
	{
		# Call .Net Foundation Code Signing Service
		Write-Output "Signing $($TargetFileName) ..."
		& $signClientExe sign -c $signClientSettings -r $env:SignClientUser -s $env:SignClientPassword -n Chem4Word -i $TargetPath
	}

	# Check that the file was signed
	Write-Output "Checking if $($TargetFileName) is signed ..."
	$sig = Get-AuthenticodeSignature -FilePath $TargetPath
	if ($sig.Status -eq "Valid")
	{
		Write-Output "File $($TargetFileName) is signed by $($sig.SignerCertificate.Subject)."
		exit 0
	}
	else
	{
		Write-Output "File $($TargetFileName) is not signed !"
		exit 1
	}
}
catch
{
	Write-Error $_.Exception.Message
	exit 2
}

# Should never get here !
exit 3
