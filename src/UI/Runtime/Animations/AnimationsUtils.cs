using System;

namespace Nk7.UI.Animations
{
    public static class AnimationsUtils
    {
        public static NotInteractableAnimationType GetNotInteractableAnimationType(AnimationType animationType)
        {
            return animationType switch
            {
                AnimationType.Show => NotInteractableAnimationType.Show,
                AnimationType.Hide => NotInteractableAnimationType.Hide,
                _ => throw new NotImplementedException("Not supported animation type for NotInteractableBehaviour")
            };
        }

        public static AnimationType GetAnimationType(NotInteractableAnimationType animationType)
        {
            return animationType switch
            {
                NotInteractableAnimationType.Show => AnimationType.Show,
                NotInteractableAnimationType.Hide => AnimationType.Hide,
                _ => throw new NotImplementedException("Not supported animation type for AnimatedBehaviour")
            };
        }
    }
}
