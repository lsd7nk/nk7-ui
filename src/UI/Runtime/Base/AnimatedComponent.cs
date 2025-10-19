using Cysharp.Threading.Tasks;
using Nk7.UI.Animations;
using System.Threading;
using UnityEngine;
using System;

namespace Nk7.UI
{
    [DisallowMultipleComponent]
    public abstract class AnimatedComponent : MonoBehaviour, IAnimatedComponent
    {
        public event Action OnShowStartEvent;
        public event Action OnShowFinishEvent;

        public event Action OnHideStartEvent;
        public event Action OnHideFinishEvent;

        [SerializeField] protected Container _animatedContainer;

        [Space(10), Header(Utils.ANIMATED_COMPONENT)]
        [SerializeField] private NotInteractableBehaviour _showBehaviour;
        [SerializeField] private NotInteractableBehaviour _hideBehaviour;

        public AnimatedComponent()
        {
            Reset();
        }

        public void Show(bool withoutAnimation = false)
        {
            if (!withoutAnimation)
            {
                ShowAsync().Forget();
            }
            else
            {
                ShowInstantly();
            }
        }

        public void Hide(bool withoutAnimation = false)
        {
            if (!withoutAnimation)
            {
                HideAsync().Forget();
            }
            else
            {
                HideInstantly();
            }
        }

        public async UniTask ShowAsync(CancellationToken cancellationToken = default)
        {
            if (_hideBehaviour.AnimationProcessed)
            {
                return;
            }

            await _showBehaviour.ExecuteAsync(_animatedContainer, cancellationToken,
                OnShowStartEvent, OnShowFinishEvent);
        }

        public async virtual UniTask HideAsync(CancellationToken cancellationToken = default)
        {
            if (_showBehaviour.AnimationProcessed)
            {
                return;
            }

            await _hideBehaviour.ExecuteAsync(_animatedContainer, cancellationToken,
                OnHideStartEvent, OnHideFinishEvent);
        }

        public void ShowInstantly()
        {
            _showBehaviour.ExecuteInstantly(_animatedContainer);
        }

        public virtual void HideInstantly()
        {
            _hideBehaviour.ExecuteInstantly(_animatedContainer);
        }

        protected virtual void Reset()
        {
            _showBehaviour = new NotInteractableBehaviour(NotInteractableAnimationType.Show);
            _hideBehaviour = new NotInteractableBehaviour(NotInteractableAnimationType.Hide);
        }

		private void OnDestroy()
		{
            OnShowStartEvent = null;
            OnShowFinishEvent = null;
            OnHideStartEvent = null;
            OnHideFinishEvent = null;
		}
	}
}