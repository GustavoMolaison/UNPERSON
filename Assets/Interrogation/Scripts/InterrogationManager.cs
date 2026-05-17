using UnityEngine;
using UnityEngine.UI;

public class InterrogationManager : MonoBehaviour
{
    
    public float cameraSize = 140f;
    public Image SuspectPng;
    public static InterrogationManager Instance;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    

    public void changeSuspectPng()
    {
        SuspectPng.sprite = SuspectTracker.instance.currentSuspect.Face;
    }
}
