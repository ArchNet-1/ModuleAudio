# Changelog

## [2.0.0] - 2026-11-25

Unity 6 release.

### Changed
- Requires Unity 6000.0 or newer.
- Code split into `Runtime/` (game code) and `Editor/` (Editor-only assembly, never in builds).
- Namespace `ArchNet.<Category>`; every file starts with a short description and a link to the documentation.
- Files moved with their `.meta`: existing scenes, prefabs and assets keep their references.
- Settings are saved between sessions (PlayerPrefs `ArchNet.Audio.*`).
- Listens to the sliders instead of polling every frame.
- Fixed: the sound slider showed the music volume; the toggles showed the music mute state.
- Prefabs moved out of `Editor/Resources` (they were missing from builds).
- Class `Audio` → `AudioModule`.
