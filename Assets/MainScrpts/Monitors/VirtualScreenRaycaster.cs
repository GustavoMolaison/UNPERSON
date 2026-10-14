using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Klikanie w canvas, który jest renderowany do RenderTexture i wyświetlany na monitorze 3D.
// Skrypt ma siedzieć na obiekcie monitora (collider ekranu musi być MeshColliderem w nim albo w jego dzieciach).
public class VirtualScreenRaycaster : MonoBehaviour
{
    [Header("Główna kamera i monitor")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask screenLayer; // warstwa monitora 3D

    [Header("Wirtualny Canvas i kamera")]
    [SerializeField] private RenderTexture renderTexture; // Przeciągnij tu swój 'New Render Texture'
    [SerializeField] private Canvas virtualCanvas;
    [Tooltip("Kamera, która renderuje canvas do RenderTexture. Puste = znajdzie się sama po targetTexture")]
    [SerializeField] private Camera renderCamera;
    [Tooltip("Odwrócenie góra-dół UV (model monitora ma odwrócone UV ekranu)")]
    [SerializeField] private bool flipY = true;

    private readonly List<RaycastResult> results = new List<RaycastResult>();

    void Start()
    {
        if (renderCamera == null) renderCamera = FindRenderCamera();
        if (renderCamera == null)
            Debug.LogWarning($"VirtualScreenRaycaster ({name}): nie znaleziono kamery renderującej do {renderTexture?.name}.");
    }

    void Update()
    {
        // Sprawdzamy kliknięcie myszy
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
        {
            HandleScreenClick();
        }
    }

    private Camera FindRenderCamera()
    {
        foreach (Camera c in FindObjectsByType<Camera>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            if (c.targetTexture == renderTexture) return c;
        return null;
    }

    private void HandleScreenClick()
    {
        if (renderCamera == null || virtualCanvas == null) return;

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        // Zasięg = zasięg kamery (scena jest w dużej skali, stałe 1000 nie sięgało np. z kamery na szybę)
        if (!Physics.Raycast(ray, out RaycastHit hit, playerCamera.farClipPlane, screenLayer)) return;

        // Każdy monitor ma swój raycaster - obsługujemy tylko trafienia w nasz monitor
        if (!hit.collider.transform.IsChildOf(transform)) return;

        Vector2 uv = hit.textureCoord;
        if (flipY) uv.y = 1f - uv.y;

        // Pozycja w pikselach kamery renderującej (= piksele RenderTexture)
        Vector2 virtualScreenPos = new Vector2(uv.x * renderCamera.pixelWidth, uv.y * renderCamera.pixelHeight);

        SimulateInputOnCanvas(virtualScreenPos);
    }

    private void SimulateInputOnCanvas(Vector2 screenPos)
    {
        if (EventSystem.current == null) return;

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = screenPos
        };

        RaycastVirtualCanvas(pointerData);

        if (results.Count == 0) return;

        // Szukamy w trafionym obiekcie lub jego rodzicach czegoś, co faktycznie reaguje na kliknięcie (np. Button)
        GameObject clickHandler = null;
        for (int i = 0; i < results.Count; i++)
        {
            clickHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(results[i].gameObject);
            if (clickHandler != null) break;
        }

        // Jeśli znaleziono przycisk – kliknij go. Jeśli nie, kliknij w wierzchni element.
        GameObject target = clickHandler != null ? clickHandler : results[0].gameObject;
        pointerData.pointerCurrentRaycast = results[0];
        pointerData.pointerPressRaycast = results[0];

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

    // Raycast tylko w canvas tego monitora (a nie we wszystkie canvasy na scenie),
    // licząc promień z kamery renderującej do RenderTexture
    private void RaycastVirtualCanvas(PointerEventData pointerData)
    {
        results.Clear();

        Canvas root = virtualCanvas.rootCanvas;

        // Canvas World Space ma jako event camera np. Main Camera - na czas raycastu podmieniamy ją na kamerę RenderTexture.
        // (Screen Space - Camera już ma kamerę renderującą, a jej podmiana przesunęłaby canvas)
        Camera previousCamera = root.worldCamera;
        bool swapCamera = root.renderMode == RenderMode.WorldSpace && previousCamera != renderCamera;
        if (swapCamera) root.worldCamera = renderCamera;

        VirtualScreenRaycasterOnly.AllowVirtualRaycast = true;
        foreach (GraphicRaycaster raycaster in root.GetComponentsInChildren<GraphicRaycaster>())
        {
            if (raycaster.isActiveAndEnabled) raycaster.Raycast(pointerData, results);
        }
        VirtualScreenRaycasterOnly.AllowVirtualRaycast = false;

        if (swapCamera) root.worldCamera = previousCamera;

        // Wierzchni element pierwszy (jak w EventSystem)
        results.Sort((a, b) =>
        {
            if (a.sortingLayer != b.sortingLayer)
                return SortingLayer.GetLayerValueFromID(b.sortingLayer).CompareTo(SortingLayer.GetLayerValueFromID(a.sortingLayer));
            if (a.sortingOrder != b.sortingOrder) return b.sortingOrder.CompareTo(a.sortingOrder);
            if (a.depth != b.depth) return b.depth.CompareTo(a.depth);
            return a.distance.CompareTo(b.distance);
        });
    }
}
