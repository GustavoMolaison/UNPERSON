using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
public class DialougeShowup : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector] private DialogueOption enrolledDialOption;
    [HideInInspector] bool hasEnrolledDialOption;

    List<DialogueOption> optionList = new List<DialogueOption>();

    Image img;
    private bool evidenceConnected = false;
    public bool clicked = false;

    private void Start()
    {
        img = GetComponent<Image>();
        
    }
    

    
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
        if(enrolledDialOption!= null && enrolledDialOption.hasEvidenceCheckDialougeLine && enrolledDialOption.evidenceCheckDialougeLine == Case_Monitor.Instance.currentlyPickedEvidence && evidenceConnected == false)
        {
            evidenceConnected = true;
            optionList.Add(enrolledDialOption.unlockedDialouge);
            DialogueOptionManager.Instance.turnOnChossenDialogues(optionList);
            enrolledDialOption.nodeTree.AddChildrenToParents(enrolledDialOption.unlockedDialouge.nodeTree);
        }
    }
}
