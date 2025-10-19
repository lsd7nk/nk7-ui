using UnityEngine;
using System;

namespace Nk7.UI.Animations
{
    [Serializable]
    public sealed class LoopAnimationsContainer : AnimationsContainer<LoopAnimation<Vector3>>
    {
        public int Cycles
        {
            get
            {
                if (!IsEnabled)
                {
                    return 0;
                }

                return Mathf.Max(Move.IsEnabled ? Move.Cycles : 0,
                                 Rotate.IsEnabled ? Rotate.Cycles : 0,
                                 Scale.IsEnabled ? Scale.Cycles : 0,
                                 Fade.IsEnabled ? Fade.Cycles : 0);
            }
        }

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

        [field: SerializeField] public LoopAnimation<Vector3> Move { get; private set; }
        [field: SerializeField] public LoopAnimation<float> Fade { get; private set; }

        public LoopAnimationsContainer() : base(AnimationType.Loop) { }

        protected override void Reset(AnimationType animationType)
        {
            base.Reset(animationType);

            Move = new LoopAnimation<Vector3>(animationType);
            Fade = new LoopAnimation<float>(animationType);
        }
    }
}
