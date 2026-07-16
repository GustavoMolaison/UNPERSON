using UnityEngine;

public class CameraData 
{
   
    public bool active;
    public Vector3 pos;
    // public float size;
    // public Vector3 angle;

    public Vector2 sizeOfObject;

    public float distanceFromMonitor = 50;
    public bool isMonitor;

    

    public CameraData(bool active, Vector3 pos, bool isMonitor, Vector2 sizeOfObject, float distanceFromMonitor)
    {

        this.active = active;
        this.pos = pos;
        // this.size = size;
        // this.angle = angle;
        this.sizeOfObject = sizeOfObject;
        this.distanceFromMonitor = distanceFromMonitor;
        this.isMonitor = isMonitor;
        

    }
}
