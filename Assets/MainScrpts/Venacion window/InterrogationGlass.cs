using TMPro;
using UnityEngine;

public class InterrogationGlass : MonoBehaviour
{

    public static InterrogationGlass Instance;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        


    }
}
