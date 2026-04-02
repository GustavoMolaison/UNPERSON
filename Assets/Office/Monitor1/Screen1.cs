using UnityEngine;

public class Screen1 : MonoBehaviour
{

    public float cameraSize = 0.16f;
    public static Screen1 Instance;

    [SerializeField]
    private Canvas mainCanvas;
    [SerializeField]
    private Canvas suspectsCanvas;
    

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void OnMouseDown()
    {
        //Debug.Log("xd1");
        GameManager.Instance.changeCamera(GameManager.Instance.inMonitor1);
        //Debug.Log("halo");
        //CameraMover.Instance.moveTo(transform.position, cameraSize);
        //Debug.Log("halo");
    }

    public void suspectButton()
    {
        Debug.Log("halo");
        mainCanvas.enabled = false;
        suspectsCanvas.enabled = true;
    }
}
