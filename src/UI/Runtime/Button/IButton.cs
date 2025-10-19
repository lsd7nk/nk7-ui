using UnityEngine.EventSystems;
using System;

namespace Nk7.UI
{
    public interface IButton : IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        public event Action OnPointerClickEvent;
        public event Action OnPointerDownEvent;
        public event Action OnPointerUpEvent;
    }
}