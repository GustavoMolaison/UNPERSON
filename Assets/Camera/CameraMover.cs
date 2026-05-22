using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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
    }

    public void backToStandardPos()
    {
        targetPosition = standardPosition;
    }

    /// <summary>
    /// Przesuwa obiekt p�ynnie do wskazanej pozycji, wsp�rz�dna z b�dzie zawsze na sztywno r�wna -1.
    /// </summary>
    
    public void moveTo(Vector3 pos, float size, Vector3 angle)
    {
        //Debug.Log("xd");
        pos.z = -1;
        targetPosition = pos;
        targetSize = size;
        targetAngle = angle;


    }

    public void mover(CameraData camera)
    { 
    //   Debug.Log(camera.active);
      if(camera.active == true)
        {
            moveTo(camera.pos, camera.size, camera.angle);
        }
    }


}
