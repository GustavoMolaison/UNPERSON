using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public class CameraData
    {
        public bool active;
        public Vector3 pos;
        public float size;

        public CameraData(bool active, Vector3 pos, float size)
        {
            this.active = active;
            this.pos = pos;
            this.size = size;
        }
    }

    public CameraData inBaseView;
    public CameraData inMonitor1;
    public CameraData prevLoc;
    public List<CameraData> CDList;

    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
       
        void Start()
    {
        inBaseView = new CameraData(true, new Vector3(0, 0 ,0), 2f);
        inMonitor1 = new CameraData(false, Screen1.Instance.transform.position, Screen1.Instance.cameraSize);
        prevLoc = inBaseView;
        CDList = new List<CameraData> { inBaseView, inMonitor1 };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void whereIsPlayer()
    {
        CameraData newPrevLoc = null;
        int boolSum = CDList.Count(x => x.active);
        if(boolSum > 1)
        {
            foreach (CameraData loc in CDList)
            {
                if (loc.active && loc == prevLoc) 
                {
                    loc.active = false;
                }

                if (loc.active && loc != prevLoc)
                {
                    newPrevLoc = loc;
                }

            }

            prevLoc = newPrevLoc;

        }
        if(boolSum == 0)
        {
            inBaseView.active = true;
        }

        if(prevLoc == null)
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }    

    public void changeCamera(CameraData camera)
    {
        camera.active = true;
        whereIsPlayer();
        CameraMover.Instance.mover(camera);
    }
}
