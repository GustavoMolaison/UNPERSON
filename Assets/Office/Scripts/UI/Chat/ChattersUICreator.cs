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
    public Dictionary<Suspect, GameObject> groupToSuspect;
    public static ChattersUICreator instance;
    private TextMeshProUGUI chatterName;




    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        // Dict for picking which convesration should be shown based on currently picked suspect
        groupToSuspect = new Dictionary<Suspect, GameObject>();
    }
    public void CreateChatersPreFab(Suspect sus)
    {
        {
            List<Conversation> convs = sus.ChatHistory;
            
            GameObject mother = Instantiate(chatterGroupPrefab, container.transform);
            mother.transform.localPosition = Vector3.zero;
            mother.name = "Chatter_Group";
            groupToSuspect.Add(sus, mother);



            // Zadbanie by rozmiar prefaba by³ odpowiedni
            RectTransform prefabRect = chatterPrefab.GetComponent<RectTransform>();
            RectTransform newRect = mother.GetComponent<RectTransform>();

            if (prefabRect != null && newRect != null)
            {
                newRect.localScale = Vector3.one;
                newRect.sizeDelta = new Vector2(30, 120);
            }


            mother.gameObject.SetActive(false);

            // 4. Pêtla tworz¹ca dzieci
            for (int i = 0; i < convs.Count; i++)
            {
                GameObject child = Instantiate(chatterPrefab, mother.transform, false); // false mowi zeby mial wyjebane w pozycje swiatowa 
                child.name = "Chatter_" + i;
                Debug.Log("child" + i);
                chatterName = child.GetComponentInChildren<TextMeshProUGUI>();
                chatterName.text = convs[i].ParticipantName;

                mother.gameObject.SetActive(false);


            }
        }
    }
}
