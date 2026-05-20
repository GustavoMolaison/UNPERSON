using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    

    public CameraData inBaseCamera;
    public CameraData inMonitor1;
    public CameraData inMonitor2;
    public CameraData inInterrogation;
    public CameraData prevCamera;
    public CameraData currentCamera;
    public List<CameraData> CDList;
    public Dictionary<MonitorBase, CameraData> screenToCameraData = new Dictionary<MonitorBase, CameraData>();


    [Header("Camera data")]
    [SerializeField] private Vector3 monitor2Angle;

    private float lastClickTime;
    [SerializeField] private float doubleTapDelay = 0.3f;
    private float lastClickTimeD;
    [SerializeField] private float doubleTapDelayD = 0.3f;

    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
       
        void Start()
    {
        inBaseCamera = new CameraData(true, new Vector3(0, 0 ,-1), 500f, new Vector3(0, 0, 0), false);
        inMonitor1 = new CameraData(false, Screen1.Instance.transform.position, Screen1.Instance.cameraSize, new Vector3(0, 0, 0), true);
        //inMonitor2 = new CameraData(false, Screen1.Instance.transform.position, Screen1.Instance.cameraSize, monitor2Angle);
        
        inMonitor2 = new CameraData(false, Screen2.Instance.transform.position, Screen1.Instance.cameraSize, new Vector3(0, 0, 0), true);
        inInterrogation = new CameraData(false, InterrogationManager.Instance.transform.position, InterrogationManager.Instance.cameraSize, new Vector3(0, 0, 0), false);
        prevCamera = inBaseCamera;
        currentCamera = inBaseCamera;
        CDList = new List<CameraData> { inBaseCamera, inMonitor1, inMonitor2, inInterrogation };

        screenToCameraData.Add(Screen1.Instance, inMonitor1);
        screenToCameraData.Add(Screen2.Instance, inMonitor2);
        // screenToCameraData.Add(InterrogationManager.Instance, inInterrogation);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            changeCamera(inBaseCamera);
        }


        if (Input.GetKeyDown(KeyCode.D))
        {
            float timeSinceLastClickD = Time.time - lastClickTimeD;
            if (timeSinceLastClickD <= doubleTapDelay)
            {
                if(inInterrogation.active)
                {
                    changeCamera(MonitorCameraTracker.Instance.getCurrentMonitor());
                }
 
                



                lastClickTime = 0f;
            }
            else
            {
                // To jest pierwszy klik, zapisujemy czas
                lastClickTimeD = Time.time;
                if (currentCamera.isMonitor)
                {
                    changeCamera(MonitorCameraTracker.Instance.monitorNavigate(currentCamera, "D"));
                }
            }

            //if (currentCamera.isMonitor)
            //{
                
            //    changeCamera(MonitorCameraTracker.Instance.monitorNavigate(currentCamera, "D"));
            //}
            
        }

        if (Input.GetKeyDown(KeyCode.A))
        {

            float timeSinceLastClick = Time.time - lastClickTime;

            if (timeSinceLastClick <= doubleTapDelay)
            {
                
                changeCamera(inInterrogation);
                
                
                
                lastClickTime = 0f; 
            }
            else
            {
                // To jest pierwszy klik, zapisujemy czas
                lastClickTime = Time.time;
                if (currentCamera.isMonitor)
                {
                    changeCamera(MonitorCameraTracker.Instance.monitorNavigate(currentCamera, "A"));
                }
            }

            

        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (currentCamera.isMonitor)
            {
                changeCamera(MonitorCameraTracker.Instance.monitorNavigate(currentCamera, "S"));
                
            }

        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (currentCamera.isMonitor)
            {
                changeCamera(MonitorCameraTracker.Instance.monitorNavigate(currentCamera, "W"));
                
            }

        }

        // Debug.Log($"inBaseCamera: {inBaseCamera.active}, inMonitor1: {inMonitor1.active}, prevCamera: {(prevCamera != null ? prevCamera.active.ToString() : "null")}, currentCamera: {(currentCamera != null ? currentCamera.active.ToString() : "null")}");
    }

    

    void whereIsPlayer()
{
    // Znajdź wszystkie obecnie aktywne kamery poza tą, która była poprzednio
    var activeCameras = CDList.Where(x => x.active && x != currentCamera).ToList();
    

    if (activeCameras.Count > 0)
    {
        prevCamera = currentCamera; // Zaktualizuj poprzednią kamerę na aktualną przed zmianą
        prevCamera.active = false; // Dezaktywuj poprzednią kamerę

        // Nową kamerą zostaje pierwsza znaleziona, która nie jest starą kamerą
        CameraData targetCamera = activeCameras[0];

        // Wyłączamy wszystko inne
        foreach (var cam in CDList)
        {
            cam.active = (cam == targetCamera);
        }
        
        currentCamera = targetCamera;
    }
    else if (CDList.Count(x => x.active) == 0)
    {
        Debug.Log("passing Camera!");
        // inBaseCamera.active = true;
        // prevCamera = inBaseCamera; // Nie zapomnij zaktualizować prevCamera!
        // currentCamera = inBaseCamera;
    }

    // Krytyczne sprawdzenie
    if (prevCamera == null)
    {
        Debug.LogError("Brak aktywnej kamery! Zamykanie edytora.");
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    // Debug.Log($"inBaseCamera: {inBaseCamera.active}, inMonitor1: {inMonitor1.active}, prevCamera: {(prevCamera != null ? prevCamera.active.ToString() : "null")}, currentCamera: {(currentCamera != null ? currentCamera.active.ToString() : "null")}");
}

    public void changeCamera(CameraData camera)
    {
        if(camera != null)
        {
           camera.active = true;
           whereIsPlayer();
        
           CameraMover.Instance.mover(camera);
    
        }
        else
        {
            Debug.LogWarning("Próbowano zmienić na nieistniejącą kamerę (null)!");
        }
    }
}
