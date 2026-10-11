using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerInspector : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current == null) return;

            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            Debug.Log($"--- KLIKNIÊCIE W EKRAN ({results.Count} trafieñ) ---");

            for (int i = 0; i < results.Count; i++)
            {
                // Obiekt [0] to TEN, KTÓRY ZJADA KLIKNIÊCIE I BLOKUJE WSZYSTKO POD NIM!
                string status = (i == 0) ? ">>> BLOKER (WIE¯CH) <<<" : $"Warstwa pod spodem #{i}";
                Debug.Log($"{status}: <b>{results[i].gameObject.name}</b> (Komponent: {results[i].module.GetType().Name}, Depth: {results[i].depth})", results[i].gameObject);
            }
        }
    }
}