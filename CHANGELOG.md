# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),

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

[1.0.1]: https://gitlab.com/dzieje-khorinis/gmc-2/-/releases/1.0.1
[1.0.0]: https://gitlab.com/dzieje-khorinis/gmc-2/-/releases/1.0.0
[0.0.5]: https://gitlab.com/dzieje-khorinis/gmc-2/-/releases/0.0.5
[0.0.4]: https://gitlab.com/dzieje-khorinis/gmc-2/-/releases/0.0.4
[0.0.3]: https://gitlab.com/dzieje-khorinis/gmc-2/-/releases/0.0.3
[0.0.2]: https://gitlab.com/dzieje-khorinis/gmc-2/-/releases/0.0.2
[0.0.1]: https://gitlab.com/dzieje-khorinis/gmc-2/-/releases/0.0.1
