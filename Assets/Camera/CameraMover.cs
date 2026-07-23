using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UnityEditor.PlayerSettings;

public class CameraMover : MonoBehaviour
{

    Camera cam;
    
    
    Vector3 standardPosition;
    Vector3 targetPosition;
    Quaternion targetRotation;

    private Vector3 positionVelocity;
    private Vector3 rotationVelocity; // do wygładzania kątów
    public float smoothTime = 0.3f;
    float targetSize = 500f;
    Vector3 targetAngle;
    [SerializeField] private float smoothSpeed = 5f;

    [SerializeField] private int rotationOverMovingHeadStart = 2;
    private int updateCycleCounter = 0;

    [Header("Ruch Głowy Pod Kątem")]
    public float headArcAmount = 0.2f;

    private float rotationRange;
    private float rotationProgress = 0f;


    public static CameraMover Instance; // Statyczna referencja do instancji

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // standardPosition = transform.position;
        // targetPosition = transform.position;
    }

    private void Start()
    {
        cam = GetComponent<Camera>(); 
        standardPosition = transform.position;
        targetPosition = transform.position;
    }

    void Update()
    {
        // transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        // cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, smoothSpeed * Time.deltaTime);
        
        // Quaternion targetRotation = Quaternion.Euler(targetAngle);
        // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);


        rotationProgress = Mathf.Abs(cam.transform.rotation.eulerAngles.y - targetAngle.y) / rotationRange;
        float arc = Mathf.Sin(rotationProgress * Mathf.PI) * headArcAmount;
        // 1. Płynne przesunięcie z masą/bezwładnością
        float tilt = Mathf.Sin(rotationProgress * Mathf.PI) * Random.Range(0f, 4f);
        Quaternion targetRotation = Quaternion.Euler(targetAngle.x, targetAngle.y, targetAngle.z + tilt);
        // Quaternion targetRotation = Quaternion.Euler(targetAngle);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);

        if(updateCycleCounter > rotationOverMovingHeadStart)
        {
            // 2. Płynna rotacja z powolnym startem i wyhamowaniem
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref positionVelocity, smoothTime);
            transform.position -= transform.forward * arc;
        }
        
         
        // 3. Zoom
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, smoothSpeed * Time.deltaTime);


        updateCycleCounter++;
      
    }

    public void backToStandardPos()
    {
        targetPosition = standardPosition;
    }

    /// <summary>
    /// Przesuwa obiekt p�ynnie do wskazanej pozycji, wsp�rz�dna z b�dzie zawsze na sztywno r�wna -1.
    /// </summary>
    
   

    private void mover(CameraData camera)
    { 
    // NIE UZYWAJ TEJ FUNKCJI POZA TYM PLIKIEM
      if(camera.active == true)
        {

            
            // Vector3 distanceFromCurrentViewObject = (standardPosition - MonitorCameraTracker.Instance.prevCamera.pos);
            // Vector3 distanceFromCurrentViewObject = (standardPosition - MonitorCameraTracker.Instance.prevCamera.monitorScript.transform.position);
            // Vector3 distanceFromNewViewObject = (standardPosition - camera.monitorScript.transform.position);

            // float angleBetweenVectors = Vector3.SignedAngle(distanceFromCurrentViewObject, distanceFromNewViewObject, Vector3.up);           
            // targetAngle = targetAngle + new Vector3(0, angleBetweenVectors, 0);
            targetAngle = camera.monitorScript.transform.eulerAngles;

            // targetSize = ((camera.sizeOfObject[1] / 2) / distanceFromNewViewObject[2]) * 2;

            if (camera.zoomed)
            {
                // targetPosition = distanceFromNewViewObject - (camera.monitorScript.transform.forward * camera.distanceFromMonitor);
                targetPosition = camera.monitorScript.transform.position - (camera.monitorScript.transform.forward * camera.distanceFromMonitor);
                // targetPosition = camera.monitorScript.transform.forward
            }
            else
            {
                targetPosition = standardPosition;
            }

            rotationRange = Mathf.Abs(cam.transform.rotation.eulerAngles.y - targetAngle.y);
            
            

           
        }
    }

    public void changeCamera(string input, CameraData cameraPicked = null)
    {
        CameraData camera;
        camera = MonitorCameraTracker.Instance.monitorNavigate(input, cameraPicked);
        updateCycleCounter = 0;
        

        if (camera != null)
        {
            camera.active = true;
            MonitorCameraTracker.Instance.whereIsPlayer();

            mover(camera);

        }
        else
        {
            Debug.LogWarning("Próbowano zmienić na nieistniejącą kamerę (null)!");
        }
    }


}
