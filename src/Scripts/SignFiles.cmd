@echo off

set release=Chem4Word-Setup.3.0.23.Release.10.msi
set working=C:\Temp
set signclientpath=C:\Tools\Azure\SignClient

echo Copying files to %working%
copy ..\Chem4Word.V3\Data\Chem4Word-Versions.xml %working% > nul
copy ..\Chem4Word.V3\Data\index.html %working% > nul

copy ..\Installer\Chem4WordSetup\bin\Setup\Chem4Word-Setup.exe %working% > nul
copy ..\Installer\WiXInstaller\bin\Setup\%release% %working% > nul

pushd %working%
dir

echo Signing Chem4Word-Setup.exe
%signclientpath%\SignClient.exe sign -c %signclientpath%\appsettings.json -r %SignClientUser% -s %SignClientPassword% -n Chem4Word -i Chem4Word-Setup.exe
echo Signing %release%
%signclientpath%\SignClient.exe sign -c %signclientpath%\appsettings.json -r %SignClientUser% -s %SignClientPassword% -n Chem4Word -i %release%

dir
popd

echo FTP these files to server
pause
