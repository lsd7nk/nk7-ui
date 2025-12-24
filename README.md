# nk7-ui

Lightweight animated UI framework for Unity built around PrimeTween tweens, UniTask-driven async flows, and inspectors that automate common animation setup.

## Features

- Strongly typed `AnimatedComponent` hierarchy with shared show/hide lifecycle and cancellation support.
- PrimeTween-powered move/rotate/scale/fade tweens with ease presets, custom curves, punch and loop behaviours, and optional From/To overrides.
- Reusable behaviours (`NotInteractableBehaviour`, `InteractableBehaviour`, `LoopBehaviour`) that coordinate container state and pointer feedback.
- `Container` caches initial `RectTransform` / `CanvasGroup` values and can restore them instantly to ensure deterministic animations.
- Prefab menu entries (*GameObject → UI → Nk7*) for `Container`, `View`, `Popup`, `Button`, and `Loop` speed up authoring.
- Custom inspectors highlight active animations, auto-assign overlays/containers, and hide irrelevant fields.

## Table of Contents
- [Installation](#installation)
  - [Unity Package Manager](#unity-package-manager)
  - [Manual Installation](#manual-installation)
- [Quick Start](#quick-start)
  - [1. Create a Container](#1-create-a-container)
  - [2. Add UI Components](#2-add-ui-components)
  - [3. Configure Behaviours](#3-configure-behaviours)
  - [4. Play It](#4-play-it)
- [Lifecycle](#lifecycle)
- [Inspector Tooling](#inspector-tooling)
- [Runtime API](#runtime-api)
- [Requirements](#requirements)

## Installation

### Unity Package Manager
1. Open Unity Package Manager (`Window → Package Manager`).
2. Click `+ → Add package from git URL…`.
3. Enter `https://github.com/s-elovikov/nk7-ui.git?path=src/UI`.

Unity does not auto-update Git-based packages; update the hash manually when needed or use [UPM Git Extension](https://github.com/mob-sakai/UpmGitExtension).

### Manual Installation
Copy the `src/UI` folder into your project and add `Nk7.UI.asmdef` to the assembly.

## Quick Start

### 1. Create a Container
- `GameObject → UI → Nk7 → Container` adds the component and a canvas if one is missing.
- `Container` initialises `RectTransform`, `CanvasGroup`, `Canvas`, stores initial values, and provides `Reset*` helpers for later use.

### 2. Add UI Components
- `View` – full-screen screen with show/hide animations.
- `Popup` – overlay-backed popup with optional destroy-on-hide.
- `Button` – animated button with click / press / release states.
- `Loop` – decorative element driven by `LoopAnimatedComponent`.

### 3. Configure Behaviours
- Expand **Show Behaviour** / **Hide Behaviour** sections.
- Enable Move/Rotate/Scale/Fade blocks as needed—disabled blocks hide their settings.
- Choose `Ease Type`: `Ease` shows preset options; `Animation Curve` exposes a custom curve field.
- Toggle `Use Custom From And To` to manually define tween bounds.

### 4. Play It
- Call `Show()` / `Hide()` during runtime or `Loop()` on a `LoopAnimatedComponent`.
- Subscribe to lifecycle events (e.g., `OnShowStartEvent`, `OnHideFinishEvent`) from the inspector or via code.

## Lifecycle
- `ShowAsync` and `HideAsync` drive PrimeTween tweens and await completion via UniTask.
- Events fire in order: `OnShowStartEvent` → animations → `OnShowFinishEvent` (the hide flow mirrors the sequence).
- `AnimatedComponent.Cancel()` interrupts active tweens, resets the container, and fires finish events with the `cancelled` flag.
- Derived components extend the lifecycle by overriding `OnShowAnimationAsync`, `OnHideAnimationAsync`, or synchronous counterparts.

## Inspector Tooling
- Inspectors present a summary of enabled behaviours and key tween settings at the top of the component.
- `Assign Container` and `Assign Overlay` buttons wire up common references automatically.
- Property drawers collapse unused fields to keep the UI compact and highlight only active animation data.
- Context-menu actions reset cached transforms and canvas groups back to their captured defaults.

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

- `Show(bool withoutAnimation = false)` / `Hide(bool withoutAnimation = false)` trigger the lifecycle immediately.
- `ShowAsync(CancellationToken)` / `HideAsync(CancellationToken)` return `UniTask` for awaiting completion.
- `LoopAnimatedComponent.Loop()` / `LoopAsync()` start looped tweens and expose a stop token for cancellation.
- `Container` helpers (`ResetPosition`, `ResetScale`, `ResetAlpha`, etc.) restore the cached baseline state on demand.

## Requirements

- Unity 2021.2+
- `com.cysharp.unitask` 2.3.0
- `com.kyrylokuzyk.primetween` 1.3.5
- `com.unity.visualscripting` 1.9.7
