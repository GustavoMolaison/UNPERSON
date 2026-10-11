using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InterrogationManager : MonoBehaviour
{
    
    public float cameraSize = 120f;
    public Image suspectPng;
    [HideInInspector] public Suspect interrogatedSuspect;

    public event Action<Suspect> OnInterrogatedSuspectChangedInputSuspect;

    public event Action OnInterrogatedSuspectChanged;
    public static InterrogationManager Instance;
    
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
    }

    private void Start()
    {
        OnInterrogatedSuspectChanged += DialogueOptionManager.Instance.initilalizeSuspectOptions;
        OnInterrogatedSuspectChanged += UiDialougeManager.Instance.forceClean;
        changeInterrogationSuspect(SuspectTracker.instance.currentSuspect);
    }


    public void changeInterrogationSuspect(Suspect susp)
    {
        interrogatedSuspect = susp;
        suspectPng.sprite = susp.Face_interrogation;
        OnInterrogatedSuspectChanged?.Invoke();
        OnInterrogatedSuspectChangedInputSuspect?.Invoke(susp);
         CameraMover.Instance.changeCamera("bum", MonitorCameraTracker.Instance.inInterrogation);
    }

    private void Update()
    {
        // Debugowanie klikniêæ w Canvas przes³uchañ
        if (Input.GetMouseButtonDown(0))
        {
            LogInterrogationClicks();
        }
    }

    private void LogInterrogationClicks()
    {
        if (EventSystem.current == null) return;

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        bool hitAnythingInThisCanvas = false;

        foreach (var result in results)
        {
            // Sprawdzamy, czy trafiony element UI nale¿y do tego obiektu lub jego dzieci/pod-canvasów
            if (result.gameObject.transform.IsChildOf(this.transform))
            {
                hitAnythingInThisCanvas = true;
                string hierarchyPath = GetHierarchyPath(result.gameObject);
                Debug.Log($"<color=cyan>[INTERROGATION HIT]</color> Trafiono: <b>{result.gameObject.name}</b> | Œcie¿ka: {hierarchyPath} | G³êbokoœæ UI (depth): {result.depth}");
            }
        }

        if (!hitAnythingInThisCanvas && results.Count > 0)
        {
            Debug.LogWarning($"<color=yellow>[UI OUTSIDE INTERROGATION]</color> Klikniêto w UI poza przes³uchaniem: <b>{results[0].gameObject.name}</b>");
        }
        else if (results.Count == 0)
        {
            Debug.Log("<color=grey>[NO UI HIT]</color> Klikniêcie nie trafi³o w ¿aden element Canvasu (czysta przestrzeñ 3D).");
        }
    }

    // Pomocnicza metoda do wypisania dok³adnej œcie¿ki w hierarchii
    private string GetHierarchyPath(GameObject obj)
    {
        string path = obj.name;
        Transform current = obj.transform.parent;
        while (current != null && current != this.transform)
        {
            path = current.name + "/" + path;
            current = current.parent;
        }
        return path;
    }

}
