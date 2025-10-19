using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using System;

namespace Nk7.UI.Animations
{
    [Serializable]
    public sealed class LoopBehaviour : AnimationBehaviour, IAsyncExecutable
    {
        protected override int MaxTweensCount => 8;

        [SerializeField] private LoopAnimationsContainer _animations;

        public LoopBehaviour() : base(AnimationType.Loop)
        {
            Reset();
        }

        public async UniTask ExecuteAsync(Container animatedContainer, CancellationToken cancellationToken = default,
            Action onStartCallback = null, Action onFinishCallback = null)
        {
            if (AnimationProcessed)
            {
                return;
            }

            _onStartEvent.Invoke();
            onStartCallback?.Invoke();

            if (_animations.Move.IsEnabled)
            {
                AddAnimation(Animator.StartLoopMove(animatedContainer.RectTransform, _animations.Move,
                    animatedContainer.StartPosition)
                    .OnComplete(target: this, target => target.LoopMove(animatedContainer)));
            }

            if (_animations.Rotate.IsEnabled)
            {
                AddAnimation(Animator.StartLoopRotate(animatedContainer.RectTransform, _animations.Rotate,
                    animatedContainer.StartRotation)
                    .OnComplete(target: this, target => target.LoopRotate(animatedContainer)));
            }

            if (_animations.Scale.IsEnabled)
            {
                AddAnimation(Animator.StartLoopScale(animatedContainer.RectTransform, _animations.Scale,
                    animatedContainer.StartScale)
                    .OnComplete(target: this, target => target.LoopScale(animatedContainer)));
            }

            if (_animations.Fade.IsEnabled)
            {
                AddAnimation(Animator.StartLoopFade(animatedContainer.CanvasGroup, _animations.Fade,
                    animatedContainer.StartAlpha)
                    .OnComplete(target: this, target => target.LoopFade(animatedContainer)));
            }

            int cycles = _animations.Cycles;

            if (cycles != AnimatorConstants.CYCLES)
            {
                await WaitEndOfAnimation(AnimatorUtils.GetLoopDuration(_animations.Cycles, _animations.Duration), cancellationToken);

                _onFinishEvent.Invoke();
                onFinishCallback?.Invoke();

                return;
            }

            SetAnimationProcessed();
        }

        private void LoopMove(Container animatedContainer)
        {
            AddAnimation(Animator.LoopMove(animatedContainer.RectTransform, _animations.Move,
                animatedContainer.StartPosition));
        }

        private void LoopRotate(Container animatedContainer)
        {
            AddAnimation(Animator.LoopRotate(animatedContainer.RectTransform, _animations.Rotate,
                    animatedContainer.StartRotation));
        }

        private void LoopScale(Container animatedContainer)
        {
            AddAnimation(Animator.LoopScale(animatedContainer.RectTransform, _animations.Scale,
                    animatedContainer.StartScale));
        }

        private void LoopFade(Container animatedContainer)
        {
            AddAnimation(Animator.LoopFade(animatedContainer.CanvasGroup, _animations.Fade,
                    animatedContainer.StartAlpha));
        }

        private void Reset()
        {
            _animations = new LoopAnimationsContainer();
        }
    }
}