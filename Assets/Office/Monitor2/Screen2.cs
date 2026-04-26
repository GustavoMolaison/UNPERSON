using UnityEngine;

public class Screen2 : MonitorBase
{
    public static Screen2 Instance;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnMouseDown()
    {
        GameManager.Instance.changeCamera(GameManager.Instance.inMonitor2);
    }


    public void backButton()
    {
        screenChanger(prevCanvas);
    }
}

    
