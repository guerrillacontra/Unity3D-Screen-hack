# Unity3D-Screen-hack

A small script that can be used to allow multiple fullscreen instances of StandAlonePlayer's to be designated to multiple adapters.

It will allow you to do things such as launch multiple instances via a cmd/bat script into different screens.

```
start App.exe -adapter 0 -screen-fullscreen 1 -screen-width 2560 -screen-height 1440 -window-mode borderless
start App.exe -adapter 1 -screen-fullscreen 1 -screen-width 1920 -screen-height 1080 -window-mode borderless
```

Without Unity constantly screwing you via its persisted "memory" of what it thinks your player should look like.


To Use:

Add the UnityStandAlonePlayerDisplayHack script to a game object (probably best to put it in the entry point of your unity app)
and run the instance with command lines like in the example above.

If the instance is not full screen, it will not do anything.

So this "hack" only works for full screen instances.
