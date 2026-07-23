using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class MonitorCameraTracker : MonoBehaviour
{
    // public CameraData inBaseCamera;
    public CameraData inMonitor1;
    public CameraData inCaseMonitor;
    public CameraData inMonitor2;
    public CameraData inInterrogation;
    public CameraData prevCamera;
    public CameraData currentCamera;
    public List<CameraData> CDList;
    public Dictionary<MonitorBase, CameraData> screenToCameraData = new Dictionary<MonitorBase, CameraData>();

    [Header("Camera data")]
    [SerializeField] private Vector3 monitor2Angle;

    public Dictionary< CameraData, Vector2> MonitorsCords;

    public Vector2 currentCords = new Vector2(-1, 0);

    public const int distanceFromMonitor = 50;


    public static MonitorCameraTracker Instance;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    //public void Start()
    //{

    //    MonitorsCords = new Dictionary<CameraData, Vector2> 
    //    {
    //        { GameManager.Instance.inMonitor1, new Vector2 (0, 0) },
    //        { GameManager.Instance.inMonitor2, new Vector2 (1, 0) },
    //        { GameManager.Instance.inCaseMonitor, new Vector2 (-1, 0) },


    //    };


    //}

    
    private bool initilized = false;
    public void initilize()
    {
        // if (Instance == null) Instance = this;
        // else Destroy(gameObject);

        if(Screen1.Instance == null || Screen2.Instance == null || Case_Monitor.Instance == null || InterrogationManager.Instance == null)
        {
            Debug.LogError("Nie wszystkie instancje monitorów zostały zainicjalizowane!");
            return;
        }
        // inBaseCamera = new CameraData(true, new Vector3(0, 0, -1), 500f, new Vector3(0, 0, 0), false);

        RectTransform screenRect = (RectTransform)Screen1.Instance.transform;
        Vector2 screenBounds = new Vector2(screenRect.rect.width, screenRect.rect.height);
        inMonitor1 = new CameraData(false, Screen1.Instance.transform.position, true, screenBounds, distanceFromMonitor);

        screenRect = (RectTransform)Case_Monitor.Instance.transform;
        screenBounds = new Vector2(screenRect.rect.width, screenRect.rect.height);
        inCaseMonitor = new CameraData(false, Case_Monitor.Instance.transform.position, true, screenBounds, 400);
        //inMonitor2 = new CameraData(false, Screen1.Instance.transform.position, Screen1.Instance.cameraSize, monitor2Angle);
        
        screenRect = (RectTransform)Screen2.Instance.transform;
        screenBounds = new Vector2(screenRect.rect.width, screenRect.rect.height);
        inMonitor2 = new CameraData(false, Screen2.Instance.transform.position, true, screenBounds, distanceFromMonitor);

        screenRect = (RectTransform)InterrogationManager.Instance.transform;
        screenBounds = new Vector2(screenRect.rect.width, screenRect.rect.height);
        inInterrogation = new CameraData(true, InterrogationManager.Instance.transform.position, true, screenBounds, distanceFromMonitor);
        
        prevCamera = inInterrogation;
        currentCamera = inInterrogation;
        CDList = new List<CameraData> { inMonitor1, inMonitor2, inInterrogation, inCaseMonitor };

        screenToCameraData.Add(Screen1.Instance, inMonitor1);
        screenToCameraData.Add(Screen2.Instance, inMonitor2);
        screenToCameraData.Add(Case_Monitor.Instance, inCaseMonitor);

        MonitorsCords = new Dictionary<CameraData, Vector2>
        {
            { inMonitor1, new Vector2 (0, 0) },
            { inMonitor2, new Vector2 (1, 0) },
            { inInterrogation, new Vector2 (-1, 0) },


        };
        

        CameraMover.Instance.changeCamera("Ininterrogation", MonitorCameraTracker.Instance.inInterrogation);

        initilized = true;


    }

    // public void changeCamera(string input, CameraData cameraPicked = null)
    // {
    //     CameraData camera;
    //     if (cameraPicked != null)
    //     {
    //         camera = cameraPicked;
    //     }
    //     else
    //     {
    //         camera = monitorNavigate(input);
    //     }
            
    //     if (camera != null)
    //     {
    //         camera.active = true;
    //         whereIsPlayer();

    //         CameraMover.Instance.mover(camera);

    //     }
    //     else
    //     {
    //         Debug.LogWarning("Próbowano zmienić na nieistniejącą kamerę (null)!");
    //     }
    // }

    public void whereIsPlayer()
    {
        // Znajdź wszystkie obecnie aktywne kamery poza tą, która była poprzednio
        var activeCameras = CDList.Where(x => x.active && x != MonitorCameraTracker.Instance.currentCamera).ToList();


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
            Debug.Log($"Zmiana kamery na: {currentCamera.pos}, poprzednia kamera: {prevCamera.pos}");
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
    }

    public CameraData monitorNavigate(string input)
    {
        Vector2 CordSave = currentCords;
        if(input == "D")
        {
            currentCords.x += 1;
        }
        if (input == "A")
        {
            currentCords.x += -1;
        }
        if (input == "W")
        {
            currentCords.y += 1;
        }
        if (input == "S")
        {
            currentCords.y += -1;
        }
        if (input == "B")
        {
            return inInterrogation;
        }
        if (input == "DD")
        {
            return getCurrentMonitor();
        }
        if (input == "AA")
        {
            if(currentCords == new Vector2(-1, 0) && inInterrogation.active == true)
            {
                return null;
            }
            else
            {
                currentCords = new Vector2(-1, 0);
                return inInterrogation;
            }
            
        }

        foreach (var pair in MonitorsCords)
        {
            if (pair.Value == currentCords)
            {
                CameraData monitor =  pair.Key;
                return monitor;
            }
        }

        currentCords = CordSave;

        return null;
    }


    public CameraData getCurrentMonitor()
    {
        return MonitorsCords.FirstOrDefault(x => x.Value == currentCords).Key;
    
    }
}
