using AnimationBase = Nk7.UI.Animations.Animation;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Events;
using System.Reflection;
using System.Text;
using UnityEditor;
using System;

namespace Nk7.UI.Editor
{
    internal static class AnimationInspectorUtility
    {
        private static readonly string[] s_AnimationOrder = { "Move", "Rotate", "Scale", "Fade" };

        internal readonly struct BehaviourSummary
        {
            public static BehaviourSummary Empty => new BehaviourSummary(null, null, null);

            public BehaviourSummary(string summary, string secondary, string warning)
            {
                Summary = summary;
                Secondary = secondary;
                Warning = warning;
            }

            public string Summary { get; }
            public string Secondary { get; }
            public string Warning { get; }
        }

        internal static BehaviourSummary InspectBehaviour(UnityEngine.Object target, string fieldName)
        {
            if (target == null || string.IsNullOrEmpty(fieldName))
            {
                return BehaviourSummary.Empty;
            }

            var behaviour = GetInstanceField(target, fieldName);

            if (behaviour == null)
            {
                return new BehaviourSummary("Not configured", null, "Behaviour reference is missing. Call Reset() or assign it manually.");
            }

            var behaviourType = behaviour.GetType();
            var animationsField = behaviourType.GetField("_animations", BindingFlags.Instance | BindingFlags.NonPublic);

            if (animationsField == null)
            {
                return new BehaviourSummary(ObjectNames.NicifyVariableName(behaviourType.Name), null, null);
            }

            var container = animationsField.GetValue(behaviour);

            if (container == null)
            {
                return new BehaviourSummary("Not configured", null, "Animation container is null. Use Reset() to regenerate default values.");
            }

            var containerType = container.GetType();
            var animationInfos = CollectAnimationInfos(container, containerType, out var missingAnimationNames);

            bool hasEventListeners = HasEventListeners(behaviour, behaviourType);
            int enabledAnimationsCount = 0;
            string enabledAnimationsSummary = null;
            
            StringBuilder enabledSummaryBuilder = null;

            for (int i = 0; i < animationInfos.Count; ++i)
            {
                var info = animationInfos[i];

                if (!info.IsEnabled)
                {
                    continue;
                }

                enabledSummaryBuilder ??= new StringBuilder(animationInfos.Count * 8);

                if (enabledAnimationsCount > 0)
                {
                    enabledSummaryBuilder.Append(", ");
                }

                enabledSummaryBuilder.Append(info.DisplayName);
                enabledAnimationsCount++;
            }

            if (enabledSummaryBuilder != null)
            {
                enabledAnimationsSummary = enabledSummaryBuilder.ToString();
            }

            bool isContainerEnabled = TryGetPropertyValue(containerType, container, "IsEnabled", out bool containerEnabled)
                ? containerEnabled
                : enabledAnimationsCount > 0;

            bool hasAnimations = enabledAnimationsCount > 0;
            bool isEffectivelyEnabled = isContainerEnabled || hasAnimations || hasEventListeners;

            var summaryParts = new List<string>(2);

            if (hasAnimations && !string.IsNullOrEmpty(enabledAnimationsSummary))
            {
                summaryParts.Add(enabledAnimationsSummary);
            }

            if (hasEventListeners)
            {
                summaryParts.Add("Events");
            }

            string summary;

            if (!isEffectivelyEnabled)
            {
                summary = animationInfos.Count == 0 && !hasEventListeners
                    ? "No animations"
                    : "Disabled";
            }
            else if (summaryParts.Count > 0)
            {
                summary = "Enabled: " + string.Join(", ", summaryParts);
            }
            else
            {
                summary = "Enabled";
            }

            string warning = null;

            if (missingAnimationNames.Count > 0)
            {
                warning = "Missing data: " + string.Join(", ", missingAnimationNames);
            }

            if (isContainerEnabled && enabledAnimationsCount == 0 && !hasEventListeners)
            {
                warning = string.IsNullOrEmpty(warning)
                    ? "All animations are disabled."
                    : warning + " All animations are disabled.";
            }

            string secondary = null;

            if (TryGetPropertyValue(containerType, container, "Duration", out float duration) && duration > 0f)
            {
                secondary = $"Duration {FormatSeconds(duration)}s";
            }

            if (TryGetPropertyValue(containerType, container, "StartDelay", out float startDelay) && startDelay > 0f)
            {
                secondary = string.IsNullOrEmpty(secondary)
                    ? $"Delay {FormatSeconds(startDelay)}s"
                    : $"{secondary} • Delay {FormatSeconds(startDelay)}s";
            }

            if (TryGetPropertyValue(containerType, container, "Cycles", out int cycles))
            {
                var cyclesLabel = cycles < 0 ? "\u221E" : cycles.ToString(CultureInfo.InvariantCulture);
                secondary = string.IsNullOrEmpty(secondary)
                    ? $"Cycles {cyclesLabel}"
                    : $"{secondary} • Cycles {cyclesLabel}";
            }

            return new BehaviourSummary(summary, secondary, warning);
        }

