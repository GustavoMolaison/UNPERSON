using UnityEngine;


public class Case_Monitor : MonitorBase
{
    public static Case_Monitor Instance;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
}
