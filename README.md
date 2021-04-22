# Gothic Mod Composer

Gothic Mod Composer (GMC-2) is a tool that helps to prepare Gothic II Dzieje Khorinis files to be properly automaticly arranged, copied, updated and compiled.
The project board is available [here in Trello]

## Prerequisite
- You need to have installed [.NET Desktop Runtime 5.0 x86](https://dotnet.microsoft.com/download/dotnet/thank-you/runtime-desktop-5.0.3-windows-x86-installer) on your computer.

## How to run?

### GMC UI
GUI is avaialable since GMC 1.1.0 version. GUI is constantly improving with new features and UX changes.

### Running via .bat files
Previously GMC could be run via `.bat` files, each one representing different profile to execute.
After building solution the GMC.exe executable file will be available. To run GMC.exe you have to provide several parameters:
- `profile` - a profile to execute. Available profiles are described in next section of this README file.
- `modPath` - an absolute path to the Gothic II Dzieje Khorinis repository.
- `gothic2Path` - an absolute path to the Gothic II root directory.

Example command to run GMC-2.exe:
```GMC.exe --modPath="F:\Gry\Gothic II Dzieje Khorinis\TheHistoryOfKhorinis" --gothic2Path="F:\Gry\Gothic II Dzieje Khorinis" --profile=Compose```

## GMC-2 profiles
GMC-2 has several profile modes that can be run with. Below you can find the description of each profile.

### Compose
> This is a standard profile to rebuild all mod. It cleans the _Work/Data folder, copies all assets and runs the textures, scripts, animations and other assets compilation. Mod Dzieje Khorinis will be fully ready.

### RestoreGothic
> The Compose profile at the first step creates the Gothic II _Work/Data original files backup into .gmc/backup path. This profile restores all the original Gothic II data and deletes the helper .gmc directory (also with built .MOD file).

### RunMod
> Runs the game with all built files. Files won't be compiled.

### Update
> This profile updates the previously copies assets after running Compose. In case of new asset files appears in the Dzieje Khorinis repository this profile will only copy this new files to the Gothic II files. If some of the files from the repository will be modified then this profile will also updated those files in the Gothic II assets and also will delete their compiled files. If some of the files will be deleted in the Dzieje Khorinis repository then this profile will indicate that and will delete them from Gothic II assets along with their compiled files.

### BuildModFile
> Builds the .mod file package from compiled files and save the package in the _gothicRoot/.gmc/Build path.

### EnableVDF
> Enabled VDF files from the assets directory.


[here in Trello]: https://trello.com/b/ndyTLtzA/gmc-2