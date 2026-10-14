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
    public CameraData inInterrogationGlass;
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
        // Kadr (pozycja, kąt, odległość) liczy CameraData.TryGetViewPose z aktualnego transformu obiektu,
        // więc przesunięcie monitora/szyby/canvasu na scenie nie wymaga zmian w kodzie.
        // Działa dla canvasów (RectTransform) i obiektów 3D z Rendererami.
        // Ekran modelu monitora patrzy w +Z, szyba - strona wykrywana automatycznie (od biurka).
        // Ustawienia per obiekt: komponent CameraFocusTarget.
        inMonitor1 = CreateCameraData(Monitor1.Instance, nameof(Monitor1), CameraFocusTarget.Facing.Forward);
        inInterrogation = CreateCameraData(InterrogationManager.Instance, nameof(InterrogationManager));
        inInterrogationGlass = CreateCameraData(InterrogationGlass.Instance, nameof(InterrogationGlass));

        // Na starcie aktywna jest tylko kamera przesłuchania
        prevCamera = inInterrogation;
        currentCamera = inInterrogation;
        if (inInterrogation != null) inInterrogation.active = true;

        CDList = new List<CameraData>();
        MonitorsCords = new Dictionary<CameraData, Vector2>();
        Register(inInterrogation, new Vector2(-1, 0));
        Register(inMonitor1, new Vector2(0, 0));
        Register(inInterrogationGlass, new Vector2(1, 0));

        initilized = true;
    }

    private CameraData CreateCameraData(MonoBehaviour target, string targetName, CameraFocusTarget.Facing facing = CameraFocusTarget.Facing.Auto)
    {
        if (target == null)
        {
            Debug.LogError($"MonitorCameraTracker: brak instancji {targetName} na scenie - pomijam tę kamerę.");
            return null;
        }
        return new CameraData(target, false, true, true, 0f, facing);
    }

    private void Register(CameraData camera, Vector2 cords)
    {
        if (camera == null) return;
        CDList.Add(camera);
        MonitorsCords.Add(camera, cords);
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
            if (prevCamera != null) prevCamera.active = false; // Dezaktywuj poprzednią kamerę

            // Nową kamerą zostaje pierwsza znaleziona, która nie jest starą kamerą
            CameraData targetCamera = activeCameras[0];

            // Wyłączamy wszystko inne
            foreach (var cam in CDList)
            {
                cam.active = (cam == targetCamera);
            }

            currentCamera = targetCamera;
            Debug.Log($"Zmiana kamery na: {currentCamera.monitorScript.name}, poprzednia kamera: {prevCamera?.monitorScript.name}");
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
