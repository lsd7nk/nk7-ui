using System.Collections.Generic;
using Nk7.UI.Animations;
using UnityEditor;
using UnityEngine;

namespace Nk7.UI.Editor
{
    [CustomPropertyDrawer(typeof(Animations.Animation), true)]
    public sealed class AnimationDrawer : PropertyDrawer
    {
        private static readonly float LineHeight = EditorGUIUtility.singleLineHeight;
        private static readonly float VerticalSpacing = EditorGUIUtility.standardVerticalSpacing;

        private static readonly GUIContent AnimationTypeLabel = new GUIContent("Animation Type");

        private static readonly string AnimationTypeField = "<AnimationType>k__BackingField";
        private static readonly string IsEnabledField = "<IsEnabled>k__BackingField";
        private static readonly string StartDelayField = "<StartDelay>k__BackingField";
        private static readonly string DurationField = "<Duration>k__BackingField";
        private static readonly string EaseTypeField = "<EaseType>k__BackingField";
        private static readonly string EaseField = "<Ease>k__BackingField";
        private static readonly string AnimationCurveField = "<AnimationCurve>k__BackingField";
        private static readonly string UseCustomFromAndToField = "<UseCustomFromAndTo>k__BackingField";
        private static readonly string FromField = "<From>k__BackingField";
        private static readonly string ToField = "<To>k__BackingField";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var foldoutRect = new Rect(position.x, position.y, position.width, LineHeight);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);

            if (!property.isExpanded)
            {
                EditorGUI.EndProperty();
                return;
            }

            float currentY = foldoutRect.yMax + VerticalSpacing;

            EditorGUI.indentLevel++;

            var excludedProperties = new HashSet<string>(System.StringComparer.Ordinal);
            var animationTypeProp = property.FindPropertyRelative(AnimationTypeField);
            
            if (animationTypeProp != null)
            {
                var rect = CreateRect(position, ref currentY, animationTypeProp);

                using (new EditorGUI.DisabledScope(true))
                {
                    EditorGUI.PropertyField(rect, animationTypeProp, AnimationTypeLabel);
                }

                excludedProperties.Add(animationTypeProp.propertyPath);
            }

            var isEnabledProp = property.FindPropertyRelative(IsEnabledField);

            if (isEnabledProp == null)
            {
                EditorGUI.LabelField(CreateSingleLineRect(position, ref currentY), "Missing IsEnabled field");
                EditorGUI.indentLevel--;
                EditorGUI.EndProperty();

                return;
            }

            EditorGUI.PropertyField(CreateRect(position, ref currentY, isEnabledProp), isEnabledProp);
            excludedProperties.Add(isEnabledProp.propertyPath);

            if (!isEnabledProp.boolValue)
            {
                EditorGUI.indentLevel--;
                EditorGUI.EndProperty();

                return;
            }

            DrawOptionalField(position, property, StartDelayField, ref currentY, excludedProperties);
            DrawOptionalField(position, property, DurationField, ref currentY, excludedProperties);

            DrawEaseFields(position, property, ref currentY, excludedProperties);
            DrawFromToFields(position, property, ref currentY, excludedProperties);

            DrawRemainingProperties(position, property, ref currentY, excludedProperties);

            EditorGUI.indentLevel--;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = LineHeight;

            if (!property.isExpanded)
            {
                return height;
            }

            height += VerticalSpacing;

            var excludedProperties = new HashSet<string>(System.StringComparer.Ordinal);
            var animationTypeProp = property.FindPropertyRelative(AnimationTypeField);

            if (animationTypeProp != null)
            {
                height += EditorGUI.GetPropertyHeight(animationTypeProp, true) + VerticalSpacing;
                excludedProperties.Add(animationTypeProp.propertyPath);
            }

            var isEnabledProp = property.FindPropertyRelative(IsEnabledField);

            if (isEnabledProp == null)
            {
                return height + LineHeight;
            }

            height += EditorGUI.GetPropertyHeight(isEnabledProp, true);
            excludedProperties.Add(isEnabledProp.propertyPath);

            if (!isEnabledProp.boolValue)
            {
                return height;
            }

            height += GetOptionalHeight(property, StartDelayField, excludedProperties);
            height += GetOptionalHeight(property, DurationField, excludedProperties);

            height += GetEaseFieldsHeight(property, excludedProperties);
            height += GetFromToHeight(property, excludedProperties);

            height += GetRemainingPropertiesHeight(property, excludedProperties);

