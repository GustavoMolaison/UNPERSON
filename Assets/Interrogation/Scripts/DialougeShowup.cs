using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
public class DialougeShowup : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector] DialogueOption enrolledDialOption;
    [HideInInspector] bool hasEnrolledDialOption;

    List<DialogueOption> optionList = new List<DialogueOption>();

    public void enroll(DialogueOption dialoption)
    {
        
        enrolledDialOption = dialoption;
        if(enrolledDialOption != null) 
        {
            hasEnrolledDialOption = true;
        }
        else
        {
            hasEnrolledDialOption = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
       
        if (hasEnrolledDialOption)
        {
            Debug.Log("dialog ma dowod i go klikam");
            optionList.Clear();
            

            if(enrolledDialOption.evidenceCheckDialougeLine == Case_Monitor.Instance.currentlyPickedEvidence)
            {
                optionList.Add(enrolledDialOption.unlockedDialouge);
                // if(enrolledDialOption.unlockedDialouge != null)
                // {
                //     Debug.Log(optionList.Count);
                // }

                DialogueOptionManager.Instance.turnOnChossenDialogues(optionList);
                DialogueOptionManager.Instance.optionsToLoad.AddRange(optionList);
                DialogueOptionManager.Instance.optionAddedOutsideTheLoop = true;
                // if(enrolledDialOption.unlockedDialouge == null)
                // {
                //   Debug.LogError("unlockedDialouge NIE ISTNIEJE");  
                // }
                // if(enrolledDialOption.unlockedDialouge.nodeTree == null)
                // {
                //   Debug.LogError("nodeTree NIE ISTNIEJE");  
                // }
                enrolledDialOption.nodeTree.AddChildrenToParents(enrolledDialOption.unlockedDialouge.nodeTree);
                
            }
            
        }
    }
}
