using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;
using Nk7.UI.Animations;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Nk7.UI
{
    [RequireComponent(typeof(Container), typeof(GraphicRaycaster))]
    public sealed partial class Button : AnimatedComponent, IButton
    {
        public event Action OnPointerClickEvent;
        public event Action OnPointerDownEvent;
        public event Action OnPointerUpEvent;

        [Space(10), Header(Utils.BUTTON)]
        [SerializeField] private InteractableBehaviour _pointerClickBehaviour;
        [SerializeField] private InteractableBehaviour _pointerDownBehaviour;
        [SerializeField] private InteractableBehaviour _pointerUpBehaviour;

        public void OnPointerClick(PointerEventData eventData)
        {
            _pointerClickBehaviour.ExecuteAsync(_animatedContainer).Forget();
            OnPointerClickEvent?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _pointerDownBehaviour.ExecuteAsync(_animatedContainer).Forget();
            OnPointerDownEvent?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _pointerUpBehaviour.ExecuteAsync(_animatedContainer).Forget();
            OnPointerUpEvent?.Invoke();
        }

        protected override void Reset()
        {
            base.Reset();

            _pointerClickBehaviour = new InteractableBehaviour();
            _pointerDownBehaviour = new InteractableBehaviour();
            _pointerUpBehaviour = new InteractableBehaviour();
        }
    }


#if UNITY_EDITOR
    public sealed partial class Button : IInitializable
    {
        public void Initialize()
        {
            if (_animatedContainer == null)
            {
                _animatedContainer = GetComponent<Container>();
            }
        }

        private static void CreateTmpText(GameObject parentObject)
        {
            var textObject = new GameObject(Utils.TMP_TEXT, typeof(RectTransform), typeof(TextMeshProUGUI));
            var textRectTransform = textObject.GetComponent<RectTransform>();
            var text = textObject.GetComponent<TextMeshProUGUI>();

            text.raycastTarget = false;

            GameObjectUtility.SetParentAndAlign(textObject, parentObject);

            textRectTransform.SetFullScreen(true);
        }

        private static void CreateImage(GameObject parentObject)
        {
            var imageObject = new GameObject(Utils.ICON, typeof(RectTransform), typeof(Image));
            var imageRectTransform = imageObject.GetComponent<RectTransform>();

            GameObjectUtility.SetParentAndAlign(imageObject, parentObject);

            imageRectTransform.SetFullScreen(true);
        }

        [MenuItem(Utils.BUTTON_PATH_TMP_TEXT, false, Utils.COMPONENT_PRIORITY)]
        private static void CreateComponentWithTmpText(MenuCommand menuCommand)
        {
            CreateButton(menuCommand, CreateTmpText, CreateImage);
        }

        [MenuItem(Utils.BUTTON_PATH_IMAGE, false, Utils.COMPONENT_PRIORITY)]
        private static void CreateComponentWithImage(MenuCommand menuCommand)
        {
            CreateButton(menuCommand, CreateImage);
        }

        private static void CreateButton(MenuCommand menuCommand, params Action<GameObject>[] createChildCallbacks)
        {
            var buttonObject = new GameObject(Utils.BUTTON, typeof(RectTransform), typeof(Button));
            var parentObject = Utils.GetCanvasAsParent(menuCommand.context as GameObject);

            GameObjectUtility.SetParentAndAlign(buttonObject, parentObject);
            Undo.RegisterCreatedObjectUndo(buttonObject, "Create " + buttonObject.name);

            var buttonRectTransform = buttonObject.GetComponent<RectTransform>();
            var container = buttonObject.GetComponent<Container>();
            var button = buttonObject.GetComponent<Button>();

            buttonRectTransform.SetToCenter(true);

            buttonRectTransform.sizeDelta = new Vector2(320f, 100f);

            for (int i = 0; i < createChildCallbacks.Length; ++i)
            {
                createChildCallbacks[i].Invoke(buttonObject);
            }

            container.Initialize();
            button.Initialize();
        }
    }
#endif
}