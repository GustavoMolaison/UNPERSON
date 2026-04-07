using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Screen1 : MonoBehaviour
{

    public float cameraSize = 0.16f;
    public static Screen1 Instance;

    [SerializeField]
    private Canvas mainCanvas;
    [SerializeField]
    private Canvas suspectsCanvas;
    private List<Canvas> canvasList;

    private Canvas prevCanvas;
    

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        canvasList = new List<Canvas> { mainCanvas, suspectsCanvas };
        mainCanvas.enabled = true;
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
        // Debug.Log("halo");
        // mainCanvas.enabled = false;
        // suspectsCanvas.enabled = true;
        screenChanger(suspectsCanvas);
    }
    
    public void backButton()
    {
        screenChanger(prevCanvas);
    }

    public void screenChanger(Canvas screen)
    {
       prevCanvas = canvasList.Where(x => x.enabled).FirstOrDefault();
       prevCanvas.enabled = false;

       screen.enabled = true;

        
    }
}
