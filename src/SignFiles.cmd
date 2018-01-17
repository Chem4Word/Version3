@echo off

set release=Chem4Word-Setup.3.0.14.Release.1.msi
set working=C:\Temp

copy Tools\DigiCertUtil.exe %working%
copy Chem4Word.V3\Data\Chem4Word-Versions.xml %working%
copy Chem4Word.V3\Data\index.html %working%

copy Installer\Chem4WordSetup\bin\Setup\Chem4Word-Setup.exe %working%
copy Installer\WiXInstaller\bin\Setup\%release% %working%

pushd %working%
DigiCertUtil.exe sign /sha1 "6217CFB6A0926B17939F104EE2A282CDBE8A0BF7" /noInput Chem4Word-Setup.exe %release%

dir
popd

echo FTP these files to server
pause
