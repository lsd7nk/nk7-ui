using UnityEngine;

#if NK7_CONTAINER
using Nk7.Container;
#endif

namespace Nk7.UI
{
    [DisallowMultipleComponent]
    public sealed class Canvas : MonoBehaviour
#if NK7_CONTAINER
        , IContainerRegistrable
#endif
    {
        [field: SerializeField] public RectTransform PopupParent { get; private set; }
    }
}