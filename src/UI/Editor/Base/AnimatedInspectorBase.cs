using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

namespace Nk7.UI.Editor
{
    public abstract class AnimatedInspectorBase : UnityEditor.Editor
    {
        private const string SCRIPT_PROPETY_NAME = "m_Script";
        private const float AUTO_BUTTON_WIDTH = 60f;
        private const float AUTO_BUTTON_SPACING = 4f;

        private static readonly GUIContent AutoButtonContent = new GUIContent("Auto");
        private readonly HashSet<string> _handledProperties = new HashSet<string>();

        protected SerializedProperty AnimatedContainerProperty { get; private set; }

        protected virtual string AnimatedContainerPropertyName => "_animatedContainer";
        protected virtual string AnimatedContainerLabel => "Animated Container";

        protected virtual void OnEnable()
        {
            AnimatedContainerProperty = serializedObject.FindProperty(AnimatedContainerPropertyName);
        }

        public sealed override void OnInspectorGUI()
        {
            serializedObject.Update();

            _handledProperties.Clear();
            _handledProperties.Add(SCRIPT_PROPETY_NAME);

            if (AnimatedContainerProperty != null)
            {
                DrawAnimatedContainer(AnimatedContainerProperty);
            }

            DrawBody();
            DrawRemainingProperties();

            serializedObject.ApplyModifiedProperties();
        }

        protected abstract void DrawBody();

        protected void DrawAnimatedContainer(SerializedProperty property)
        {
            Register(property);

            DrawPropertyWithAuto(property, EditorGUIUtility.TrTempContent(AnimatedContainerLabel),
                GetContainerForComponent, "Assign Animated Container");

            if (property.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("Assign the Container that stores animated values for this component.", MessageType.Warning);
            }
        }

        protected void DrawBehaviour(SerializedProperty property, string label)
        {
            if (property == null)
            {
                return;
            }

            Register(property);

            var summary = AnimationInspectorUtility.InspectBehaviour(target, property.propertyPath);

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                var headerRect = EditorGUILayout.GetControlRect();
                property.isExpanded = EditorGUI.Foldout(headerRect, property.isExpanded, label, true, EditorStyles.foldoutHeader);

                bool hasSummary = !string.IsNullOrEmpty(summary.Summary);
                bool hasSecondary = !string.IsNullOrEmpty(summary.Secondary);
                bool hasWarning = !string.IsNullOrEmpty(summary.Warning);

                if (hasSummary || hasSecondary || hasWarning || property.isExpanded)
                {
                    using (new EditorGUI.IndentLevelScope())
                    {
                        if (hasSummary)
                        {
                            EditorGUILayout.LabelField(summary.Summary, EditorStyles.miniLabel);
                        }

                        if (hasSecondary)
                        {
                            EditorGUILayout.LabelField(summary.Secondary, EditorStyles.miniLabel);
                        }

                        if (hasWarning)
                        {
                            EditorGUILayout.HelpBox(summary.Warning, MessageType.Warning);
                        }

                        if (property.isExpanded)
                        {
                            using (new EditorGUI.IndentLevelScope())
                            {
                                DrawChildren(property);
                            }
                        }
                    }
                }
            }
        }

        protected void DrawRegisteredProperty(SerializedProperty property)
        {
            if (property == null)
            {
                return;
            }

            Register(property);
            EditorGUILayout.PropertyField(property, true);
        }

        protected void DrawObjectFieldWithAuto(SerializedProperty property,
            Func<Component, UnityEngine.Object> resolver, string missingMessage = null)
        {
            if (property == null)
            {
                return;
            }

            Register(property);
            DrawPropertyWithAuto(property, EditorGUIUtility.TrTempContent(property.displayName),
                resolver, $"Assign {property.displayName}");

            if (property.objectReferenceValue == null && !string.IsNullOrEmpty(missingMessage))
            {
                EditorGUILayout.HelpBox(missingMessage, MessageType.Info);
            }
        }

