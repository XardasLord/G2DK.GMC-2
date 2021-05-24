# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),

## [1.5.0] (2021-05-24)
### Added
- New settings in GUI:
  - Resolution
- Add new steps in Compose profile. Video BIK are disabled before Gothic2.exe process is run and enabled after process is finished.
- More than one GMC profile cannot be executed at the same time (this works only from GUI).
- GMC logo icon (first draft).
  
### Changed
- GUI main page reorganization.
- ZEN's are read from Gothic directory, not from repository directory.
- When file is renamed that this action is loged as a 'rename' instead of 'moved'.
- GUI is now centered at startup.

## [1.4.0] (2021-05-12)
### Added
- New settings in GUI:
  - Restore Default configuration
  - Open Logs directory
  - Clear Logs directory

## [1.3.1] (2021-05-06)
### Fixed
- Settings from GUI are now used only for RunMod profile.

## [1.3.0] (2021-05-06)
### Added
- In GMC UI we have couple of additional setting parameters:
  - Window mode
  - Dev mode
  - Disable music
  - Disable sound
  - Reparse scripts
- GMC UI is looking for ZEN files also in subdirectories.

### Fixed
- In GMC UI ZEN files are now correctly verified if they are binary or not (based on their content instead of just size).

## [1.2.0] (2021-04-30)
### Added
- Settings window in GMC UI. We can set Gothic 2 root path, modification path and select 3D ZEN World.
- `--configurationFile` parameter in GMC.

## [1.1.1] (2021-04-22)
### Added
- `Gothic2.exe` compilation process is no longer ran when changes during the Update profile only happened in `Worlds` asset directory.
- MessageBox with confirmation before executing Compose ot RestoreGothic profile.

### Changed
- Order profiles in GMC UI.
- Improved `GMC.ini` content generation. Overriden elements are in the `[OVERRIDES]` section now.

### Fixed
- Order of dialogues compilation in *Compose* and *Update* profiles. Now dialogues compilation executes **after** scripts compilation by `Gothic2.exe` process.

## [1.1.0] (2021-04-02)
### Added
- GUI for GMC

## [1.0.4] (2021-04-01)
### Added
- Usage of `REPARSEVIS` parameter to `Gothic2.exe` when something is changed under `Scripts\System\VisualFX` directory.
- Removing `GMC.ini` helper file after each profile.
- Execute Gothic compilation step is now run conditionally only.

## [1.0.3] (2021-03-31)
### Added
- GMC version is now visible in log files.

### Fixed
- UpdateDialogues condition is now properly set.
- GMC Compose compilation crash due to missing `MUSIC.DAT` file under `Scripts/_compiled` directory.

## [1.0.2] (2021-03-30)
### Added
- Update dialogues step is executed only if needed.

### Fixed
- Problem with null reference exception in update dialogues.

## [1.0.1] (2021-03-29)
### Added
- Added `EnableVDF` profile.
- Handled null reference exception in CslWriter.
- Additional logging in case of unexpected exception in updating dialogues command.

## [1.0.0] (2021-03-26)
### Added
- Removing uncompiled assets (from Textures and Meshes folders) on `RunMod.bat` profile.

## [0.0.5] (2021-03-12)
### Changed
- Optimized dialogues updating step. Now it's changed to execute in Parallel instead of synchronously which is ~4 times faster than before`.

## [0.0.4] (2021-03-03)
### Added
- Added new profile - BuildModFile + created `BuildModFile.bat` to execute GMC with this profile easily.
- Added new configuration section in `gmc-2.json` file called 'GothicVdfsConfig'

## [0.0.3] (2021-03-01)
### Removed
- 'GothicRoot' configuration path from the `gmc-2.json` file and moved it as an argument of `GMC-2.exe`.

## [0.0.2] (2021-02-25)
### Added
- 'VDFS:PHYSICALFIRST' argument to `Gothic2.exe` in RunMod profile.
- GMC version is now displayed in GMC console title bar.

### Changed
- All `.bat` files to run GMC on maximized console window instead of windowed ones.

## [0.0.1] (2021-02-25)
### Added
- First official release of new GMC-2 tool.

[1.5.0]: https://gitlab.com/dzieje-khorinis/gmc-2/-/releases/1.5.0
[1.4.0]: https://gitlab.com/dzieje-khorinis/gmc-2/-/releases/1.4.0
[1.3.1]: https://gitlab.com/dzieje-khorinis/gmc-2/-/releases/1.3.1
[1.3.0]: https://gitlab.com/dzieje-khorinis/gmc-2/-/releases/1.3.0
[1.2.0]: https://gitlab.com/dzieje-khorinis/gmc-2/-/releases/1.2.0
[1.1.1]: https://gitlab.com/dzieje-khorinis/gmc-2/-/releases/1.1.1
[1.1.0]: https://gitlab.com/dzieje-khorinis/gmc-2/-/releases/1.1.0
[1.0.4]: https://gitlab.com/dzieje-khorinis/gmc-2/-/releases/1.0.4
[1.0.3]: https://gitlab.com/dzieje-khorinis/gmc-2/-/releases/1.0.3
[1.0.2]: https://gitlab.com/dzieje-khorinis/gmc-2/-/releases/1.0.2
[1.0.1]: https://gitlab.com/dzieje-khorinis/gmc-2/-/releases/1.0.1
[1.0.0]: https://gitlab.com/dzieje-khorinis/gmc-2/-/releases/1.0.0
[0.0.5]: https://gitlab.com/dzieje-khorinis/gmc-2/-/releases/0.0.5
[0.0.4]: https://gitlab.com/dzieje-khorinis/gmc-2/-/releases/0.0.4
[0.0.3]: https://gitlab.com/dzieje-khorinis/gmc-2/-/releases/0.0.3
[0.0.2]: https://gitlab.com/dzieje-khorinis/gmc-2/-/releases/0.0.2
[0.0.1]: https://gitlab.com/dzieje-khorinis/gmc-2/-/releases/0.0.1
