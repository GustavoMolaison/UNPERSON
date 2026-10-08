using UnityEngine;

public class Monitor1 : MonoBehaviour
{

    public static Monitor1 Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        
    }
}
