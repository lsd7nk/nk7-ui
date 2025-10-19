using UnityEngine;
using System;

namespace Nk7.UI.Animations
{
    [Serializable]
    public class AnimationsContainer<TAnimation> where TAnimation : Animation
    {
        protected const float MAX_START_DELAY = 10000f;
        protected const float MIN_TOTAL_DURATION = 0f;

        public virtual bool IsEnabled
        {
            get
            {
                return AnimationType switch
                {
                    AnimationType.None => false,
                    _ => Rotate.IsEnabled || Scale.IsEnabled
                };
            }
        }

        public virtual float StartDelay
        {
            get
            {
                if (!IsEnabled)
                {
                    return 0;
                }

                return Mathf.Min(Rotate.IsEnabled ? Rotate.StartDelay : MAX_START_DELAY,
                                 Scale.IsEnabled ? Scale.StartDelay : MAX_START_DELAY);
            }
        }

        public virtual float Duration
        {
            get
            {
                if (!IsEnabled)
                {
                    return 0;
                }

                return Mathf.Max(Rotate.IsEnabled ? Rotate.TotalDuration : MIN_TOTAL_DURATION,
                                 Scale.IsEnabled ? Scale.TotalDuration : MIN_TOTAL_DURATION);
            }
        }

#if UNITY_EDITOR
        [field: ReadOnly]
#endif
        [field: SerializeField] public AnimationType AnimationType { get; protected set; } = AnimationType.None;

        [field: SerializeField] public TAnimation Rotate { get; protected set; }
        [field: SerializeField] public TAnimation Scale { get; protected set; }

        public AnimationsContainer(AnimationType animationType)
        {
            Reset(animationType);
        }

        protected virtual void Reset(AnimationType animationType)
        {
            AnimationType = animationType;
        }
    }
}