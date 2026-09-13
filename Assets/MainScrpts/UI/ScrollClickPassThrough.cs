using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollClickPassThrough : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    private ScrollRect scrollRect;

    void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PassEvent(eventData, ExecuteEvents.pointerDownHandler);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PassEvent(eventData, ExecuteEvents.pointerUpHandler);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Jeśli podczas kliknięcia nastąpiło przeciąganie scrolla, ignorujemy klik
        if (!eventData.dragging)
        {
            PassEvent(eventData, ExecuteEvents.pointerClickHandler);
        }
    }

    private void PassEvent<T>(PointerEventData eventData, ExecuteEvents.EventFunction<T> function) where T : IEventSystemHandler
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            // Ignorujemy sam ScrollRect, Viewport i obiekty będące jego dziećmi
            if (result.gameObject == gameObject || result.gameObject.transform.IsChildOf(transform))
                continue;

            // Przekazujemy zdarzenie pierwszemu obiektowi pod spodem, który potrafi je obsłużyć
            if (ExecuteEvents.ExecuteHierarchy(result.gameObject, eventData, function))
            {
                break;
            }
        }
    }
}