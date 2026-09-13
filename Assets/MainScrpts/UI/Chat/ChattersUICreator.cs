using System.Collections.Generic;
using System.Text.RegularExpressions;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChattersUICreator : MonoBehaviour
{
    [SerializeField] private GameObject chatterPrefab;
    [SerializeField] private GameObject chatterGroupPrefab;
    [SerializeField] private Transform container;
    
    
    public static ChattersUICreator instance;
    private TextMeshProUGUI chatterName;


    [SerializeField] private GameObject Chatlayout;
    [SerializeField] private GameObject Cloud;
    [SerializeField] private GameObject ChatContainer;
    




    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        
    }
    public void CreateChatersPreFab(Suspect sus)
    {
        {
            List<Conversation> convs = sus.ChatHistory;
            
            GameObject mother = Instantiate(chatterGroupPrefab, container.transform);
            mother.transform.localPosition = Vector3.zero;
            mother.name = "Chatter_Group";
            Screen2.Instance.groupToSuspect.Add(sus, mother);



            // Zadbanie by rozmiar prefaba by³ odpowiedni
            RectTransform prefabRect = chatterPrefab.GetComponent<RectTransform>();
            RectTransform newRect = mother.GetComponent<RectTransform>();

            if (prefabRect != null && newRect != null)
            {
                newRect.localScale = Vector3.one;
                newRect.sizeDelta = new Vector2(30, 120);
            }


            

            // 4. Pêtla tworz¹ca dzieci
            for (int i = 0; i < convs.Count; i++)
            {
                GameObject child = Instantiate(chatterPrefab, mother.transform, false); // false mowi zeby mial wyjebane w pozycje swiatowa 
                child.name = "Chatter_" + i;
                //Debug.Log("child" + i);
                chatterName = child.GetComponentInChildren<TextMeshProUGUI>();
                chatterName.text = convs[i].ParticipantName;

                //mother.gameObject.SetActive(false);

                // To tworzy faktyczne czaty z wiadomoœciami
                CreateChatLayoutPreFab(convs[i], child);


            }
            mother.gameObject.SetActive(false);
        }
    }

    public void CreateChatLayoutPreFab(Conversation convs, GameObject chatter)
    {
        List<ChatMessage> messList = convs.messages;
        GameObject mother = Instantiate(Chatlayout, ChatContainer.transform);
        mother.transform.localPosition = new Vector3(0, -9, 0);
        mother.name = "Chat_layout";

        Screen2.Instance.chatterToConv.Add(chatter, mother);



        //mother.gameObject.SetActive(false);

        for (int i = 0; i < messList.Count; i++)
        {
            GameObject child = Instantiate(Cloud, mother.transform, false);
            child.name = "Cloud_chat" + i;
            TextMeshProUGUI MesText = child.GetComponentInChildren<TextMeshProUGUI>();      
            MesText.text = messList[i].content;
            if(i % 2 != 0)
            {
                child.transform.localRotation = Quaternion.Euler(0, 180, 0);
                MesText.transform.localRotation = Quaternion.Euler(0, 180, 0);
            }

        }

        mother.gameObject.SetActive(false);
    }


}
