using UnityEngine;
using UnityEngine.UI;

public class InterrogationManager : MonoBehaviour
{
    
    public float cameraSize = 120f;
    public Image suspectPng;
    [HideInInspector] public Suspect interrogatedSuspect;
    public static InterrogationManager Instance;
    
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    

    public void changeInterrogationSuspect(Suspect susp)
    {
        interrogatedSuspect = susp;
        suspectPng.sprite = susp.Face;
        DialogueOptionManager.Instance.dialoguesChange(false);

        // DialogueOptionManager.Instance.cleanDialogueOptions();
        UiDialougeManager.Instance.forceCleanChat();
<<<<<<< HEAD
        MonitorCameraTracker.Instance.changeCamera("bum", MonitorCameraTracker.Instance.inInterrogation);
=======
        // MonitorCameraTracker.Instance.changeCamera("bum", MonitorCameraTracker.Instance.inInterrogation);
>>>>>>> parent of c61b330 (merge)
    }
 
}
