using UnityEngine;
using System;

namespace Nk7.UI.Animations
{
    [Serializable]
    public class VectorAnimationsContainer : AnimationsContainer<Animation<Vector3>>
    {
        public override bool IsEnabled
        {
            get
            {
                return AnimationType switch
                {
                    AnimationType.None => false,
                    _ => Move.IsEnabled || Rotate.IsEnabled || Scale.IsEnabled
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
                                 Scale.IsEnabled ? Scale.StartDelay : MAX_START_DELAY);
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
                                 Scale.IsEnabled ? Scale.TotalDuration : MIN_TOTAL_DURATION);
            }
        }

        [field: SerializeField] public MoveAnimation Move { get; private set; }

        public VectorAnimationsContainer(AnimationType animationType) : base(animationType) { }

        protected override void Reset(AnimationType animationType)
        {
            base.Reset(animationType);

            Rotate = new Animation<Vector3>(animationType);
            Scale = new Animation<Vector3>(animationType);
            Move = new MoveAnimation(animationType);
        }
    }
}
