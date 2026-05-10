using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;
using System.Linq;
[RequireComponent(typeof(Canvas))]
public class MonitorBase : MonoBehaviour
{
    [HideInInspector] public Canvas prevCanvas;

    [HideInInspector] public List<Canvas> canvasList;

    [SerializeField]  public float cameraSize;
    
    
    public virtual void canvasChanger(Canvas canvas)
    {
        Debug.Log(canvasList.Where(x => x.enabled).FirstOrDefault());
        prevCanvas = canvasList.Where(x => x.enabled).FirstOrDefault();
        prevCanvas.enabled = false;
        //Debug.Log("wellnigga");

        canvas.enabled = true;
        //Debug.Log("wellnigga1");
    }

    public virtual void OnMouseDown()
    {
        Debug.Log("Zmiana kamery");
        GameManager.Instance.changeCamera(GameManager.Instance.screenToCameraData[this]);
    }




}
