using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class TalkWindow : MonoBehaviour
{
    [SerializeField] private GameObject child; 

    [Header("Parameters")]
    [SerializeField] private float rotation;
    private TextMeshProUGUI txt;


    public void addMessage(string message)
    {
        GameObject newChild = Instantiate(child, transform, false);
        newChild.transform.Rotate(new Vector3(0,1,0), rotation);
        txt = newChild.GetComponentInChildren<TextMeshProUGUI>();
        txt.transform.Rotate(new Vector3(0,1,0), rotation);
        if (txt != null)
        {
            txt.text = message;
        }
    }
}
