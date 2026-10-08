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

    // if equals to 0 it means we ignore distance we stay at basic camera position
    public const int distanceFromMonitor = 0;


    public static MonitorCameraTracker Instance;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
   

    
    private bool initilized = false;
    public void initilize()
    {


        if(Monitor1.Instance == null || EvidenceManager.Instance == null || InterrogationManager.Instance == null)
        {
            Debug.LogError("Nie wszystkie instancje monitorów zostały zainicjalizowane!");
            return;
        }

        RectTransform screenRect;
        //RectTransform screenRect = (RectTransform)Screen1.Instance.transform;
        //Vector2 screenBounds = new Vector2(screenRect.rect.width, screenRect.rect.height);
        //inMonitor1 = new CameraData(Screen1.Instance, true, true, true, screenBounds, 180);

        Renderer screenRenderer = Monitor1.Instance.GetComponentInChildren<Renderer>();
        // bounds.size zwraca wymiary prostopadłościanu w world space uwzględniające scale
        Vector3 size = screenRenderer.bounds.size;

        // Jeśli monitor stoi w osi X/Y lub Z/Y:
        // Dla kamery patrzącej wprost potrzebujesz szerokości i wysokości tafli ekranu:
        float width = size.x; // jeśli ekran jest obrócony bokiem (np. Y ok. -96 st.), może to być size.z
        float height = size.y;

        Vector2 screenBounds = new Vector2(width, height);
        inMonitor1 = new CameraData(Monitor1.Instance, true, true, true, screenBounds, 180);



        screenRect = (RectTransform)InterrogationManager.Instance.transform;
        screenBounds = new Vector2(screenRect.rect.width, screenRect.rect.height);
        inInterrogation = new CameraData(InterrogationManager.Instance, true, true, true, screenBounds, 500);
        
        prevCamera = inInterrogation;
        currentCamera = inInterrogation;
        CDList = new List<CameraData> { inMonitor1, inInterrogation};

        //screenToCameraData.Add(Screen1.Instance, inMonitor1);
        
        // screenToCameraData.Add(Case_Monitor.Instance, inCaseMonitor);

        MonitorsCords = new Dictionary<CameraData, Vector2>
        {
            { inMonitor1, new Vector2 (0, 0) },
            { inInterrogation, new Vector2 (-1, 0) },


        };
        

        // CameraMover.Instance.changeCamera("Ininterrogation", MonitorCameraTracker.Instance.inMonitor1);

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
            Debug.Log($"Zmiana kamery na: {currentCamera.monitorScript.name}, poprzednia kamera: {prevCamera.monitorScript.name}");
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

    public CameraData monitorNavigate(string input, CameraData camera = null)
    {

        // We pass the CameraData we only update cords
        if(camera != null)
        {
            foreach (var pair in MonitorsCords)
            {
                if (pair.Key == camera)
                {
                    currentCords = pair.Value;
                    break;
                }
                
               
            }
            Debug.Log("currentCords updated to: " + currentCords);
            return camera;
        }
        // We dont pass the CameraData we update cords and look for the camera in the dictionary
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
