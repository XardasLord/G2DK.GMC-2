# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),

## [1.1.1] (2021-04-XX)
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
