using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
public class DialogueOptionWindow : MonoBehaviour
{
    TextMeshProUGUI txt;
    public DialogueOption enrolledDialogue;
    private bool clicked = false;
    public bool initialized = false;


    private void Awake()
    {
        txt = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void enrollDialogue(DialogueOption dial)
    {
        Debug.Log("Enrolling dialogue: ");
        enrolledDialogue = dial;
        changeText(enrolledDialogue.dialogueTitle);
        initialized = true;
    }
    public void changeText(string newText)
    {
        txt.text = newText;
    }

    public void onClick()
    {
        
        DialogueManager.Instance.StartCoroutine(DialogueManager.Instance.dialogueOptionClicked(enrolledDialogue));

        if (!clicked)
        {
            clicked = true;
            Image img = GetComponent<Image>();
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0.5f); 
        }
    }

    // public IEnumerator onClickWait()
    // {
    //     ConversationManager.Instance.chatNewMess(enrolledDialogue.dialogueContent); //THIS FRIST
    //     DialogueOptionManager.Instance.cleanDialogueOptions(); // THIS SECOND
    //     yield return new WaitUntil(() => DialogueManager.Instance.isProcessingQueue == false); // THIS THIRD

    //     // ConversationManager.Instance.chatNewMess(enrolledDialogue.dialogueContent);
    //     if(enrolledDialogue.isNewDialogueSequence)
    //     {
    //         DialogueOptionManager.Instance.dialoguesChange(true, enrolledDialogue.newDialogueSequence);
    //     }
    //     else
    //     {
    //         DialogueOptionManager.Instance.dialoguesChange(false);
    //     }
    // } 
}
