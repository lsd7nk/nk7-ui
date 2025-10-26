using UnityEngine;

namespace Nk7.UI.Animations
{
    public static class AnimatorUtils
    {
        #region animations (loop)
        public static float GetLoopDuration(int cycles, float duration)
        {
            return cycles * duration * Animator.LOOP_DURATION_MULTIPLIER;
        }
        #endregion

        #region animations (show & hide)
        public static Vector3 GetMoveFrom(RectTransform target, MoveAnimation animation, Vector3 startValue)
        {
            return animation.AnimationType switch
            {
                AnimationType.Show => animation.UseCustomFromAndTo
                    ? animation.From
                    : GetToPositionByDirection(target, animation, animation.UseCustomFromAndTo
                        ? animation.CustomPosition
                        : startValue),

                AnimationType.Hide => animation.UseCustomFromAndTo
                    ? animation.From
                    : startValue,

                _ => AnimatorConstants.START_POSITION
            };
        }

        public static Vector3 GetMoveTo(RectTransform target, MoveAnimation animation, Vector3 startValue)
        {
            return animation.AnimationType switch
            {
                AnimationType.Show => animation.UseCustomFromAndTo
                    ? animation.To
                    : startValue,

                AnimationType.Hide => animation.UseCustomFromAndTo
                    ? animation.To
                    : GetToPositionByDirection(target, animation, animation.UseCustomFromAndTo
                        ? animation.CustomPosition
                        : startValue),

                _ => AnimatorConstants.START_POSITION
            };
        }

        public static Vector3 GetRotateFrom(Animation<Vector3> animation, Vector3 startValue)
        {
            return animation.AnimationType switch
            {
                AnimationType.Show => animation.From,
                AnimationType.Hide => animation.UseCustomFromAndTo
                    ? animation.From
                    : startValue,

                _ => AnimatorConstants.START_ROTATION
            };
        }

        public static Vector3 GetRotateTo(Animation<Vector3> animation, Vector3 startValue)
        {
            return animation.AnimationType switch
            {
                AnimationType.Show => animation.UseCustomFromAndTo
                    ? animation.To
                    : startValue,
                AnimationType.Hide => animation.To,

                _ => AnimatorConstants.START_POSITION
            };
        }

        public static Vector3 GetScaleFrom(Animation<Vector3> animation, Vector3 startValue)
        {
            return animation.AnimationType switch
            {
                AnimationType.Show => animation.From,
                AnimationType.Hide => animation.UseCustomFromAndTo
                    ? animation.From
                    : startValue,

                _ => AnimatorConstants.START_SCALE
            };
        }

        public static Vector3 GetScaleTo(Animation<Vector3> animation, Vector3 startValue)
        {
            return animation.AnimationType switch
            {
                AnimationType.Show => animation.UseCustomFromAndTo
                    ? animation.To
                    : startValue,
                AnimationType.Hide => animation.To,

                _ => AnimatorConstants.START_SCALE
            };
        }

        public static float GetFadeFrom(Animation<float> animation, float startValue)
        {
            return animation.AnimationType switch
            {
                AnimationType.Show => animation.From,
                AnimationType.Hide => animation.UseCustomFromAndTo
                    ? animation.From
                    : startValue,

                _ => AnimatorConstants.START_ALPHA
            };
        }

        public static float GetFadeTo(Animation<float> animation, float startValue)
        {
            return animation.AnimationType switch
            {
                AnimationType.Show => animation.UseCustomFromAndTo
                    ? animation.To
                    : startValue,
                AnimationType.Hide => animation.To,

                _ => AnimatorConstants.START_ALPHA
            };
        }

        private static Vector3 GetToPositionByDirection(RectTransform target, MoveAnimation animation, Vector3 startValue)
        {
            var rootCanvas = target.GetComponent<Canvas>().rootCanvas;
            var rootCanvasRect = rootCanvas.GetComponent<RectTransform>().rect;

            float xOffset = rootCanvasRect.width / 2 + target.rect.width * target.pivot.x;
            float yOffset = rootCanvasRect.height / 2 + target.rect.height * target.pivot.y;

            return animation.Direction switch
            {
                DirectionType.Left => new Vector3(-xOffset, startValue.y, startValue.z),
                DirectionType.Right => new Vector3(xOffset, startValue.y, startValue.z),
                DirectionType.Top => new Vector3(startValue.x, yOffset, startValue.z),
                DirectionType.Bottom => new Vector3(startValue.x, -yOffset, startValue.z),
                DirectionType.TopLeft => new Vector3(-xOffset, yOffset, startValue.z),
                DirectionType.TopCenter => new Vector3(0, yOffset, startValue.z),
                DirectionType.TopRight => new Vector3(xOffset, yOffset, startValue.z),
                DirectionType.MiddleLeft => new Vector3(-xOffset, 0, startValue.z),
                DirectionType.MiddleCenter => new Vector3(0, 0, startValue.z),
                DirectionType.MiddleRight => new Vector3(xOffset, 0, startValue.z),
                DirectionType.BottomLeft => new Vector3(-xOffset, -yOffset, startValue.z),
                DirectionType.BottomCenter => new Vector3(0, -yOffset, startValue.z),
                DirectionType.BottomRight => new Vector3(xOffset, -yOffset, startValue.z),
                DirectionType.CustomPosition => animation.CustomPosition,
                _ => Vector3.zero
            };
        }
        #endregion
    }
}