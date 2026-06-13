using UnityEngine;
using System.Collections.Generic;


public class LevelsContentInfo : MonoBehaviour
{
    public static LevelsContentInfo Instance;
    [SerializeField] public List<Level> levelsList = new List<Level>();
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);


    }

    

}
