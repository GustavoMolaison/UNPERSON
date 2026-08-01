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
    private TypewriterEffect typewriter;


    public IEnumerator addMessage(string message, bool isPlayer)
    {
        GameObject newChild = Instantiate(child, transform, false);
        newChild.transform.Rotate(new Vector3(0,1,0), rotation);

        typewriter = newChild.GetComponentInChildren<TypewriterEffect>();

        typewriter.transform.Rotate(new Vector3(0,1,0), rotation);
        
        if (typewriter != null)
        {
            string txt = isPlayer? 
            "You: " + message : 
            SuspectTracker.instance.currentSuspect.FirstName + ": " + message;
            
            yield return typewriter.SetText(txt);
        }
    }
}
