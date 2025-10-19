using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;
using System.Threading;
using UnityEngine;
using PrimeTween;
using System;

namespace Nk7.UI.Animations
{
    [Serializable]
    public abstract class AnimationBehaviour
    {
        public bool AnimationProcessed { get; private set; }

        protected abstract int MaxTweensCount { get; }

        [SerializeField] protected UnityEvent _onStartEvent;
        [SerializeField] protected UnityEvent _onFinishEvent;

        private List<Tween> _tweens;

        public AnimationBehaviour(AnimationType animationType)
        {
            Reset(animationType);
        }

        protected async UniTask WaitEndOfAnimation(float duration, CancellationToken cancellationToken = default)
        {
            SetAnimationProcessed();

            try
            {
                await UniTask.Delay((int)(duration * AnimatorConstants.UNI_TASK_DELAY_MULTIPLIER),
                    cancellationToken: cancellationToken);
            }
            catch (OperationCanceledException) { }
            finally
            {
                AnimationProcessed = false;
                StopAnimations();
            }
        }

        protected void SetAnimationProcessed()
        {
            AnimationProcessed = true;
        }

        protected void AddAnimation(Tween tween)
        {
            _tweens.Add(tween);
        }

        protected virtual void Reset(AnimationType animationType)
        {
            AnimationProcessed = false;

            _onStartEvent?.RemoveAllListeners();
            _onFinishEvent?.RemoveAllListeners();

            _tweens = new List<Tween>(MaxTweensCount);
        }

        protected void StopAnimations()
        {
            for (int i = 0; i < _tweens.Count; ++i)
            {
                var tween = _tweens[i];

                if (!tween.isAlive)
                {
                    continue;
                }

                tween.Stop();
            }

            _tweens.Clear();
            AnimationProcessed = false;
        }
    }
}