using UnityEditor;
using UnityEngine;

namespace Nk7.UI.Editor
{
    [CustomEditor(typeof(AnimatedComponent), true)]
    public sealed class AnimatedComponentEditor : AnimatedInspectorBase
    {
        private SerializedProperty _showBehaviour;
        private SerializedProperty _hideBehaviour;

        private SerializedProperty _pointerClickBehaviour;
        private SerializedProperty _pointerDownBehaviour;
        private SerializedProperty _pointerUpBehaviour;

        private SerializedProperty _overlay;
        private SerializedProperty _destroyAfterHide;

        protected override void OnEnable()
        {
            base.OnEnable();

            _showBehaviour = serializedObject.FindProperty("_showBehaviour");
            _hideBehaviour = serializedObject.FindProperty("_hideBehaviour");

            _pointerClickBehaviour = serializedObject.FindProperty("_pointerClickBehaviour");
            _pointerDownBehaviour = serializedObject.FindProperty("_pointerDownBehaviour");
            _pointerUpBehaviour = serializedObject.FindProperty("_pointerUpBehaviour");

            _overlay = serializedObject.FindProperty("_overlay");
            _destroyAfterHide = serializedObject.FindProperty("_destroyAfterHide");
        }

        protected override void DrawBody()
        {
            DrawBehaviour(_showBehaviour, "Show Behaviour");
            EditorGUILayout.Space(4f);
            DrawBehaviour(_hideBehaviour, "Hide Behaviour");

            if (_pointerClickBehaviour != null)
            {
                EditorGUILayout.Space(6f);
                EditorGUILayout.LabelField("Pointer Animations", EditorStyles.boldLabel);
                DrawBehaviour(_pointerClickBehaviour, "On Pointer Click");
                DrawBehaviour(_pointerDownBehaviour, "On Pointer Down");
                DrawBehaviour(_pointerUpBehaviour, "On Pointer Up");
            }

            if (_overlay != null)
            {
                DrawObjectFieldWithAuto(_overlay, FindOverlayContainer,
                    "Assign an overlay container. Usually this is the first child in the hierarchy.");
                DrawRegisteredProperty(_destroyAfterHide);
            }
        }

        private UnityEngine.Object FindOverlayContainer(Component component)
        {
            if (component == null)
            {
                return null;
            }

            var mainContainer = AnimationInspectorUtility.GetInstanceField(component, "_animatedContainer") as Container;
            var containers = component.GetComponentsInChildren<Container>(true);

            foreach (var container in containers)
            {
                if (container == null)
                {
                    continue;
                }

                if (mainContainer != null && ReferenceEquals(container, mainContainer))
                {
                    continue;
                }

                return container;
            }

            return null;
        }
    }
}
