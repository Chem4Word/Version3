del setup.log
del remove.log

msiexec /i Chem4Word-Setup.msi /l*v setup.log

pause

msiexec /uninstall Chem4Word-Setup.msi /l*v remove.log

pause

find "Property(" setup.log > properties.log
