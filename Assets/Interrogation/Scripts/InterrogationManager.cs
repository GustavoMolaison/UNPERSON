using UnityEngine;
using UnityEngine.UI;
using System;

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
        OnInterrogatedSuspectChanged += DialogueOptionManager.Instance.initilalizeSuspectOptions;
        OnInterrogatedSuspectChanged += UiDialougeManager.Instance.forceClean;
    }
    

    public void changeInterrogationSuspect(Suspect susp)
    {
        interrogatedSuspect = susp;
        suspectPng.sprite = susp.Face_interrogation;
        OnInterrogatedSuspectChanged?.Invoke();
        OnInterrogatedSuspectChangedInputSuspect?.Invoke(susp);
         CameraMover.Instance.changeCamera("bum", MonitorCameraTracker.Instance.inInterrogation);
    }
 
}
