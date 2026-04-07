using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering;

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

    public CameraData inBaseCamera;
    public CameraData inMonitor1;
    public CameraData prevCamera;

    public CameraData currentCamera;
    public List<CameraData> CDList;

    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
       
        void Start()
    {
        inBaseCamera = new CameraData(true, new Vector3(0, 0 ,0), 2f);
        inMonitor1 = new CameraData(false, Screen1.Instance.transform.position, Screen1.Instance.cameraSize);
        prevCamera = inBaseCamera;
        currentCamera = inBaseCamera;
        CDList = new List<CameraData> { inBaseCamera, inMonitor1 };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if(inBaseCamera == prevCamera)
        {
            Debug.Log("xd hell yeah");
        }
            changeCamera(prevCamera);
        }

        // Debug.Log($"inBaseCamera: {inBaseCamera.active}, inMonitor1: {inMonitor1.active}, prevCamera: {(prevCamera != null ? prevCamera.active.ToString() : "null")}, currentCamera: {(currentCamera != null ? currentCamera.active.ToString() : "null")}");
    }

    

    void whereIsPlayer()
{
    // Znajdź wszystkie obecnie aktywne kamery poza tą, która była poprzednio
    var activeCameras = CDList.Where(x => x.active && x != currentCamera).ToList();
    

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

    Debug.Log($"inBaseCamera: {inBaseCamera.active}, inMonitor1: {inMonitor1.active}, prevCamera: {(prevCamera != null ? prevCamera.active.ToString() : "null")}, currentCamera: {(currentCamera != null ? currentCamera.active.ToString() : "null")}");
}

    public void changeCamera(CameraData camera)
    {
        camera.active = true;
        whereIsPlayer();
        if(inBaseCamera == prevCamera)
        {
            Debug.Log("xd hell yeah222");
        }
        CameraMover.Instance.mover(camera);
    }
}
