using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.Localization.Metadata;
using UnityEngine.ResourceManagement.AsyncOperations;


public class UiDialougeManager : MonoBehaviour
{
    

   
    [SerializeField] private GameObject chatlayout;
    [SerializeField] public float messageCooldown;
    [HideInInspector] public float originalmessageCooldown;
    
    

   

    Vector2 playerDimensions;
    Vector2 SuspectDimensions;
    Vector2 chatCloudDimensions;
    TalkWindow layoutCode;


    


    public static UiDialougeManager Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    
        private void Start()
    {
        originalmessageCooldown = messageCooldown;
        playerDimensions = GetUiDimensions(chatlayout);

        layoutCode = chatlayout.GetComponent<TalkWindow>();
    }
    
public IEnumerator ShowMessagesRoutine(DialogueOption dialoption)
{
    LocalizedStringTable tableReference = dialoption.dialogueTable;
    // 1. Musimy najpierw asynchronicznie pobrać właściwą tabelę StringTable
    var tableHandle = tableReference.GetTableAsync();
    yield return tableHandle;

    if (tableHandle.Status != AsyncOperationStatus.Succeeded)
    {
        Debug.LogError("Nie udało się załadować tabeli lokalizacyjnej!");
        yield break;
    }

    StringTable table = tableHandle.Result;

    // 2. Pobieramy wszystkie wpisy z tabeli (są przechowywane jako KeyValuePair)
    var entries = new List<StringTableEntry>(table.Values);

    for (int i = 0; i < entries.Count; i++)
    {
        StringTableEntry entry = entries[i];
         
        bool isPlayer;
        bool isSlowed;
        bool isEvidenceConntected;
        Color color;

        var sharedEntry = table.SharedData.GetEntry(entry.KeyId);
        var commentMeta = sharedEntry.Metadata.GetMetadata<Comment>();
        
        (isPlayer, isSlowed, isEvidenceConntected, color) = metaDataParser(commentMeta.CommentText);
        Debug.Log("isPlayer: " + isPlayer);
        Debug.Log("isSlowed: " + isSlowed);
        Debug.Log("isEvidenceConntected: " + isEvidenceConntected);
        Debug.Log("color: " + color);

        
        
       
        
        if (commentMeta != null && commentMeta.CommentText.Trim() == "1")
            {
                 isPlayer = true;
            }
        

        
        
       

        cleanDialogueLayout(isPlayer);

        // Z loaded StringTable tekst mamy od razu w strefie pamięci (LocalizedValue)
        string messageText = entry.LocalizedValue;

        yield return StartCoroutine(layoutCode.addMessage(messageText, isPlayer, isEvidenceConntected, dialoption));

        if (i < entries.Count - 1)
        {
            yield return new WaitForSeconds(messageCooldown);
        }
    }
}
    

    public void forceCleanChat()
    {
        foreach (Transform child in layoutCode.transform)
        {
            Destroy(child.gameObject);
        }

        
    }

    public void cleanDialogueLayout(bool isPlayerChat)
    {
        RectTransform layoutRect = layoutCode.GetComponent<RectTransform>();
        ManageChatOverflow(layoutRect, playerDimensions);    

    }



