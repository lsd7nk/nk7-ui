using UnityEngine;
using System;

namespace Nk7.UI.Animations
{
    [Serializable]
    public sealed class AlphaAnimationsContainer : VectorAnimationsContainer
    {
        public override bool IsEnabled
        {
            get
            {
                return AnimationType switch
                {
                    AnimationType.None => false,
                    AnimationType.Punch => Move.IsEnabled || Rotate.IsEnabled || Scale.IsEnabled,
                    _ => Move.IsEnabled || Rotate.IsEnabled || Scale.IsEnabled || Fade.IsEnabled
                };
            }
        }

        public override float StartDelay
        {
            get
            {
                if (!IsEnabled)
                {
                    return 0;
                }

                return Mathf.Min(Move.IsEnabled ? Move.StartDelay : MAX_START_DELAY,
                                 Rotate.IsEnabled ? Rotate.StartDelay : MAX_START_DELAY,
                                 Scale.IsEnabled ? Scale.StartDelay : MAX_START_DELAY,
                                 Fade.IsEnabled ? Fade.StartDelay : MAX_START_DELAY);
            }
        }

        public override float Duration
        {
            get
            {
                if (!IsEnabled)
                {
                    return 0;
                }

                return Mathf.Max(Move.IsEnabled ? Move.TotalDuration : MIN_TOTAL_DURATION,
                                 Rotate.IsEnabled ? Rotate.TotalDuration : MIN_TOTAL_DURATION,
                                 Scale.IsEnabled ? Scale.TotalDuration : MIN_TOTAL_DURATION,
                                 Fade.IsEnabled ? Fade.TotalDuration : MIN_TOTAL_DURATION);
            }
        }

        [field: SerializeField] public Animation<float> Fade { get; private set; }

        public AlphaAnimationsContainer(AnimationType animationType) : base(animationType) { }

        protected override void Reset(AnimationType animationType)
        {
            base.Reset(animationType);

            Fade = new Animation<float>(animationType);
        }
    }
}