        protected bool CanResolveForAll(Func<Component, UnityEngine.Object> resolver)
        {
            if (resolver == null)
            {
                return false;
            }

            foreach (var obj in serializedObject.targetObjects)
            {
                if (obj is Component component)
                {
                    if (resolver(component) == null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        protected void Register(SerializedProperty property)
        {
            if (property != null)
            {
                _handledProperties.Add(property.propertyPath);
            }
        }

        private void DrawRemainingProperties()
        {
            if (_handledProperties.Count == 0)
            {
                DrawPropertiesExcluding(serializedObject, Array.Empty<string>());
                return;
            }

            var propertyNames = new string[_handledProperties.Count];
            _handledProperties.CopyTo(propertyNames);

            DrawPropertiesExcluding(serializedObject, propertyNames);
        }

        private void DrawChildren(SerializedProperty property)
        {
            var iterator = property.Copy();
            var endProperty = iterator.GetEndProperty();
            
            bool enterChildren = true;

            while (iterator.NextVisible(enterChildren) && !SerializedProperty.EqualContents(iterator, endProperty))
            {
                EditorGUILayout.PropertyField(iterator, new GUIContent(iterator.displayName), true);
                enterChildren = false;
            }
        }

        private void AssignReference(SerializedProperty property, Func<Component, UnityEngine.Object> resolver, string undoLabel)
        {
            if (property == null || resolver == null)
            {
                return;
            }

            var targets = serializedObject.targetObjects;
            Undo.RecordObjects(targets, undoLabel);

            foreach (var obj in targets)
            {
                if (obj is not Component component)
                {
                    continue;
                }

                var resolved = resolver(component);

                if (resolved == null)
                {
                    continue;
                }

                var individualObject = new SerializedObject(component);
                var prop = individualObject.FindProperty(property.propertyPath);

                if (prop != null)
                {
                    prop.objectReferenceValue = resolved;
                    individualObject.ApplyModifiedProperties();
                }
            }

            serializedObject.Update();
        }

        private static UnityEngine.Object GetContainerForComponent(Component component)
        {
            if (component == null)
            {
                return null;
            }

            if (component.TryGetComponent<Container>(out var containerOnSelf))
            {
                return containerOnSelf;
            }

            var containers = component.GetComponentsInChildren<Container>(true);

            if (containers == null || containers.Length == 0)
            {
                return null;
            }

            Container firstContainer = null;

            for (int i = 0; i < containers.Length; ++i)
            {
                var candidate = containers[i];

                if (candidate == null)
                {
                    continue;
                }

                firstContainer ??= candidate;

                if (string.Equals(candidate.name, Utils.CONTAINER, StringComparison.OrdinalIgnoreCase))
                {
                    return candidate;
                }
            }

            return firstContainer;
        }

        private void DrawPropertyWithAuto(SerializedProperty property, GUIContent label,
            Func<Component, UnityEngine.Object> resolver, string undoLabel)
        {
            var controlRect = EditorGUILayout.GetControlRect(true, EditorGUI.GetPropertyHeight(property, label, false));

            EditorGUI.BeginProperty(controlRect, label, property);

            var fieldRect = controlRect;
            fieldRect.width = Mathf.Max(0f, fieldRect.width - AUTO_BUTTON_WIDTH - AUTO_BUTTON_SPACING);

            EditorGUI.PropertyField(fieldRect, property, label, includeChildren: false);

            var buttonRect = new Rect(fieldRect.x + fieldRect.width + AUTO_BUTTON_SPACING, controlRect.y,
                AUTO_BUTTON_WIDTH, controlRect.height);

            bool canResolve = resolver != null && CanResolveForAll(resolver);

            using (new EditorGUI.DisabledScope(!canResolve))
            {
                if (GUI.Button(buttonRect, AutoButtonContent))
                {
                    AssignReference(property, resolver, undoLabel);
                }
            }

            EditorGUI.EndProperty();
        }
    }
}