    private Vector2 GetUiDimensions(GameObject go)
    {
        // 1. Sprawdzenie, czy sam obiekt nie jest nullem
        if (go == null)
        {
            Debug.LogError("Przekazany GameObject jest pusty (null)!");
            return Vector2.zero;
        }

        // 2. Bezpieczna pr�ba wyci�gni�cia RectTransform
        if (go.TryGetComponent<RectTransform>(out RectTransform rectTransform))
        {
            // Je�li obiekt ma RectTransform, zwracamy jego wymiary
            float width = rectTransform.rect.width;
            float height = rectTransform.rect.height;
            return new Vector2(width, height);
        }
        else
        {
            // Krok krytyczny: Je�li obiekt to np. zwyk�y Cube 3D, a nie element UI
            Debug.LogError($"Obiekt '{go.name}' nie posiada komponentu RectTransform! Czy to na pewno element UI Canvasa?");
            return Vector2.zero;
        }
    }

// Ta funkcja ma zadbac by dialogi pięknie i ładnie się wyświetlały
    public void ManageChatOverflow(RectTransform layoutRect, Vector2 windowDimensions)
    {
        Debug.Log("Czyścimy");
        if (layoutRect == null) return;

        // 1. Pobieramy komponent VerticalLayoutGroup
        if (!layoutRect.TryGetComponent<VerticalLayoutGroup>(out VerticalLayoutGroup layoutInfo))
        {
            Debug.LogError($"Obiekt {layoutRect.name} nie ma komponentu VerticalLayoutGroup!");
            return;
        }

        // 2. Obliczamy aktualną wysokość zawartości czatu
        float totalContentHeight = layoutInfo.padding.top + layoutInfo.padding.bottom;
        int activeChildCount = 0;

        foreach (Transform child in layoutRect)
        {
            if (!child.gameObject.activeSelf) continue;

            if (child.TryGetComponent<RectTransform>(out RectTransform childRect))
            {
                //Debug.Log("Adding height of cloud");
                totalContentHeight += childRect.rect.height;
                activeChildCount++;
            }
        }

        if (activeChildCount > 1)
        {
            //Debug.Log("Adding Spacing");
            totalContentHeight += layoutInfo.spacing * (activeChildCount - 1);
        }

        // Okej od tego momenu mamy obliczone wszystkie wartości
        

        // 3. Pętla While czyszcząca czat, gdy zawartość przekracza wysokość okna
        float windowHeight = windowDimensions.y;
        if (windowHeight < totalContentHeight + 50)
            
        {
            Debug.Log("Przekracza");
            while (windowHeight < totalContentHeight + 50 && layoutRect.childCount > 0)
            {
                GameObject oldestCloud = layoutRect.GetChild(0).gameObject;
                RectTransform cloudRect = oldestCloud.GetComponent<RectTransform>();

                totalContentHeight -= (cloudRect.rect.height + layoutInfo.spacing);
                oldestCloud.transform.SetParent(null);
                Debug.Log("USUNIECIE");
                Destroy(oldestCloud);

                // Wymuszamy aktualizacj� layoutu, by dane w p�tli by�y poprawne
                LayoutRebuilder.ForceRebuildLayoutImmediate(layoutRect);
            }

            // Dodatkowe czyszczenie ("na zapas")
            if (layoutRect.childCount > 0)
            {
                GameObject oldestCloud1 = layoutRect.GetChild(0).gameObject;
                RectTransform cloudRect1 = oldestCloud1.GetComponent<RectTransform>();
                totalContentHeight -= cloudRect1.rect.height;

                oldestCloud1.transform.SetParent(null);
                Destroy(oldestCloud1);

                // Finalna aktualizacja po usuni�ciu bonusowej chmurki
                LayoutRebuilder.ForceRebuildLayoutImmediate(layoutRect);
            }
        }

        
       

       
    }

    private (bool, bool, bool, Color) metaDataParser(string data)
    {
        bool isPlayer = false;
        bool isSlowed = false;
        bool isEvidenceConnected = false;

        List<bool> boolList = new List<bool> { isPlayer, isSlowed, isEvidenceConnected };

        string[] words = data.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);

        for(int i = 0; i < words.Length; i++)
        {
            // kolor nie jest oparty na boolu wiec go skipujemy
            if(i == 3)
                continue;

            boolList[i] = words[i][1] == '1';
            Debug.Log("Bool " + i + ": " + boolList[i]);
        }
        

        Color color; 
        if(words[2].Length < 2)
        {
            color = Color.white;
        }
        else
        {
            switch (words[2])
           {
            case "red":
                color = Color.red;
                break;

            default:
                color = Color.white;
                break;
            
           }
        }
        

        return (boolList[0], boolList[1], boolList[2], color);
    }
}