            return height;
        }

        private static void DrawEaseFields(Rect position, SerializedProperty property, ref float currentY,
            HashSet<string> excludedProperties)
        {
            var easeTypeProp = property.FindPropertyRelative(EaseTypeField);

            if (easeTypeProp == null)
            {
                return;
            }

            EditorGUI.PropertyField(CreateRect(position, ref currentY, easeTypeProp), easeTypeProp);
            excludedProperties.Add(easeTypeProp.propertyPath);

            var easeType = (EaseType)easeTypeProp.enumValueIndex;

            if (easeType == EaseType.Ease)
            {
                DrawOptionalField(position, property, EaseField, ref currentY, excludedProperties);
                ExcludeProperty(property, AnimationCurveField, excludedProperties);
            }
            else if (easeType == EaseType.AnimationCurve)
            {
                DrawOptionalField(position, property, AnimationCurveField, ref currentY, excludedProperties);
                ExcludeProperty(property, EaseField, excludedProperties);
            }
            else
            {
                ExcludeProperty(property, EaseField, excludedProperties);
                ExcludeProperty(property, AnimationCurveField, excludedProperties);
            }
        }

        private static float GetEaseFieldsHeight(SerializedProperty property, HashSet<string> excludedProperties)
        {
            var easeTypeProp = property.FindPropertyRelative(EaseTypeField);

            if (easeTypeProp == null)
            {
                return 0f;
            }

            float height = EditorGUI.GetPropertyHeight(easeTypeProp, true) + VerticalSpacing;
            excludedProperties.Add(easeTypeProp.propertyPath);

            var easeType = (EaseType)easeTypeProp.enumValueIndex;

            if (easeType == EaseType.Ease)
            {
                height += GetOptionalHeight(property, EaseField, excludedProperties);

                ExcludeProperty(property, AnimationCurveField, excludedProperties);
            }
            else if (easeType == EaseType.AnimationCurve)
            {
                height += GetOptionalHeight(property, AnimationCurveField, excludedProperties);

                ExcludeProperty(property, EaseField, excludedProperties);
            }
            else
            {
                ExcludeProperty(property, EaseField, excludedProperties);
                ExcludeProperty(property, AnimationCurveField, excludedProperties);
            }

            return height;
        }

        private static void DrawFromToFields(Rect position, SerializedProperty property, ref float currentY,
            HashSet<string> excludedProperties)
        {
            var useCustomProp = property.FindPropertyRelative(UseCustomFromAndToField);

            if (useCustomProp == null)
            {
                return;
            }

            EditorGUI.PropertyField(CreateRect(position, ref currentY, useCustomProp), useCustomProp);
            excludedProperties.Add(useCustomProp.propertyPath);

            if (!useCustomProp.boolValue)
            {
                ExcludeProperty(property, FromField, excludedProperties);
                ExcludeProperty(property, ToField, excludedProperties);

                return;
            }

            DrawOptionalField(position, property, FromField, ref currentY, excludedProperties);
            DrawOptionalField(position, property, ToField, ref currentY, excludedProperties);
        }

        private static float GetFromToHeight(SerializedProperty property, HashSet<string> excludedProperties)
        {
            var useCustomProp = property.FindPropertyRelative(UseCustomFromAndToField);

            if (useCustomProp == null)
            {
                return 0f;
            }

            float height = EditorGUI.GetPropertyHeight(useCustomProp, true);
            excludedProperties.Add(useCustomProp.propertyPath);

            if (useCustomProp.boolValue)
            {
                height += GetOptionalHeight(property, FromField, excludedProperties);
                height += GetOptionalHeight(property, ToField, excludedProperties);
            }
            else
            {
                ExcludeProperty(property, FromField, excludedProperties);
                ExcludeProperty(property, ToField, excludedProperties);
            }

            return height;
        }

        private static void DrawRemainingProperties(Rect position, SerializedProperty property, ref float currentY,
            HashSet<string> excludedProperties)
        {
            var iterator = property.Copy();
            var endProperty = iterator.GetEndProperty();
            bool enterChildren = true;

            while (iterator.NextVisible(enterChildren) && !SerializedProperty.EqualContents(iterator, endProperty))
            {
                if (!excludedProperties.Contains(iterator.propertyPath))
                {
                    EditorGUI.PropertyField(CreateRect(position, ref currentY, iterator), iterator, true);
                }

                enterChildren = false;
            }
        }

        private static float GetRemainingPropertiesHeight(SerializedProperty property, HashSet<string> excludedProperties)
        {
            float height = 0f;

            bool enterChildren = true;
            bool hasContent = false;

            var iterator = property.Copy();
            var endProperty = iterator.GetEndProperty();

            while (iterator.NextVisible(enterChildren) && !SerializedProperty.EqualContents(iterator, endProperty))
            {
                if (!excludedProperties.Contains(iterator.propertyPath))
                {
                    height += EditorGUI.GetPropertyHeight(iterator, true) + VerticalSpacing;
                    hasContent = true;
                }

                enterChildren = false;
            }

            if (hasContent)
            {
                height -= VerticalSpacing;
            }

            return height;
        }

        private static void DrawOptionalField(Rect position, SerializedProperty property, string fieldName,
            ref float currentY, HashSet<string> excludedProperties)
        {
            var field = property.FindPropertyRelative(fieldName);

            if (field == null)
            {
                return;
            }

            EditorGUI.PropertyField(CreateRect(position, ref currentY, field), field, true);
            excludedProperties.Add(field.propertyPath);
        }

        private static float GetOptionalHeight(SerializedProperty property, string fieldName,
            HashSet<string> excludedProperties)
        {
            var field = property.FindPropertyRelative(fieldName);

            if (field == null)
            {
                return 0f;
            }

            excludedProperties.Add(field.propertyPath);
            return EditorGUI.GetPropertyHeight(field, true) + VerticalSpacing;
        }

        private static Rect CreateRect(Rect position, ref float currentY, SerializedProperty property)
        {
            float height = EditorGUI.GetPropertyHeight(property, true);
            var rect = new Rect(position.x, currentY, position.width, height);

            currentY = rect.yMax + VerticalSpacing;

            return rect;
        }

        private static Rect CreateSingleLineRect(Rect position, ref float currentY)
        {
            var rect = new Rect(position.x, currentY, position.width, LineHeight);
            currentY = rect.yMax + VerticalSpacing;

            return rect;
        }

        private static void ExcludeProperty(SerializedProperty property, string fieldName,
            HashSet<string> excludedProperties)
        {
            var field = property.FindPropertyRelative(fieldName);
            
            if (field != null)
            {
                excludedProperties.Add(field.propertyPath);
            }
        }
    }
}
