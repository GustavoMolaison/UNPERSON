using TMPro;
using UnityEngine;
using System.Collections.Generic;

// This script is meant to be placed on the parent of the dialogue option windows,
// it will be responsible for creating and deleting them when needed
// I think so idk i write this weeks after writing it
public class DialougeOptionManager : MonoBehaviour
{
    public static DialougeOptionManager Instance;
    public GameObject dialougePickWindow;
    

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

   
    public void cleanDialogueOptions()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
    public void dialougesChange(bool newDialougeSequence, List<DialogueOption> DialougeSequences = null)
    {

       
        Debug.Log("change");
        // cleanDialogueOptions();

        if (!newDialougeSequence)
        {
            Debug.Log("change new");
           Suspect intSusp = InterrogationManager.Instance.interrogatedSuspect;
           for (int i = 0; i < intSusp.DialogueOptions.Count; i++)
           {
            
            GameObject window = Instantiate(dialougePickWindow, transform, false);
            DialougeOptionWindow windowScript = window.GetComponent<DialougeOptionWindow>();
            //Debug.Log(intSusp.DialogueOptions[i].dialougeTittle);
            windowScript.enrollDialouge(intSusp.DialogueOptions[i]);
           } 
 


        }
        else
        {
            Debug.Log("change notnew");
            if(DialougeSequences != null)
            {
                for (int i = 0; i < DialougeSequences.Count; i++)
                {
                    GameObject window = Instantiate(dialougePickWindow, transform, false);
                    DialougeOptionWindow windowScript = window.GetComponent<DialougeOptionWindow>();
                    //Debug.Log(DialougeSequences[i]);
                    
                    windowScript.enrollDialouge(DialougeSequences[i]);
                }
            }
            
        }

        
    }


}
