
using UnityEngine;



public class CameraMover : MonoBehaviour
{

    Camera cam;


    Vector3 standardPosition;
    Vector3 targetPosition;
    Quaternion targetRotation;

    private Vector3 positionVelocity;
    public float smoothTime = 0.3f;
    float targetSize;
    [SerializeField] private float smoothSpeed = 5f;

    [SerializeField] private int rotationOverMovingHeadStart = 2;
    private int updateCycleCounter = 0;

    [Header("Ruch Głowy Pod Kątem")]
    public float headArcAmount = 0.2f;

    private float rotationRange = 1f;
    private float rotationProgress = 0f;

    [SerializeField] public bool isMoving = false;

    // Kamera, na którą aktualnie patrzymy. Kadr liczymy z jej obiektu co klatkę,
    // więc przesunięcie monitora/szyby/canvasu na scenie od razu przesuwa też kamerę
    private CameraData currentTarget;



    public static CameraMover Instance; // Statyczna referencja do instancji

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        cam = GetComponent<Camera>();
        standardPosition = transform.position;
        targetPosition = transform.position;
        targetRotation = transform.rotation;
        targetSize = cam.orthographicSize;


    }

    private void Start()
    {

        // TU SIE ZMIENIA POCZATKOWA KAMERE
        changeCamera("inMonitor1", MonitorCameraTracker.Instance.inMonitor1);


    }

    void Update()
    {
        // Podczas chodzenia kamerą steruje PlayerController
        if (PlayerController.Instance != null && !PlayerController.Instance.InDesk) return;
        // Swobodne rozglądanie (spacja) - kamerą steruje MouseLook
        if (MouseLook.Instance != null && MouseLook.Instance.mouseMovementAllowed) return;

        RefreshTarget();

        // Stoimy w miejscu - ruszamy się tylko, jeśli obiekt, na który patrzymy, się przesunął
        if (!isMoving)
        {
            if (IsAtTarget()) return;
            isMoving = true;
        }

        // 1. Sprawdzenie rotacji
        float remainingAngle = Quaternion.Angle(transform.rotation, targetRotation);

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

        Quaternion tiltedRotation = targetRotation * Quaternion.Euler(0f, 0f, tilt);
        transform.rotation = Quaternion.Slerp(transform.rotation, tiltedRotation, smoothSpeed * Time.deltaTime);

        // 2. Pozycja
        if (updateCycleCounter > rotationOverMovingHeadStart)
        {
            Vector3 desiredPosition = targetPosition - (transform.forward * arc);
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref positionVelocity, smoothTime);
        }

        // 3. Zoom (tylko kamera ortograficzna - perspektywiczna "zoomuje" odległością)
        if (cam.orthographic)
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, smoothSpeed * Time.deltaTime);

        updateCycleCounter++;

        // --- ZATRZASKIWANIE I STOP ---
        if (IsAtTarget())
        {
            transform.position = targetPosition;
            transform.rotation = targetRotation;
            if (cam.orthographic) cam.orthographicSize = targetSize;
            positionVelocity = Vector3.zero;

            isMoving = false;
            Debug.Log("KONIEC");
        }
    }

    // Progi tolerancji (dostosuj jeśli potrzebujesz większej/mniejszej precyzji)
    private bool IsAtTarget()
    {
        float distToTarget = Vector3.Distance(transform.position, targetPosition);
        float angleToTarget = Quaternion.Angle(transform.rotation, targetRotation);
        float sizeDiff = cam.orthographic ? Mathf.Abs(cam.orthographicSize - targetSize) : 0f;

        return distToTarget < 0.01f && angleToTarget < 0.1f && sizeDiff < 0.01f;
    }

    // Przelicza kadr z aktualnej pozycji/obrotu/rozmiaru obiektu, na który patrzymy
    private void RefreshTarget()
    {
        if (currentTarget == null) return;

        if (!currentTarget.TryGetViewPose(cam, standardPosition, out Vector3 position, out Quaternion rotation, out float size))
        {
            Debug.LogWarning("CameraMover: obiekt kamery zniknął albo nie da się go wykadrować.");
            currentTarget = null;
            return;
        }

        targetPosition = currentTarget.zoomed ? position : standardPosition;
        targetRotation = rotation;
        targetSize = size;
    }

    public void backToStandardPos()
    {
        currentTarget = null;
        targetPosition = standardPosition;
        isMoving = true;
    }

    private void mover(CameraData camera)
    {
    // NIE UZYWAJ TEJ FUNKCJI POZA TYM PLIKIEM
        if (camera.active == true)
        {
            Quaternion previousRotation = targetRotation;

            currentTarget = camera;
            RefreshTarget();

            if (Quaternion.Angle(previousRotation, targetRotation) > 1f)
            {
                // Wydajemy dzwiek gdy obracamy fotel damn
                GetComponent<RandomAudioPlayer>().PlayRandomCreak();
            }

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
