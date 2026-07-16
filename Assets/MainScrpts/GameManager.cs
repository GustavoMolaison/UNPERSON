using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    [HideInInspector] public Level currentLevel;
 
    private float lastClickTime;
    [SerializeField] private float doubleTapDelay = 0.3f;
    private float lastClickTimeD;
    [SerializeField] private float doubleTapDelayD = 0.3f;

    public static GameManager Instance;

    void Awake()
    {
        Debug.Log("GameManager Awake() called");
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        // LevelsContentInfo.Instance.initilize();
        // MonitorCameraTracker.Instance.initilize();
        // SuspectTracker.instance.initilize();
    }
       
        void Start()
    {
        Debug.Log("GameManager Start() called");
        LevelsContentInfo.Instance.initilize();
        MonitorCameraTracker.Instance.initilize();
        SuspectTracker.instance.initilize();

    }

 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            CameraMover.Instance.changeCamera("B");
        }


        if (Input.GetKeyDown(KeyCode.D))
        {
            float timeSinceLastClickD = Time.time - lastClickTimeD;
            if (timeSinceLastClickD <= doubleTapDelay)
            {
                if(MonitorCameraTracker.Instance.inInterrogation.active)
                {
                    CameraMover.Instance.changeCamera("DD");
                }
 
                



                lastClickTime = 0f;
            }
            else
            {
                // To jest pierwszy klik, zapisujemy czas
                lastClickTimeD = Time.time;
                if (MonitorCameraTracker.Instance.currentCamera.isMonitor)
                {
                    CameraMover.Instance.changeCamera("D");
                }
            }

            
        }

        if (Input.GetKeyDown(KeyCode.A))
        {

            float timeSinceLastClick = Time.time - lastClickTime;

            if (timeSinceLastClick <= doubleTapDelay)
            {

                CameraMover.Instance.changeCamera("AA");
                
                
                
                lastClickTime = 0f; 
            }
            else
            {
                // To jest pierwszy klik, zapisujemy czas
                lastClickTime = Time.time;
                if (MonitorCameraTracker.Instance.currentCamera.isMonitor)
                {
                    CameraMover.Instance.changeCamera("A");
                }
            }

            

        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (MonitorCameraTracker.Instance.currentCamera.isMonitor)
            {
                CameraMover.Instance.changeCamera("S");

            }

        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (MonitorCameraTracker.Instance.currentCamera.isMonitor)
            {
                CameraMover.Instance.changeCamera("W");

            }

        }

        
    }

    

    
}
