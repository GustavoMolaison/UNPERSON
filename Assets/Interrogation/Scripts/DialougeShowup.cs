using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
public class DialougeShowup : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector] private DialogueOption enrolledDialOption;
    [HideInInspector] private string enrolledEvidenceName;
    [HideInInspector] private Evidence enrolledEvidence;
    [HideInInspector] bool hasEnrolledDialOption;

    List<DialogueOption> optionList = new List<DialogueOption>();

    Image img;
    private bool evidenceConnected = false;
    public bool clicked = false;

    private void Start()
    {
        img = GetComponent<Image>();
        
    }
    

    
    public void enroll(DialogueOption dialoption, string enrolledEvidName)
    {
        
        enrolledDialOption = dialoption;
        enrolledEvidenceName = enrolledEvidName;
        if(enrolledDialOption != null) 
        {
            hasEnrolledDialOption = true;
        }
        else
        {
            hasEnrolledDialOption = false;
        }
    }
    
    public void onClick(bool on)
    {
        if (on)
        {
            clicked = true;
            img.color = Color.white;
        }
        else
        {
            clicked = false;
            img.color = new Color32(171, 171, 171, 255);
        }
        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        TalkWindow.Instance.manageShowUps(this);
    }
    public void entryEvidenceConnect()
    {
        Debug.Log("Sprawdzam");
        if(enrolledDialOption != null)
        {
            
        
        int evidenceIndex = -1;

        for (int i = 0; i < enrolledDialOption.evidenceCheckDialogueLine.Count; i++) 
        {
           Evidence evid = enrolledDialOption.evidenceCheckDialogueLine[i];

           if (evid.name == enrolledEvidenceName)
              {
                 enrolledEvidence = evid;
                 evidenceIndex = i;
                 break;
              }
        }
        if(enrolledDialOption!= null && enrolledDialOption.hasEvidenceCheckDialogueLine && enrolledEvidence == Case_Monitor.Instance.currentlyPickedEvidence && evidenceConnected == false)
        {
            Debug.Log("sigma");
            evidenceConnected = true;
            optionList.Add(enrolledDialOption.unlockedDialouge[evidenceIndex]);
            DialogueOptionManager.Instance.turnOnChossenDialogues(optionList);
            enrolledDialOption.nodeTree.AddChildrenToParents(enrolledDialOption.unlockedDialouge[evidenceIndex].nodeTree);
        }

        }
    }
}
