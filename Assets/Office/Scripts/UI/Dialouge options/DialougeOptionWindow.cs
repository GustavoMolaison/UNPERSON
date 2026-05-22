using UnityEngine;
using TMPro;
public class DialougeOptionWindow : MonoBehaviour
{
    TextMeshProUGUI txt;
    private dialogueOption enrolledDialouge;


    private void Awake()
    {
        txt = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void enrollDialouge(dialogueOption dial)
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
        ConversationManager.Instance.chatNewMess(enrolledDialouge.DialougeContent, true);
    }
        
}
