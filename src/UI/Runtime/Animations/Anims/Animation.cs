using UnityEngine;
using PrimeTween;
using System;

namespace Nk7.UI.Animations
{
    [Serializable]
    public sealed class PunchAnimation : ByAnimation<Vector3>
    {
        [field: Space, SerializeField] public int Frequency { get; private set; }
        [field: SerializeField] public float AsymmetryFactor { get; private set; }

        public PunchAnimation() : base(AnimationType.Punch) { }

        public override void Reset(AnimationType animationType)
        {
            base.Reset(animationType);

            Frequency = AnimatorConstants.FREQUENCY;
            AsymmetryFactor = AnimatorConstants.ASYMMETRY_FACTOR;
        }
    }


    [Serializable]
    public class LoopAnimation<T> : EasyByAnimation<T> where T : struct
    {
        [field: Space, SerializeField] public CycleMode CycleMode { get; private set; }
        [field: SerializeField] public int Cycles { get; private set; }

        public LoopAnimation(AnimationType animationType) : base(animationType) { }

        public override void Reset(AnimationType animationType)
        {
            base.Reset(animationType);

            Cycles = AnimatorConstants.CYCLES;
            CycleMode = AnimatorConstants.CYCLE_MODE;
        }
    }

    [Serializable]
    public class ByAnimation<T> : Animation where T : struct
    {
        [field: Space, SerializeField] public T By { get; private set; }

        public ByAnimation(AnimationType animationType) : base(animationType) { }

        public override void Reset(AnimationType animationType)
        {
            base.Reset(animationType);

            By = default;
        }
    }

    [Serializable]
    public class EasyByAnimation<T> : EasyAnimation where T : struct
    {
        [field: Space, SerializeField] public T By { get; private set; }

        public EasyByAnimation(AnimationType animationType) : base(animationType) { }

        public override void Reset(AnimationType animationType)
        {
            base.Reset(animationType);

            By = default;
        }
    }


    [Serializable]
    public class Animation<T> : EasyAnimation where T : struct
    {
        [field: Space, SerializeField] public bool UseCustomFromAndTo { get; private set; }

        [field: SerializeField] public T From { get; private set; }
        [field: SerializeField] public T To { get; private set; }

        public Animation(AnimationType animationType) : base(animationType) { }

        public override void Reset(AnimationType animationType)
        {
            base.Reset(animationType);

            UseCustomFromAndTo = AnimatorConstants.USE_CUSTOM_FROM_AND_TO;

            From = default;
            To = default;
        }
    }

    [Serializable]
    public abstract class EasyAnimation : Animation
    {
        [field: Space, SerializeField] public EaseType EaseType { get; private set; }

        [field: SerializeField] public Ease Ease { get; private set; }
        [field: SerializeField] public AnimationCurve AnimationCurve { get; private set; }

        public EasyAnimation(AnimationType animationType) : base(animationType) { }

        public override void Reset(AnimationType animationType)
        {
            base.Reset(animationType);

            EaseType = AnimatorConstants.EASY_TYPE;
            Ease = AnimatorConstants.EASE;
            AnimationCurve = new AnimationCurve();
        }

        public Easing GetEasing()
        {
            return EaseType switch
            {
                EaseType.Ease => Ease,
                EaseType.AnimationCurve => AnimationCurve,
                _ => throw new NotImplementedException()
            };
        }
    }


    [Serializable]
    public abstract class Animation
    {
        public float TotalDuration
        {
            get
            {
                return StartDelay + Duration;
            }
        }

        public AnimationType AnimationType { get; private set; } = AnimationType.None;

        [field: SerializeField] public bool IsEnabled { get; private set; }

        [field: SerializeField] public float StartDelay { get; private set; }
        [field: SerializeField] public float Duration { get; private set; }

        public Animation(AnimationType animationType)
        {
            Reset(animationType);
        }

        public virtual void Reset(AnimationType animationType)
        {
            AnimationType = animationType;

            IsEnabled = AnimatorConstants.IS_ENABLED;

            StartDelay = AnimatorConstants.START_DELAY;
            Duration = AnimatorConstants.DURATION;
        }
    }
}