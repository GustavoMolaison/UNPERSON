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
    
    
    public virtual void screenChanger(Canvas screen)
    {
        prevCanvas = canvasList.Where(x => x.enabled).FirstOrDefault();
        prevCanvas.enabled = false;

        screen.enabled = true;
    }

    
}
