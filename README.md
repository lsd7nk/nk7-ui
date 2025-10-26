# Nk7 UI

Lightweight animated UI framework for Unity built on top of PrimeTween.  
The package ships with ready-to-use components (screens, popups, buttons, looping elements), asynchronous show/hide flows backed by UniTask, and enhanced inspector tooling that speeds up animation setup.

---

## Table of Contents

- [Features](#features)
- [Requirements](#requirements)
- [Installation](#installation)
- [Quick Start](#quick-start)
- [Runtime API](#runtime-api)

## Features

- **Animated containers** – `Container` stores references to `RectTransform` and `CanvasGroup`, caches the starting position/rotation/scale/alpha and can instantly restore them.
- **Async show / hide** – `AnimatedComponent` and its derivatives (`View`, `Popup`, `Button`) expose synchronous and asynchronous `Show/Hide` methods with start/finish events.
- **PrimeTween animations** – Move/Rotate/Scale/Fade tweens with extensive settings (ease types, custom curves, custom From/To, punch and loop behaviours).
- **Prebuilt behaviours** – `NotInteractableBehaviour` (show/hide), `InteractableBehaviour` (pointer reactions) and `LoopBehaviour` (continuous animation) reuse shared containers.
- **Loop component** – `LoopAnimatedComponent` drives endless animation for decorative UI elements.
- **Improved inspector** – custom inspectors show a summary of active animations, auto-assign containers/overlays, and a property drawer hides irrelevant fields.
- **Creation menu** – Nk7 prefabs (`Container`, `View`, `Button`, `Popup`, `Loop`) are available under *GameObject → UI → Nk7*.

## Requirements

- **Unity**: 2021.2 or newer
- **Dependencies** (pulled automatically when the package is installed):
  - [`com.cysharp.unitask` `2.3.0`](https://github.com/Cysharp/UniTask) – async operations for `ShowAsync` / `HideAsync`.
  - [`com.kyrylokuzyk.primetween` `1.3.5`](https://assetstore.unity.com/packages/tools/animation/primetween-221986) – tween engine.
  - `com.unity.visualscripting` `1.9.7`

## Installation

- Open *Window → Package Manager*.
- Click the *+* button → *Add package from git URL…*.
- Paste the repo URL with the package path:

```
https://github.com/lsd7nk/nk7-ui.git?path=src/UI
```

## Quick Start

1. **Create a container**
   - *GameObject → UI → Nk7 → Container*. If a canvas is missing it will be created automatically.
   - `Container` initialises `RectTransform`, `CanvasGroup`, `Canvas` and stores initial values.

2. **Add UI components**
   - `View` – full-screen view.
   - `Popup` – popup with overlay and optional destroy-on-hide.
   - `Button` – button with click / press / release animations.
   - `Loop` – decorative looping element.

3. **Configure behaviours**
   - Expand **Show Behaviour** / **Hide Behaviour** sections.
   - Enable the necessary Move/Rotate/Scale/Fade entries – disabled blocks hide their fields.
   - Select `Ease Type`: `Ease` shows the ease dropdown, `Animation Curve` exposes a curve field.
   - `Use Custom From And To` reveals manual `From` / `To` values.

4. **Test it**
   - Call `Show()` / `Hide()` in Play Mode or `Loop()` on a `LoopAnimatedComponent`.
   - Subscribe to events (`OnShowStartEvent`, `OnHideFinishEvent`, etc.) from the inspector or code.

## Runtime API

```csharp
public sealed class MyPopup : Popup
{
    private async UniTaskVoid Start()
    {
        Show();            // fire-and-forget (runs async internally)
        await HideAsync(); // wait for animation to finish
    }
}

public sealed class MyButton : Button
{
    void Awake()
    {
        OnPointerClickEvent += () => Debug.Log("Click!");
    }
}
```

- `Show(bool withoutAnimation = false)` / `Hide(bool withoutAnimation = false)` – immediate calls.
- `ShowAsync(CancellationToken)` / `HideAsync(CancellationToken)` – awaitable methods.
- `LoopAnimatedComponent.Loop()` / `LoopAsync()` – start loop animation.
- `Container` exposes helpers such as `ResetPosition`, `ResetScale`, `ResetAlpha`, etc.
