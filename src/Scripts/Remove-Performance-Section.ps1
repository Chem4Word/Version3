<#
.DESCRIPTION
Removes performance section from file.

  GlobalSection(Performance) = preSolution
      HasPerformanceSessions = true
  EndGlobalSection

.PARAMETER $slnFile
Local path to the .sln to modify
.EXAMPLE
Remove-Performance-Section.ps1 ..\Chem4Word.V3-1.sln
Remove-Performance-Section.ps1 ..\QuickTesting.sln
#>
param($slnFile)

$oldCode = [Regex]::Escape("`tGlobalSection(Performance) = preSolution`r`n`t`tHasPerformanceSessions = true`r`n`tEndGlobalSection`r`n")

$fileContent = Get-Content $slnFile -Raw
$newFileContent = $fileContent -replace $oldCode, ""
Set-Content -Encoding UTF8 -Path $slnFile -Value $newFileContent -NoNewline