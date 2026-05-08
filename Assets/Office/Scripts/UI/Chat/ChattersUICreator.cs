using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class ChattersUICreator : MonoBehaviour
{
    [SerializeField] private GameObject chatterPrefab;
    [SerializeField] private Transform container;
    public Dictionary<Suspect, GameObject> groupToSuspect;
    public static ChattersUICreator instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        groupToSuspect = new Dictionary<Suspect, GameObject>();
    }
    public void CreateChatersPreFab(Suspect sus)
    {
        {
            List<Conversation> convs = sus.ChatHistory;
            // 1. Tworzymy "Matkê"
            GameObject mother = new GameObject("ChatterGroup", typeof(RectTransform));
            mother.transform.SetParent(container, false);
            groupToSuspect.Add(sus, mother);

            
            // 4. Pêtla tworz¹ca dzieci
            for (int i = 0; i < convs.Count; i++)
            {
                GameObject child = Instantiate(chatterPrefab, mother.transform);
                child.name = "Chatter_" + i;
                mother.gameObject.SetActive(false);

            }
        }
    }
}
