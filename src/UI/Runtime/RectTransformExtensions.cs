using UnityEngine;

namespace Nk7.UI
{
    public static class RectTransformExtensions
    {
        private static readonly Vector2 HALF_VECTOR2 = new Vector2(0.5f, 0.5f);

        public static void SetFullScreen(this RectTransform target, bool resetScaleToOne)
        {
            if (resetScaleToOne)
            {
                target.ResetLocalScaleToOne();
            }

            target.SetAnchorMinToZero();
            target.SetAnchorMaxToOne();

            target.SetCenterPivot();
            target.SetSizeDeltaToZero();

            target.ResetAnchoredPosition3D();
            target.ResetLocalPosition();
        }

        public static void SetToCenter(this RectTransform target, bool resetScaleToOne)
        {
            if (resetScaleToOne)
            {
                target.ResetLocalScaleToOne();
            }

            target.SetAnchorMinToCenter();
            target.SetAnchorMaxToCenter();

            target.SetCenterPivot();
            target.SetSizeDeltaToZero();
        }

        public static void SetAnchorMaxToCenter(this RectTransform target)
        {
            target.anchorMax = HALF_VECTOR2;
        }

        public static void SetAnchorMinToCenter(this RectTransform target)
        {
            target.anchorMin = HALF_VECTOR2;
        }

        public static void SetAnchorMinToZero(this RectTransform target)
        {
            target.anchorMin = Vector2.zero;
        }

        public static void SetAnchorMaxToOne(this RectTransform target)
        {
            target.anchorMax = Vector2.one;
        }

        public static void SetCenterPivot(this RectTransform target)
        {
            target.pivot = HALF_VECTOR2;
        }

        public static void SetSizeDeltaToZero(this RectTransform target)
        {
            target.sizeDelta = Vector2.zero;
        }

        public static void ResetAnchoredPosition3D(this RectTransform target)
        {
            target.anchoredPosition3D = Vector3.zero;
        }

        public static void ResetLocalPosition(this RectTransform target)
        {
            target.localPosition = Vector3.zero;
        }

        public static void ResetLocalScaleToOne(this RectTransform target)
        {
            target.localScale = Vector3.one;
        }
    }
}