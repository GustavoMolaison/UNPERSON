using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;



public class TalkWindow : MonoBehaviour
{
    [SerializeField] private GameObject child; 

    [Header("Parameters")]
    [SerializeField] private float rotation;
    private TypewriterEffect typewriter;

    private int clickedCount = 0;

    public static TalkWindow Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Update()
    {
        if (prevShowUp != null && prevShowUp.clicked)
        {  
            prevShowUp.entryEvidenceConnect();  
        }
    }
    public IEnumerator addMessage(string message, bool isPlayer, bool isEvidenceConnected, string connectedEvidence, DialogueOption dialoption)
    {

        GameObject newChild = Instantiate(child, transform, false);
        if (isEvidenceConnected)
        {
            DialougeShowup dialShowUp = newChild.GetComponent<DialougeShowup>();
            dialShowUp.enroll(dialoption, connectedEvidence);
        }
        

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

    private DialougeShowup prevShowUp = null;
    public void manageShowUps(DialougeShowup showup)
    {
        // Debug.Log(prevShowUp);
        if(prevShowUp == showup)
        {
            Debug.Log("takisam");
            showup.onClick(false);
            prevShowUp = null;
            return;
        }

        else if (prevShowUp != null)
       {
        Debug.Log("2");
        prevShowUp.onClick(false);
       }
        Debug.Log("Biały");
        showup.onClick(true);
        prevShowUp = showup;
    }
}