        internal static object GetInstanceField(UnityEngine.Object target, string fieldName)
        {
            if (target == null || string.IsNullOrEmpty(fieldName))
            {
                return null;
            }

            var type = target.GetType();

            while (type != null)
            {
                var field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                if (field != null)
                {
                    return field.GetValue(target);
                }

                type = type.BaseType;
            }

            return null;
        }

        private static List<AnimationInfo> CollectAnimationInfos(object container, Type containerType, out List<string> missing)
        {
            var infos = new List<AnimationInfo>();
            missing = new List<string>();

            var properties = containerType.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var property in properties)
            {
                if (!typeof(AnimationBase).IsAssignableFrom(property.PropertyType))
                {
                    continue;
                }

                var displayName = ObjectNames.NicifyVariableName(property.Name);
                var animationValue = property.GetValue(container);

                if (animationValue == null)
                {
                    missing.Add(displayName);
                    continue;
                }

                bool isEnabled = false;

                if (TryGetPropertyValue(animationValue.GetType(), animationValue, "IsEnabled", out bool enabled))
                {
                    isEnabled = enabled;
                }

                infos.Add(new AnimationInfo(property.Name, displayName, isEnabled));
            }

            var comparer = new AnimationInfoComparer();
            infos.Sort(comparer);

            return infos;
        }

        private static int GetAnimationOrder(string name)
        {
            int index = Array.IndexOf(s_AnimationOrder, name);
            return index >= 0 ? index : int.MaxValue;
        }

        private static string FormatSeconds(float value)
        {
            return value.ToString("0.###", CultureInfo.InvariantCulture);
        }

        private static bool TryGetPropertyValue<T>(Type type, object instance, string propertyName, out T value)
        {
            if (type == null || instance == null || string.IsNullOrEmpty(propertyName))
            {
                value = default;
                return false;
            }

            var property = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (property != null && typeof(T).IsAssignableFrom(property.PropertyType))
            {
                var propertyValue = property.GetValue(instance);

                if (propertyValue is T typedValue)
                {
                    value = typedValue;
                    return true;
                }
            }

            value = default;
            return false;
        }

        private readonly struct AnimationInfo
        {
            public AnimationInfo(string name, string displayName, bool isEnabled)
            {
                Name = name;
                DisplayName = displayName;
                IsEnabled = isEnabled;
            }

            public string Name { get; }
            public string DisplayName { get; }
            public bool IsEnabled { get; }
        }

        private static bool HasEventListeners(object behaviour, Type behaviourType)
        {
            if (behaviour == null || behaviourType == null)
            {
                return false;
            }

            for (var type = behaviourType; type != null; type = type.BaseType)
            {
                var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly);

                for (int i = 0; i < fields.Length; ++i)
                {
                    var field = fields[i];

                    if (!typeof(UnityEventBase).IsAssignableFrom(field.FieldType))
                    {
                        continue;
                    }

                    if (field.GetValue(behaviour) is UnityEventBase unityEvent &&
                        unityEvent.GetPersistentEventCount() > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private readonly struct AnimationInfoComparer : IComparer<AnimationInfo>
        {
            public int Compare(AnimationInfo x, AnimationInfo y)
            {
                return GetAnimationOrder(x.Name).CompareTo(GetAnimationOrder(y.Name));
            }
        }
    }
}
