using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using System;

namespace Nk7.UI.Animations
{
    [Serializable]
    public sealed class InteractableBehaviour : AnimationBehaviour, IAsyncExecutable
    {
        protected override int MaxTweensCount => 3;

        [SerializeField] private PunchAnimationsContainer _animations;

        public InteractableBehaviour() : base(AnimationType.Punch) { }

        public async UniTask ExecuteAsync(Container animatedContainer, CancellationToken cancellationToken = default,
            Action onStartCallback = null, Action onFinishCallback = null)
        {
            if (AnimationProcessed)
            {
                return;
            }

            _onStartEvent.Invoke();
            onStartCallback?.Invoke();

#pragma warning disable CS4014
            if (_animations.Move.IsEnabled)
            {
                animatedContainer.ResetPosition();
                AddAnimation(Animator.PunchMove(animatedContainer.RectTransform, _animations.Move));
            }

            if (_animations.Rotate.IsEnabled)
            {
                animatedContainer.ResetRotation();
                AddAnimation(Animator.PunchRotate(animatedContainer.RectTransform, _animations.Rotate));
            }

            if (_animations.Scale.IsEnabled)
            {
                animatedContainer.ResetScale();
                AddAnimation(Animator.PunchScale(animatedContainer.RectTransform, _animations.Scale));
            }
#pragma warning restore CS4014

            await WaitEndOfAnimation(_animations.Duration, cancellationToken);

            _onFinishEvent.Invoke();
            onFinishCallback?.Invoke();
        }

        protected override void Reset(AnimationType animationType)
        {
            _animations = new PunchAnimationsContainer();

            base.Reset(animationType);
        }
    }
}
