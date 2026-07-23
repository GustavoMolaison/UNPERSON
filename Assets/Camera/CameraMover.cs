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
    float targetSize = 500f;
    Vector3 targetAngle;
    [SerializeField] private float smoothSpeed = 5f;


    public static CameraMover Instance; // Statyczna referencja do instancji

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        standardPosition = transform.position;
        targetPosition = transform.position;
    }

    private void Start()
    {
        cam = GetComponent<Camera>(); 
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, smoothSpeed * Time.deltaTime);
        Quaternion targetRotation = Quaternion.Euler(targetAngle);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);
        // Debug.Log(transform.position);
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
            Vector3 newPosition = new Vector3(camera.pos.x, camera.pos.y, camera.pos.z - camera.distanceFromMonitor);
            Vector3 distanceFromCurrentViewObject = (cam.transform.position - MonitorCameraTracker.Instance.prevCamera.pos);
            Vector3 distanceFromNewViewObject = (cam.transform.position - camera.pos);

            // targetPosition = newPosition;

            targetSize = ((camera.sizeOfObject[1] / 2) / distanceFromNewViewObject[2]) * 2;

            float angleBetweenVectors = Vector3.SignedAngle(distanceFromCurrentViewObject, distanceFromNewViewObject, Vector3.up);
            
            targetAngle = targetAngle + new Vector3(0, angleBetweenVectors, 0);
            

           
        }
    }

    public void changeCamera(string input, CameraData cameraPicked = null)
    {
        CameraData camera;
        // prevCamera = currentCamera; // Zaktualizuj poprzednią kamerę na aktualną przed zmianą
        // prevCamera.active = false;
        if (cameraPicked != null)
        {
            camera = cameraPicked;
        }
        else
        {
            camera = MonitorCameraTracker.Instance.monitorNavigate(input);
        }
            
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
