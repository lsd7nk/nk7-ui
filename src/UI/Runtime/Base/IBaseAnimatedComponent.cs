using System;

namespace Nk7.UI
{
    public interface IBaseAnimatedComponent
    {
        event Action OnShowStartEvent;
        event Action OnShowFinishEvent;

        event Action OnHideStartEvent;
        event Action OnHideFinishEvent;
    }
}