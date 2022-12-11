# LoadsonPortProject
Port most of the mods here (even closed-source) to Loadson

- [x] [enemy-counter](https://github.com/karlsonmodding/enemy-counter)
- [x] [retry-key](https://github.com/karlsonmodding/retry-key)
- [x] [custom-crosshair](https://github.com/karlsonmodding/custom-crosshair)
- [x] [input-display](https://github.com/karlsonmodding/input-display)
- [x] [third-person-mod](https://github.com/karlsonmodding/third-person-mod)
- [ ] ~~[KarlsonMod](https://github.com/karlsonmodding/KarlsonMod)~~ (won't be done because it contains everything above)
- [ ] ~~[replay-mod](https://github.com/karlsonmodding/replay-mod)~~ (won't be done because KarlsonTAS)
- [x] [BoomerMod](https://github.com/karlsonmodding/BoomerMod)
- [ ] ~~[KarlsonTAS](https://github.com/karlsonmodding/KarlsonTAS)~~ (currently doesn't work. i won't fix it in the near future)
- [ ] ~~[MangLevelLoader](https://github.com/karlsonmodding/MangLevelLoader)~~ (replaced by KarlsonCustomLevels)

## How to compile a mod
1. Run ModBuilder.exe
2. Select your Karlson install
3. Exit ModBuilder.exe **DON'T ENTER MOD NAME**
4. Open `LoadsonMod.sln` in visual studio
5. If the mod doesn't build, download [0Harmony.dll](https://github.com/karlsonmodding/Loadson/raw/deployment/files/Internal/Loadson%20deps/0Harmony.dll) and place it in the `lib` folder
6. Build.
