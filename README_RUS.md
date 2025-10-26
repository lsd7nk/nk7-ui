# Nk7 UI

Лёгкий анимированный UI-фреймворк для Unity на базе PrimeTween.   
Пакет включает готовые компоненты (экраны, попапы, кнопки, зацикленные элементы), асинхронные сценарии показа/скрытия с UniTask и расширенный инспектор, упрощающий настройку анимаций.

---

## Содержание

- [Особенности](#особенности)
- [Требования](#требования)
- [Установка](#установка)
- [Быстрый старт](#быстрый-старт)
- [Runtime API](#runtime-api)

## Особенности

- **Анимируемые контейнеры** – `Container` хранит ссылки на `RectTransform` и `CanvasGroup`, кеширует стартовые позицию/поворот/масштаб/прозрачность и умеет мгновенно их восстанавливать.
- **Асинхронный показ/скрытие** – `AnimatedComponent` и его наследники (`View`, `Popup`, `Button`) предоставляют синхронные и асинхронные методы `Show/Hide` с событиями начала/завершения.
- **Анимации PrimeTween** – Move/Rotate/Scale/Fade твины с широкой настройкой (ease-типы, кастомные кривые, собственные From/To, punch и loop поведения).
- **Готовые поведения** – `NotInteractableBehaviour` (show/hide), `InteractableBehaviour` (реакции на pointer), `LoopBehaviour` (циклическая анимация) переиспользуют общие контейнеры.
- **Компонент Loop** – `LoopAnimatedComponent` проигрывает бесконечную анимацию для декоративных элементов.
- **Улучшенный инспектор** – кастомные инспекторы показывают суммарное состояние анимаций, автоматически назначают контейнеры/оверлеи, property drawer скрывает неактуальные поля.
- **Меню создания** – префабы Nk7 (`Container`, `View`, `Button`, `Popup`, `Loop`) доступны через *GameObject → UI → Nk7*.

## Требования

- **Unity**: 2021.2 и новее
- **Зависимости** (подтягиваются автоматически при установке пакета):
  - [`com.cysharp.unitask` `2.3.0`](https://github.com/Cysharp/UniTask) – асинхронные операции для `ShowAsync`/`HideAsync`.
  - [`com.kyrylokuzyk.primetween` `1.3.5`](https://assetstore.unity.com/packages/tools/animation/primetween-221986) – tween-движок.
  - `com.unity.visualscripting` `1.9.7`

## Установка

- Откройте *Window → Package Manager*.
- Нажмите кнопку *+* → *Add package from git URL…*.
- Вставьте ссылку с путём к пакету:

```
https://github.com/lsd7nk/nk7-ui.git?path=src/UI
```

## Быстрый старт

1. **Создайте контейнер**
   - *GameObject → UI → Nk7 → Container*. Если канваса нет – он создастся автоматически.
   - Компонент `Container` инициализирует `RectTransform`, `CanvasGroup`, `Canvas` и сохраняет исходные значения.

2. **Добавьте компоненты UI**
   - `View` – полноэкранный экран.
   - `Popup` – попап с оверлеем и опцией удаления после скрытия.
   - `Button` – кнопка с анимациями клика, нажатия, отпускания.
   - `Loop` – декоративный элемент с бесконечной анимацией.

3. **Настройте поведения**
   - Раскройте блоки **Show Behaviour** / **Hide Behaviour**.
   - Включите нужные Move/Rotate/Scale/Fade – отключённые блоки скрывают свои поля.
   - Выберите `Ease Type`: при `Ease` отображается список Ease, при `Animation Curve` – поле кривой.
   - `Use Custom From And To` включает ручные значения `From`/`To`.

4. **Проверьте в работе**
   - Вызовите `Show()` / `Hide()` в Play Mode или `Loop()` на `LoopAnimatedComponent`.
   - Подпишитесь на события (`OnShowStartEvent`, `OnHideFinishEvent` и т.д.) из инспектора или кода.

## Runtime API

```csharp
public sealed class MyPopup : Popup
{
    private async UniTaskVoid Start()
    {
        Show();            // мгновенный вызов (асинхронно внутри)
        await HideAsync(); // ожидаем завершения анимации
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

- `Show(bool withoutAnimation = false)` / `Hide(bool withoutAnimation = false)` – моментальные операции.
- `ShowAsync(CancellationToken)` / `HideAsync(CancellationToken)` – awaitable-методы.
- `LoopAnimatedComponent.Loop()` / `LoopAsync()` – запуск луп-анимации.
- `Container` предоставляет методы `ResetPosition`, `ResetScale`, `ResetAlpha` и др.
