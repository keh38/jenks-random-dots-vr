; -- sync.iss --

; SEE THE DOCUMENTATION FOR DETAILS ON CREATING .ISS SCRIPT FILES!
#define verStr GetFileVersion("D:\Development\Jenks\VR\Random Dots VR\VRD Controller\VRD Controller\bin\Release\RandomDotsVR Controller.exe")
#define lastDot RPos(".", verStr)
;#define revStr Copy(verStr, lastDot+1)
#define revStr_ StringChange(revStr, '.', '_')

;#include "CommonInfo.iss"

[Setup]
AppName=Random Dots VR
AppVerName=Random Dots VR V{#revStr}
DefaultDirName={commonpf}\Jenks\Random Dots\V{#revStr}
OutputDir = Output
;OutputDir=D:\Transfer
DefaultGroupName=Jenks
AllowNoIcons=yes
OutputBaseFilename=Random_Dots_VR_{#revStr}
UsePreviousAppDir=no
UsePreviousGroup=no
UsePreviousSetupType=no
DisableProgramGroupPage=yes
DisableReadyMemo=yes
PrivilegesRequired=admin
Uninstallable=no

[Dirs]
Name: "{commonappdata}\Jenks";
Name: "{userdocs}\Jenks\Random Dots VR";

[Files]
Source: "D:\Development\Jenks\VR\Random Dots VR\VRD VR\Build\*.*"; DestDir: "{app}\VR"; Flags: replacesameversion recursesubdirs;
Source: "D:\Development\Jenks\VR\Random Dots VR\VRD Controller\Images\*.ico"; DestDir: "{app}\VR"; Flags: replacesameversion;
Source: "D:\Development\Jenks\VR\Random Dots VR\VRD Controller\VRD Controller\bin\Release\*.*"; DestDir: "{app}\Controller"; Flags: replacesameversion;
Source: "D:\Development\Jenks\VR\Random Dots VR\VRD Controller\Images\*.ico"; DestDir: "{app}\Controller"; Flags: replacesameversion;

[Icons]
Name: "{commondesktop}\Random Dots VR Controller"; Filename: "{app}\Controller\RandomDotsVR Controller.exe"; IconFilename: "{app}\Controller\VRD Controller.ico"; IconIndex: 0;
Name: "{commondesktop}\Random Dots VR"; Filename: "{app}\VR\RandomDotsVR.exe"; IconFilename: "{app}\VR\VRD Glasses.ico"; IconIndex: 0;

