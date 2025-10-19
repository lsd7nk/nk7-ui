using UnityEngine;
using System;

namespace Nk7.UI.Animations
{
    [Serializable]
    public sealed class MoveAnimation : Animation<Vector3>
    {
        [field: SerializeField] public Vector3 CustomPosition { get; private set; }
        [field: SerializeField] public DirectionType Direction { get; private set; }

        public MoveAnimation(AnimationType animationType) : base(animationType)
        {
            CustomPosition = default;
            Direction = AnimatorConstants.DIRECTION;
        }
    }
}
