[Setup]
AppName=Wildcat One
AppVersion={#MyAppVersion}
AppPublisher=Adrian Seth Tabotabo
AppPublisherURL=https://github.com/dreeyanzz/wildcat-one-windows
DefaultDirName={autopf}\Wildcat One
DefaultGroupName=Wildcat One
OutputDir=.
OutputBaseFilename=WildcatOneSetup
Compression=lzma
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=lowest
ArchitecturesInstallIn64BitMode=x64compatible

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\Wildcat One"; Filename: "{app}\wildcat-one-windows.exe"
Name: "{group}\{cm:UninstallProgram,Wildcat One}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\Wildcat One"; Filename: "{app}\wildcat-one-windows.exe"; Tasks: desktopicon

[Run]
Filename: "{app}\wildcat-one-windows.exe"; Description: "{cm:LaunchProgram,Wildcat One}"; Flags: nowait postinstall skipifsilent
