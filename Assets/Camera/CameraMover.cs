using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraMover : MonoBehaviour
{

    Camera cam;
    
    Vector3 standardPosition;
    Vector3 targetPosition;
    float targetSize = 2f;
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
    }

    public void backToStandardPos()
    {
        targetPosition = standardPosition;
    }

    /// <summary>
    /// Przesuwa obiekt p³ynnie do wskazanej pozycji, wspó³rzêdna z bêdzie zawsze na sztywno równa -1.
    /// </summary>
    
    public void moveTo(Vector3 pos, float size)
    {
        //Debug.Log("xd");
        pos.z = -1;
        targetPosition = pos;
        targetSize = size;

    }

    public void mover(GameManager.CameraData camera)
    { 
      if(camera.active == true)
        {
            moveTo(camera.pos, camera.size);
        }
    }


}
