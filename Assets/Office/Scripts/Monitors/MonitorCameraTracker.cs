using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonitorCameraTracker : MonoBehaviour
{ 

    
    public Dictionary< CameraData, Vector2> MonitorsCords;

    public Vector2 currentCords = new Vector2(0, 0);


    public static MonitorCameraTracker Instance;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public void Start()
    {
        if(GameManager.Instance.inMonitor1 == null)
        {
            Debug.Log("CWL");
        }
        MonitorsCords = new Dictionary<CameraData, Vector2> 
        {
            { GameManager.Instance.inMonitor1, new Vector2 (0, 0) },
            { GameManager.Instance.inMonitor2, new Vector2 (1, 0) }

        };

        if (MonitorsCords == null)
        {
            Debug.LogError("S�OWNIK JEST NULL! Zapomnia�e� go zainicjalizowa�.");
            return; // Przerwij, �eby nie wywali�o b��du
        }
        else
        {
            Debug.Log("What");
        }
    }

public CameraData monitorNavigate(CameraData currentCamera, string input)
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
 
}
