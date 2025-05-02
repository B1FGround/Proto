using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollBlocker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] ScrollRect scrollRect;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (scrollRect != null)
            scrollRect.enabled = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (scrollRect != null)
            scrollRect.enabled = true;
    }
}