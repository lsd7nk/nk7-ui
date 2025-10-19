using UnityEngine;
using PrimeTween;

namespace Nk7.UI.Animations
{
    public static class Animator
    {
        public const float LOOP_DURATION_MULTIPLIER = 0.5f;

        #region animations (loop)
        public static Tween StartLoopMove(RectTransform target, LoopAnimation<Vector3> animation,
            Vector3 startValue)
        {
            var positionA = startValue - animation.By;

            return Tween.UIAnchoredPosition(target, positionA, animation.Duration * LOOP_DURATION_MULTIPLIER,
                    animation.GetEasing(), startDelay: animation.StartDelay);
        }

        public static Tween StartLoopRotate(RectTransform target, LoopAnimation<Vector3> animation,
            Vector3 startValue)
        {
            var rotationA = startValue - animation.By;

            return Tween.LocalRotation(target, rotationA, animation.Duration * LOOP_DURATION_MULTIPLIER,
                    animation.GetEasing(), startDelay: animation.StartDelay);
        }

        public static Tween StartLoopScale(RectTransform target, LoopAnimation<Vector3> animation,
            Vector3 startValue)
        {
            var scaleA = startValue - animation.By;

            scaleA.z = 1f;

            return Tween.Scale(target, scaleA, animation.Duration * LOOP_DURATION_MULTIPLIER,
                    animation.GetEasing(), startDelay: animation.StartDelay);
        }

        public static Tween StartLoopFade(CanvasGroup target, LoopAnimation<float> animation,
            float startValue)
        {
            var fadeA = startValue - animation.By;

            return Tween.Alpha(target, fadeA, animation.Duration * LOOP_DURATION_MULTIPLIER,
                    animation.GetEasing(), startDelay: animation.StartDelay);
        }

        public static Tween LoopMove(RectTransform target, LoopAnimation<Vector3> animation,
            Vector3 startValue)
        {
            var positionB = startValue + animation.By;

            return Tween.UIAnchoredPosition(target, positionB, animation.Duration * LOOP_DURATION_MULTIPLIER,
                    animation.GetEasing(), animation.Cycles, animation.CycleMode);
        }

        public static Tween LoopRotate(RectTransform target, LoopAnimation<Vector3> animation,
            Vector3 startValue)
        {
            var rotationB = startValue + animation.By;

            return Tween.LocalRotation(target, rotationB, animation.Duration * LOOP_DURATION_MULTIPLIER,
                    animation.GetEasing(), animation.Cycles, animation.CycleMode);
        }

        public static Tween LoopScale(RectTransform target, LoopAnimation<Vector3> animation,
            Vector3 startValue)
        {
            var scaleB = startValue + animation.By;

            scaleB.z = 1f;

            return Tween.Scale(target, scaleB, animation.Duration * LOOP_DURATION_MULTIPLIER,
                    animation.GetEasing(), animation.Cycles, animation.CycleMode);
        }

        public static Tween LoopFade(CanvasGroup target, LoopAnimation<float> animation,
            float startValue)
        {
            var fadeB = startValue + animation.By;

            return Tween.Alpha(target, fadeB, animation.Duration * LOOP_DURATION_MULTIPLIER,
                    animation.GetEasing(), animation.Cycles, animation.CycleMode);
        }
        #endregion

        #region animations (punch & state)
        public static Tween PunchMove(RectTransform target, PunchAnimation animation)
        {
            return Tween.PunchLocalPosition(target, animation.By, animation.Duration, animation.Frequency, asymmetryFactor: animation.AsymmetryFactor, startDelay: animation.StartDelay);
        }

        public static Tween PunchRotate(RectTransform target, PunchAnimation animation)
        {
            return Tween.PunchLocalRotation(target, animation.By, animation.Duration, animation.Frequency, asymmetryFactor: animation.AsymmetryFactor, startDelay: animation.StartDelay);
        }

        public static Tween PunchScale(RectTransform target, PunchAnimation animation)
        {
            return Tween.PunchScale(target, animation.By, animation.Duration, animation.Frequency, asymmetryFactor: animation.AsymmetryFactor, startDelay: animation.StartDelay);
        }
        #endregion

        #region animations (show & hide)
        public static void InstantlyMove(RectTransform target, Vector3 endValue)
        {
            target.anchoredPosition3D = endValue;
        }

        public static Tween Move(RectTransform target, MoveAnimation animation,
            Vector3 startValue, Vector3 endValue)
        {
            return Tween.UIAnchoredPosition3D(target, startValue, endValue, animation.Duration,
                animation.GetEasing(), startDelay: animation.StartDelay);
        }

        public static void InstantlyRotate(RectTransform target, Vector3 endValue)
        {
            target.localRotation = Quaternion.Euler(endValue);
        }

        public static Tween Rotate(RectTransform target, Animation<Vector3> animation,
            Vector3 startValue, Vector3 endValue)
        {
            return Tween.LocalRotation(target, startValue, endValue, animation.Duration,
                animation.GetEasing(), startDelay: animation.StartDelay);
        }

        public static void InstantlyScale(RectTransform target, Vector3 endValue)
        {
            endValue.z = 1f;
            target.localScale = endValue;
        }

        public static Tween Scale(RectTransform target, Animation<Vector3> animation,
            Vector3 startValue, Vector3 endValue)
        {
            startValue.z = 1f;
            endValue.z = 1f;

            return Tween.Scale(target, startValue, endValue, animation.Duration,
                animation.GetEasing(), startDelay: animation.StartDelay);
        }

        public static void InstantlyFade(CanvasGroup target, float endValue)
        {
            endValue = Mathf.Clamp01(endValue);
            target.alpha = endValue;
        }

        public static Tween Fade(CanvasGroup target, Animation<float> animation,
            float startValue, float endValue)
        {
            endValue = Mathf.Clamp01(endValue);
            startValue = Mathf.Clamp01(startValue);

            return Tween.Alpha(target, startValue, endValue, animation.Duration,
                animation.GetEasing(), startDelay: animation.StartDelay);
        }
        #endregion
    }
}