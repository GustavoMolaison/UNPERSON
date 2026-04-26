using UnityEngine;

public class CameraData 
{
   
    public bool active;
    public Vector3 pos;
    public float size;
    public Vector3 angle;
    public bool isMonitor;

    public CameraData(bool active, Vector3 pos, float size, Vector3 angle, bool isMonitor)
    {
        this.active = active;
        this.pos = pos;
        this.size = size;
        this.angle = angle;
        this.isMonitor = isMonitor;

    }
}
