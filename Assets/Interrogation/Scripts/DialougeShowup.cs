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
            optionList.Add(enrolledDialOption.unlockedDialouge);
            DialogueOptionManager.Instance.turnOnChossenDialouges(optionList);
            enrolledDialOption.nodeTree.AddChildrenToParents(enrolledDialOption.unlockedDialouge.nodeTree);
        }
    }
}
