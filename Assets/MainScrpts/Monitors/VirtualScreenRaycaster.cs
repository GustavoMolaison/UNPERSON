using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualScreenRaycaster : MonoBehaviour
{
    [Header("G³ówna kamera i monitor")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask screenLayer; // warstwa monitora 3D

    [Header("Wirtualny Canvas i kamera")]
    [SerializeField] private RenderTexture renderTexture; // Przeci¹gnij tu swój 'New Render Texture'
    [SerializeField] private Canvas virtualCanvas;

    private PointerEventData pointerData;

    void Update()
    {
        // Sprawdzamy klikniêcie myszy
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
        {
            HandleScreenClick();
        }
    }

    private void HandleScreenClick()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, screenLayer))
        {
            Vector2 uv = hit.textureCoord;
            

            if (uv == Vector2.zero)
            {
              
            }

            Vector2 virtualScreenPos = new Vector2(
            uv.x * renderTexture.width,
            (1f - uv.y) * renderTexture.height // odwrócenie góra-dó³
            );
            

            SimulateInputOnCanvas(virtualScreenPos);
        }
    }

    private void SimulateInputOnCanvas(Vector2 screenPos)
    {
        if (EventSystem.current == null)
        {
            
            return;
        }

        pointerData = new PointerEventData(EventSystem.current)
        {
            position = screenPos
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        

        if (results.Count > 0)
        {
            // Szukamy w trafionym obiekcie lub jego rodzicach czegoœ, co faktycznie reaguje na klikniêcie (np. Button)
            GameObject clickHandler = null;

            for (int i = 0; i < results.Count; i++)
            {
                clickHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(results[i].gameObject);
                if (clickHandler != null)
                {
                    
                    break;
                }
            }

            // Jeœli znaleziono przycisk – kliknij go. Jeœli nie, kliknij w wierzchni element.
            GameObject target = clickHandler != null ? clickHandler : results[0].gameObject;

            if (Input.GetMouseButtonDown(0))
            {
                ExecuteEvents.Execute(target, pointerData, ExecuteEvents.pointerDownHandler);
                ExecuteEvents.Execute(target, pointerData, ExecuteEvents.pointerClickHandler);
                
            }
            else if (Input.GetMouseButtonUp(0))
            {
                ExecuteEvents.Execute(target, pointerData, ExecuteEvents.pointerUpHandler);
            }
        }
    }
}