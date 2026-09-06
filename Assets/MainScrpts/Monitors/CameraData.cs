using UnityEngine;

public class CameraData 
{
    public MonoBehaviour monitorScript;
    public bool active;
    public bool zoomed;
    
    // public float size;
    // public Vector3 angle;

    public Vector2 sizeOfObject;

    public float distanceFromMonitor = 50;
    public bool isMonitor;


    

    public CameraData(MonoBehaviour monitorScript, bool active, bool zoom, bool isMonitor, Vector2 sizeOfObject, float distanceFromMonitor)
    {

        this.monitorScript = monitorScript;
        this.active = active;
        this.zoomed = zoom;
        
        // this.size = size;
        // this.angle = angle;
        this.sizeOfObject = sizeOfObject;
        this.distanceFromMonitor = distanceFromMonitor;
        this.isMonitor = isMonitor;
        

    }
}
