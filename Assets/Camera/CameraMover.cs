
using Unity.VisualScripting;
using UnityEngine;



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

    private float rotationRange = 1f;
    private float rotationProgress = 0f;

    [SerializeField] public bool isMoving = false;

    [Header("monitor1 camera adjustments")]
    [SerializeField] private Vector3 xyzAdjustments;
    [SerializeField] private float angleAdjustments;



    public static CameraMover Instance; // Statyczna referencja do instancji

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        cam = GetComponent<Camera>(); 
        standardPosition = transform.position;
        targetPosition = transform.position;

        
    }

    private void Start()
    {
        
        // TU SIE ZMIENIA POCZATKOWA KAMERE
        changeCamera("inMonitor1", MonitorCameraTracker.Instance.inMonitor1);

        
    }

    void Update()
    {
        // Jeśli stoimy w miejscu, nie rób nic
        if (!isMoving) return;

        // 1. Sprawdzenie rotacji
        float remainingAngle = Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle.y));

        if (rotationRange > 0.01f && remainingAngle > 0.1f)
        {
            rotationProgress = Mathf.Clamp01(remainingAngle / rotationRange);
        }
        else
        {
            rotationProgress = 0f;
        }

        float arc = Mathf.Sin(rotationProgress * Mathf.PI) * headArcAmount;
        float tilt = rotationProgress > 0.001f ? Mathf.Sin(rotationProgress * Mathf.PI) * 2f : 0f;

        Quaternion targetRotation = Quaternion.Euler(targetAngle.x, targetAngle.y, targetAngle.z + tilt);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);

        // 2. Pozycja
        if (updateCycleCounter > rotationOverMovingHeadStart)
        {
            Vector3 desiredPosition = targetPosition - (transform.forward * arc);
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref positionVelocity, smoothTime);
        }

        // 3. Zoom
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, smoothSpeed * Time.deltaTime);

        updateCycleCounter++;

        // --- ZATRZASKIWANIE I STOP ---
        float distToTarget = Vector3.Distance(transform.position, targetPosition);
        float angleToTarget = Quaternion.Angle(transform.rotation, Quaternion.Euler(targetAngle));
        float sizeDiff = Mathf.Abs(cam.orthographicSize - targetSize);

        // Progi tolerancji (dostosuj jeśli potrzebujesz większej/mniejszej precyzji)
        if (distToTarget < 0.01f && angleToTarget < 0.1f && sizeDiff < 0.01f)
        {
            transform.position = targetPosition;
            transform.rotation = Quaternion.Euler(targetAngle);
            cam.orthographicSize = targetSize;
            positionVelocity = Vector3.zero;

            isMoving = false;
            Debug.Log("KONIEC");
        }
    }

    public void backToStandardPos()
    {
        targetPosition = standardPosition;
    }

    /// <summary>
    /// Przesuwa obiekt p�ynnie do wskazanej pozycji, wsp�rz�dna z bedzie zawsze na sztywno r�wna -1.
    /// </summary>
    
   

    private void mover(CameraData camera)
    { 
    // NIE UZYWAJ TEJ FUNKCJI POZA TYM PLIKIEM
      if(camera.active == true)
        {
            bool hasCanvas = camera.monitorScript.TryGetComponent<Canvas>(out Canvas canvas);



            if (targetAngle != camera.monitorScript.transform.eulerAngles)
            {
                // Wydajemy dzwiek gdy obracamy fotel damn
                GetComponent<RandomAudioPlayer>().PlayRandomCreak();
            }

            targetAngle = camera.monitorScript.transform.eulerAngles;

            // targetSize = ((camera.sizeOfObject[1] / 2) / distanceFromNewViewObject[2]) * 2;

            if (camera.zoomed)
            {
                
                if (hasCanvas)
                {
                    targetPosition = camera.monitorScript.transform.position - (camera.monitorScript.transform.forward * camera.distanceFromMonitor);
                }
                else
                {
                    targetPosition = camera.monitorScript.transform.position + (camera.monitorScript.transform.forward * camera.distanceFromMonitor);
                    targetPosition.x -= xyzAdjustments[0];
                    targetPosition.y += xyzAdjustments[1];
                }
                    
                
            }
            else
            {
                targetPosition = standardPosition;
            }

            //rotationRange = Mathf.Abs(cam.transform.rotation.eulerAngles.y - targetAngle.y);

            if (!hasCanvas)
            {
                targetAngle.y += angleAdjustments;
            }

            Quaternion targetRotation = Quaternion.Euler(targetAngle);
            rotationRange = Quaternion.Angle(cam.transform.rotation, targetRotation);
            isMoving = true;
            MouseLook.Instance.mouseMovementAllowed = false;




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
