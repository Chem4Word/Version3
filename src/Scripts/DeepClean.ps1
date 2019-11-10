cls

$targets = Get-ChildItem ..\. -include bin,obj -Recurse
foreach ($target in $targets)
{
    if (Test-Path $target.Fullname -PathType Container)
    {
        Write-Host "Purging $($target.Fullname)" -ForegroundColor Cyan
        Remove-Item $target.Fullname -Force -Recurse
    }
}