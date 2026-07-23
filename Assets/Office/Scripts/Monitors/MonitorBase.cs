using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;
using System.Linq;
[RequireComponent(typeof(Canvas))]
public class MonitorBase : MonoBehaviour
{
    [HideInInspector] public Canvas prevCanvas;

    [Header("Frist Canvas to show has to be first in this list")]
    [SerializeField] public List<Canvas> canvasList;

    [SerializeField]  public float cameraSize;
    
    
    public virtual void canvasChanger(Canvas targetCanvas)
{
    // 1. Bezpiecznie szukamy obecnie aktywnego canvasu
    Canvas currentActive = canvasList.FirstOrDefault(x => x != null && x.enabled);

    // 2. Jeśli coś znaleźliśmy, zapisujemy do prevCanvas i DEAKTYWUJEMY CAŁY GAMEOBJECT
    if (currentActive != null)
    {
        prevCanvas = currentActive;
        // prevCanvas.gameObject.SetActive(false);
        prevCanvas.enabled = false;
    }
    else
    {
        Debug.LogWarning("Brak aktywnego Canvasu na liście przed zmianą.");
    }

    // 3. Bezpiecznie włączamy nowy Canvas (cały GameObject)
    if (targetCanvas != null)
    {
        // targetCanvas.gameObject.SetActive(true);
        targetCanvas.enabled = true;
    }
    else
    {
        Debug.Log("Próbowano przełączyć na nieistniejący Canvas (null)!");
    }
}

// public virtual void canvasChanger(Canvas canvas)

//     {

//         Debug.Log(canvasList.Where(x => x.enabled).FirstOrDefault());

//         prevCanvas = canvasList.Where(x => x.enabled).FirstOrDefault();

//         prevCanvas.enabled = false;

//         //Debug.Log("wellnigga");



//         canvas.enabled = true;

//         //Debug.Log("wellnigga1");

//     }

    public virtual void OnMouseDown()
    {
        Debug.Log("Zmiana kamery");
        CameraMover.Instance.changeCamera("bum", MonitorCameraTracker.Instance.screenToCameraData[this]);
        // Setting current coordinates in MonitorCameraTracker to the coordinates of the current monitor
        MonitorCameraTracker.Instance.currentCords = MonitorCameraTracker.Instance.MonitorsCords[MonitorCameraTracker.Instance.screenToCameraData[this]];
    }




}
