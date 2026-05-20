using UnityEngine;
using UnityEngine.UI;

public class InterrogationManager : MonoBehaviour
{
    
    public float cameraSize = 140f;
    public Image suspectPng;
    public Suspect interrogatedSuspect;
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

    }
 
}
