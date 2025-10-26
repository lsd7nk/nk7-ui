using UnityEditor;

namespace Nk7.UI.Editor
{
    [CustomEditor(typeof(LoopAnimatedComponent))]
    public sealed class LoopAnimatedComponentEditor : AnimatedInspectorBase
    {
        private SerializedProperty _loopBehaviour;

        protected override void OnEnable()
        {
            base.OnEnable();
            _loopBehaviour = serializedObject.FindProperty("_loopBehaviour");
        }

        protected override void DrawBody()
        {
            DrawBehaviour(_loopBehaviour, "Loop Behaviour");
        }
    }
}
