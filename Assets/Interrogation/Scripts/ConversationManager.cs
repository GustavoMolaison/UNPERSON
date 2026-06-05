using UnityEngine;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;

public class ConversationManager : MonoBehaviour
{
    
    
    public static ConversationManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void chatNewMess(List<DialogueLine> messeages)
    {
        Debug.Log("mess:" +  messeages);
        DialogueManager.Instance.chatNewMess(messeages);

    }

}
