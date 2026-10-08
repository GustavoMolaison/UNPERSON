
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
        // Bezpieczne liczenie pozostałego kąta (uwzględnia przeskok 0-360)
        float remainingAngle = Mathf.Abs(Mathf.DeltaAngle(cam.transform.rotation.eulerAngles.y, targetAngle.y));

        // Jeśli kamera jest już praktycznie u celu, odetnij efekt łuku i przechyłu
        if (rotationRange > 0.01f && remainingAngle > 0.1f)
        {
            // Clampujemy progress ściśle do przedziału 0..1
            rotationProgress = Mathf.Clamp01(remainingAngle / rotationRange);
        }
        else
        {
            rotationProgress = 0f;
        }

        float arc = Mathf.Sin(rotationProgress * Mathf.PI) * headArcAmount;

        // Tilt aplikujemy tylko w trakcie realnego ruchu, bez losowania w stanie spoczynku
        float tilt = rotationProgress > 0.001f ? Mathf.Sin(rotationProgress * Mathf.PI) * 2f : 0f;

        Quaternion targetRotation = Quaternion.Euler(targetAngle.x, targetAngle.y, targetAngle.z + tilt);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);

        if (updateCycleCounter > rotationOverMovingHeadStart)
        {
            // SmoothDamp dąży do pozycji z nałożonym łukiem, zamiast odejmować go w nieskończoność
            Vector3 desiredPosition = targetPosition - (transform.forward * arc);
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref positionVelocity, smoothTime);
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
                    targetPosition.x -= 100f;
                    targetPosition.y += 50f;
                }
                    
                
            }
            else
            {
                targetPosition = standardPosition;
            }

            //rotationRange = Mathf.Abs(cam.transform.rotation.eulerAngles.y - targetAngle.y);

            if (!hasCanvas)
            {
                targetAngle.y += 180f;
            }

            Quaternion targetRotation = Quaternion.Euler(targetAngle);
            rotationRange = Quaternion.Angle(cam.transform.rotation, targetRotation);




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
