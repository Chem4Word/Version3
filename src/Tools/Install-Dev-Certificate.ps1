$pwd = Split-Path -Path $MyInvocation.MyCommand.Path

# This does not work ???
# Have to CD first ???
# $certificate = """$($pwd)\Certificates\Developer\Chem4WordAddIn.pfx"""

CD "$($pwd)\Certificates\Developer"

$certificate = "Chem4WordAddIn.pfx"

Write-Host "Importing Certificate " $certificate

$password = ConvertTo-SecureString -String "Password_123" -Force –AsPlainText

Import-PfxCertificate -FilePath $certificate -CertStoreLocation Cert:\CurrentUser\My -Exportable -Password $password