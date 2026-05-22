using UnityEngine;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;

public class ConversationManager : MonoBehaviour
{
    private GameObject playerChat;
    private GameObject suspectChat;

    [SerializeField] private GameObject layoutPlayer;
    [SerializeField] private GameObject layoutSuspect;

    [SerializeField] private GameObject chatCloud;
    private GameObject child;
    private TextMeshProUGUI txt;

    // Przyjmujemy list<string>
    public static ConversationManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void chatNewMess(List<string> messeages, bool isPlayerChat)
    {
        if (isPlayerChat)
        {
            for (int i = 0; i < messeages.Count; i++)
            {
                child = Instantiate(chatCloud, layoutPlayer.transform, false);
                txt = child.GetComponentInChildren<TextMeshProUGUI>();
                if(txt == null)
                {
                    Debug.Log("STOP blagam blagam");
                }
                Debug.Log(messeages[i]);
                txt.text = messeages[i];
            }

        }
        else
        {
            for (int i = 0; i < messeages.Count; i++)
            {
                child = Instantiate(chatCloud, layoutSuspect.transform, false);
                txt = child.GetComponent<TextMeshProUGUI>();
                txt.text = messeages[i];
            }
        }

    }

}
