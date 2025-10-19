using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;
using Nk7.UI.Animations;
using UnityEngine.UI;
using UnityEngine;
using System;
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

        private static void CreateImage(GameObject parentObject)
        {
            var imageObject = new GameObject(Utils.ICON, typeof(RectTransform), typeof(Image));
            var imageRectTransform = imageObject.GetComponent<RectTransform>();

            GameObjectUtility.SetParentAndAlign(imageObject, parentObject);

            imageRectTransform.SetFullScreen(true);
        }

        [MenuItem(Utils.BUTTON_PATH, false, Utils.COMPONENT_PRIORITY)]
        private static void CreateComponent(MenuCommand menuCommand)
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

            CreateImage(buttonObject);

            container.Initialize();
            button.Initialize();
        }
    }
#endif
}