# nk7-ui

Лёгкий анимированный UI-фреймворк для Unity: PrimeTween управляет твинами, UniTask отвечает за асинхронные сценарии, инспекторы автоматизируют настройку анимаций.

## Особенности

- Иерархия `AnimatedComponent` с общим жизненным циклом, событиями начала/конца и поддержкой отмены.
- Анимации Move/Rotate/Scale/Fade на PrimeTween с ease-пресетами, собственными кривыми, punch/loop-режимами и кастомными From/To.
- Поведения `NotInteractableBehaviour`, `InteractableBehaviour`, `LoopBehaviour` переиспользуют контейнеры и управляют состоянием взаимодействия.
- `Container` кеширует стартовые значения `RectTransform` / `CanvasGroup` и умеет мгновенно их восстанавливать.
- Префабы *GameObject → UI → Nk7* (`Container`, `View`, `Popup`, `Button`, `Loop`) ускоряют создание UI.
- Кастомные инспекторы подсвечивают активные анимации, автоматически назначают контейнеры и скрывают неиспользуемые поля.

## Содержание
- [Установка](#установка)
  - [Unity Package Manager](#unity-package-manager)
  - [Ручная установка](#ручная-установка)
- [Быстрый старт](#быстрый-старт)
  - [1. Создайте контейнер](#1-создайте-контейнер)
  - [2. Добавьте компоненты](#2-добавьте-компоненты)
  - [3. Настройте поведения](#3-настройте-поведения)
  - [4. Проверьте в работе](#4-проверьте-в-работе)
- [Жизненный цикл](#жизненный-цикл)
- [Инструменты инспектора](#инструменты-инспектора)
- [Runtime API](#runtime-api)
- [Требования](#требования)

## Установка

### Unity Package Manager
1. Откройте Unity Package Manager (`Window → Package Manager`).
2. Нажмите `+ → Add package from git URL…`.
3. Вставьте `https://github.com/lsd7nk/nk7-ui.git?path=src/UI`.

Unity не обновляет git-пакеты автоматически – при необходимости меняйте хеш вручную или используйте [UPM Git Extension](https://github.com/mob-sakai/UpmGitExtension).

### Ручная установка
Скопируйте папку `src/UI` в проект и добавьте `Nk7.UI.asmdef` к сборке.

## Быстрый старт

### 1. Создайте контейнер
- *GameObject → UI → Nk7 → Container* создаёт компонент и добавляет канвас, если его ещё нет.
- `Container` инициализирует `RectTransform`, `CanvasGroup`, `Canvas`, сохраняет исходные значения и предоставляет методы `Reset*`.

### 2. Добавьте компоненты
- `View` – полноэкранный экран с анимациями показа/скрытия.
- `Popup` – попап с оверлеем и опцией удаления после скрытия.
- `Button` – кнопка с состояниями клика, нажатия и отпускания.
- `Loop` – декоративный элемент на базе `LoopAnimatedComponent`.

### 3. Настройте поведения
- Раскройте блоки **Show Behaviour** / **Hide Behaviour**.
- Включите нужные Move/Rotate/Scale/Fade – отключённые блоки скрывают свои поля.
- Выберите `Ease Type`: `Ease` показывает список пресетов, `Animation Curve` — поле кривой.
- `Use Custom From And To` активирует ручные границы `From` / `To`.

### 4. Проверьте в работе
- Вызовите `Show()` / `Hide()` в рантайме или `Loop()` на `LoopAnimatedComponent`.
- Подпишитесь на события (`OnShowStartEvent`, `OnHideFinishEvent` и др.) в инспекторе или коде.

## Жизненный цикл
- `ShowAsync` и `HideAsync` запускают твины PrimeTween и ожидают завершения через UniTask.
- События идут по порядку: `OnShowStartEvent` → анимации → `OnShowFinishEvent`; для скрытия последовательность зеркальна.
- `AnimatedComponent.Cancel()` прерывает активные твины, возвращает контейнер в исходное состояние и завершает события с флагом отмены.
- Наследники переопределяют `OnShowAnimationAsync`, `OnHideAnimationAsync` или синхронные версии, расширяя поведение.

## Инструменты инспектора
- Шапка инспектора показывает резюме активных анимаций и ключевые настройки.
- Кнопки `Assign Container` и `Assign Overlay` автоматически подставляют ссылки.
- Property drawers скрывают неактивные поля и оставляют только задействованные параметры.
- Действия из контекстного меню возвращают кешированные трансформы и CanvasGroup к исходным значениям.

## Runtime API

```csharp
public sealed class MyPopup : Popup
{
    private async UniTaskVoid Start()
    {
        Show();            // мгновенный запуск (внутри асинхронно)
        await HideAsync(); // ждём окончания анимации
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

- `Show(bool withoutAnimation = false)` / `Hide(bool withoutAnimation = false)` — моментальные вызовы.
- `ShowAsync(CancellationToken)` / `HideAsync(CancellationToken)` — awaitable-методы.
- `LoopAnimatedComponent.Loop()` / `LoopAsync()` — запуск и ожидание луп-анимации, есть токен отмены.
- Методы `Container` (`ResetPosition`, `ResetScale`, `ResetAlpha` и др.) восстанавливают кешированное базовое состояние.

## Требования

- Unity 2021.2+
- `com.cysharp.unitask` 2.3.0
- `com.kyrylokuzyk.primetween` 1.3.5
- `com.unity.visualscripting` 1.9.7
