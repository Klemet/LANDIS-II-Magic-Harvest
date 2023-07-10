; LANDIS-II Extension infomation
#define CoreRelease "LANDIS-II-V7"
#define ExtensionName "Magic harvest"
#define AppVersion "1.2"
#define AppPublisher "Clément Hardy"

; Build directory
#define BuildDir "..\bin\Release"

; LANDIS-II installation directories
#define ExtDir "C:\Program Files\LANDIS-II-v7\extensions"
#define AppDir "C:\Program Files\LANDIS-II-v7"
#define LandisPlugInDir "C:\Program Files\LANDIS-II-v7\plug-ins-installer-files"
#define ExtensionsCmd AppDir + "\commands\landis-ii-extensions.cmd"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{95A4E245-308B-4315-A493-270E82A8E8A9}
AppName={#CoreRelease} {#ExtensionName}
AppVersion={#AppVersion}
; Name in "Programs and Features"
AppVerName={#CoreRelease} {#ExtensionName} v{#AppVersion}
AppPublisher={#AppPublisher}
DefaultDirName={pf}\{#ExtensionName}
DisableDirPage=yes
DefaultGroupName={#ExtensionName}
DisableProgramGroupPage=yes
LicenseFile=.\Installation Files\LANDIS-II_Binary_license.rtf
OutputDir={#SourcePath}
OutputBaseFilename={#CoreRelease} {#ExtensionName} {#AppVersion}-setup
Compression=lzma
SolidCompression=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"


[Files]
; This .dll IS the extension (ie, the extension's assembly)
; NB: Do not put an additional version number in the file name of this .dll
; (The name of this .dll is defined in the extension's \src\*.csproj file)
Source: {#BuildDir}\Landis.Extension.MagicHarvest.dll; DestDir: {#ExtDir}; Flags: ignoreversion

; Requisite auxiliary libraries
; NB. These libraries are used by other extensions and thus are never uninstalled.
Source: {#BuildDir}\Landis.Library.HarvestManagement-v4.dll; DestDir: {#ExtDir}; Flags: uninsneveruninstall ignoreversion
Source: {#BuildDir}\Landis.Utilities.dll; DestDir: {#ExtDir}; Flags: uninsneveruninstall ignoreversion

; Complete example for testing the extension
Source: "..\Examples\BaseHarvest\*"; DestDir: {#AppDir}\Examples\{#ExtensionName}; Flags: ignoreversion

; LANDIS-II identifies the extension with the info in this .txt file
; NB. New releases must modify the name of this file and the info in it
#define InfoTxt "Magic harvest v1.2.txt"
Source: .\Installation Files\plug-ins-installer-files\{#InfoTxt}; DestDir: {#LandisPlugInDir}
; NOTE: Don't use "Flags: ignoreversion" on any shared system files


[Run]
Filename: {#ExtensionsCmd}; Parameters: "remove ""Magic Harvest"" "; WorkingDir: {#LandisPlugInDir}
Filename: {#ExtensionsCmd}; Parameters: "add ""{#InfoTxt}"" "; WorkingDir: {#LandisPlugInDir} 


[UninstallRun]
; Remove "REHARVEST 0.1" from "extensions.xml" file.
Filename: {#ExtensionsCmd}; Parameters: "remove ""Magic Harvest"" "; WorkingDir: {#LandisPlugInDir}


