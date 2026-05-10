using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class ChattersUICreator : MonoBehaviour
{
    [SerializeField] private GameObject chatterPrefab;
    [SerializeField] private GameObject chatterGroupPrefab;
    [SerializeField] private Transform container;
    public Dictionary<Suspect, GameObject> groupToSuspect;
    public static ChattersUICreator instance;

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

                Debug.Log(newRect.sizeDelta);
                Debug.Log(prefabRect.sizeDelta);
                //newRect.anchoredPosition = Vector2.zero; // Pozycja (0,0) wzglêdem anchorów
                //newRect.sizeDelta = prefabRect.sizeDelta; // Kopiowanie szerokoœci i wysokoœci
                newRect.localScale = Vector3.one;
                newRect.sizeDelta = new Vector2(30, 120);
                // Reset skali
            }


            // 4. Pêtla tworz¹ca dzieci
            for (int i = 0; i < convs.Count; i++)
            {
                GameObject child = Instantiate(chatterPrefab, mother.transform, false); // false mowi zeby mial wyjebane w pozycje swiatowa 
                child.name = "Chatter_" + i;
                mother.gameObject.SetActive(false);

            }
        }
    }
}
