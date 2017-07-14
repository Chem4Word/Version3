Plugins go here :-

Must implement either of :-
    public interface IChem4WordEditor : IChem4WordCommon
    public interface IChem4WordRenderer : IChem4WordCommon
    public interface IChem4WordSearcher : IChem4WordCommon

    IChem4WordCommon uses
        IChem4WordTelemetry Telemetry { get; set; }

    Projects must reference
        Chem4Word.Contracts
        Chem4Word.Core
        Chem4Word.Model
        Chem4Word.View // If Structure Viewer is being used

See files in Contracts project for details

Any supporting assemblies MUST be referenced in the main AddIn project
 to ensure they are deployed to the bin folder, not the Plug-Ins folder

Full Post Build Command :-
--------------------------
if "$(ConfigurationName)" == "Vso-Ci" (
rem
) else (
rem echo "$(TargetDir)$(TargetFileName)" > folders.txt
rem echo "$(SolutionDir)$(SolutionName)\$(OutDir)PlugIns\" >> folders.txt
xcopy "$(TargetDir)$(TargetFileName)" "$(SolutionDir)$(SolutionName)\$(OutDir)PlugIns\" /C /f /r /y /i
)
--------------------------