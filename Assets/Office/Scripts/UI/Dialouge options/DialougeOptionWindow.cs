using UnityEngine;
using TMPro;
using System.Collections;
public class DialougeOptionWindow : MonoBehaviour
{
    TextMeshProUGUI txt;
    private DialogueOption enrolledDialouge;


    private void Awake()
    {
        txt = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void enrollDialouge(DialogueOption dial)
    {
        enrolledDialouge = dial;
        changeText(enrolledDialouge.dialougeTittle);
    }
    public void changeText(string newText)
    {
        txt.text = newText;
    }

    public void onClick()
    {
        DialogueManager.Instance.StartCoroutine(DialogueManager.Instance.dialogueOptionClicked(enrolledDialouge));
    }

    // public IEnumerator onClickWait()
    // {
    //     ConversationManager.Instance.chatNewMess(enrolledDialouge.dialougeContent); //THIS FRIST
    //     DialougeOptionManager.Instance.cleanDialogueOptions(); // THIS SECOND
    //     yield return new WaitUntil(() => DialogueManager.Instance.isProcessingQueue == false); // THIS THIRD

    //     // ConversationManager.Instance.chatNewMess(enrolledDialouge.dialougeContent);
    //     if(enrolledDialouge.isNewDialogueSequence)
    //     {
    //         DialougeOptionManager.Instance.dialougesChange(true, enrolledDialouge.newdialogueSequence);
    //     }
    //     else
    //     {
    //         DialougeOptionManager.Instance.dialougesChange(false);
    //     }
    // } 
}
