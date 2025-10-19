using UnityEngine;
using PrimeTween;

namespace Nk7.UI.Animations
{
    public static class AnimatorConstants
    {
        public static Vector3 START_POSITION = Vector3.zero;
        public static Vector3 START_ROTATION = Vector3.zero;
        public static Vector3 START_SCALE = Vector3.one;

        public const int UNI_TASK_DELAY_MULTIPLIER = 1000;

        public const float START_ALPHA = 1f;

        public const bool IS_ENABLED = false;
        public const bool USE_CUSTOM_FROM_AND_TO = false;

        public const float START_DELAY = 0f;
        public const float DURATION = 1f;

        public const int FREQUENCY = 10;
        public const float ASYMMETRY_FACTOR = 1;
        public const int CYCLES = -1;

        public const CycleMode CYCLE_MODE = CycleMode.Yoyo;
        public const EaseType EASY_TYPE = EaseType.Ease;

        public const Ease EASE = Ease.Linear;

        public const DirectionType DIRECTION = DirectionType.Left;
    }
}