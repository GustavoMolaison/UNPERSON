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

    public void chatNewMess(List<string> messeages, bool isPlayerChat)
    {
        Debug.Log("mess:" +  messeages);
        UiDialougeManager.Instance.chatNewMess(messeages, isPlayerChat);

    }

}
