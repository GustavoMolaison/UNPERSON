using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiDialougeManager : MonoBehaviour
{
    

    [SerializeField] private GameObject layoutPlayer;
    [SerializeField] private GameObject layoutSuspect;
    [SerializeField] private GameObject chatCloud;
    [SerializeField] private float messageCooldown;

   

    Vector2 playerDimensions;
    Vector2 SuspectDimensions;
    Vector2 chatCloudDimensions;
    VerticalLayoutGroup layoutInfo;


    


    public static UiDialougeManager Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    
        private void Start()
    {
        playerDimensions = GetUiDimensions(layoutPlayer);
        SuspectDimensions = GetUiDimensions(layoutSuspect);
        chatCloudDimensions = GetUiDimensions(chatCloud);

        layoutInfo = layoutPlayer.GetComponent<VerticalLayoutGroup>();
    }
    
    public IEnumerator ShowMessagesRoutine(List<string> messages, bool isPlayerChat)
    {
        // Ustalamy rodzica TYLKO RAZ przed pêtl¹ (Zasada DRY)
        Transform targetLayout = isPlayerChat ? layoutPlayer.transform : layoutSuspect.transform;

        for (int i = 0; i < messages.Count; i++)
        {
            // Deklarujemy zmienn¹ 'child' LOKALNIE w pêtli
            cleanDialogueLayout(isPlayerChat);
            GameObject child = Instantiate(chatCloud, targetLayout, false);

            // Deklarujemy 'txt' LOKALNIE. Zak³adam, ¿e prefab ma tekst w dziecku.
            TextMeshProUGUI txt = child.GetComponentInChildren<TextMeshProUGUI>();

            if (txt != null)
            {
                txt.text = messages[i];
            }
            else
            {
                Debug.LogError("B³¹d: Prefab chatCloud nie ma komponentu TextMeshProUGUI w dzieciach!");
            }

            // Czekamy okreœlon¹ iloœæ sekund przed instancjacj¹ kolejnego dymku
            if (i < messages.Count - 1) 
            {
                yield return new WaitForSeconds(messageCooldown);
            }


    }
    }

    public void cleanDialogueLayout(bool isPlayerChat)
    {
        if (isPlayerChat)
        {
            RectTransform layoutRect = layoutPlayer.GetComponent<RectTransform>();
            ManageChatOverflow(layoutRect, playerDimensions, chatCloudDimensions);
        }
        else
        {
            RectTransform layoutRect = layoutSuspect.GetComponent<RectTransform>();
            ManageChatOverflow(layoutRect, SuspectDimensions, chatCloudDimensions);
        }
           

    }



    private Vector2 GetUiDimensions(GameObject go)
    {
        // 1. Sprawdzenie, czy sam obiekt nie jest nullem
        if (go == null)
        {
            Debug.LogError("Przekazany GameObject jest pusty (null)!");
            return Vector2.zero;
        }

        // 2. Bezpieczna próba wyci¹gniêcia RectTransform
        if (go.TryGetComponent<RectTransform>(out RectTransform rectTransform))
        {
            // Jeœli obiekt ma RectTransform, zwracamy jego wymiary
            float width = rectTransform.rect.width;
            float height = rectTransform.rect.height;
            return new Vector2(width, height);
        }
        else
        {
            // Krok krytyczny: Jeœli obiekt to np. zwyk³y Cube 3D, a nie element UI
            Debug.LogError($"Obiekt '{go.name}' nie posiada komponentu RectTransform! Czy to na pewno element UI Canvasa?");
            return Vector2.zero;
        }
    }


    public void ManageChatOverflow(RectTransform layoutRect, Vector2 windowDimensions, Vector2 chatCloudDimensions)
    {
        Debug.Log("Czyœcimy");
        if (layoutRect == null) return;

        // 1. Pobieramy komponent VerticalLayoutGroup
        if (!layoutRect.TryGetComponent<VerticalLayoutGroup>(out VerticalLayoutGroup layoutInfo))
        {
            Debug.LogError($"Obiekt {layoutRect.name} nie ma komponentu VerticalLayoutGroup!");
            return;
        }

        // 2. Obliczamy aktualn¹ wysokoœæ zawartoœci czatu
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

        // 3. Pêtla While czyszcz¹ca czat, gdy zawartoœæ przekracza wysokoœæ okna
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

                // Wymuszamy aktualizacjê layoutu, by dane w pêtli by³y poprawne
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

                // Finalna aktualizacja po usuniêciu bonusowej chmurki
                LayoutRebuilder.ForceRebuildLayoutImmediate(layoutRect);
            }
        }

        
        //int currentChildCount = layoutRect.childCount;
        //float estimatedHeight = (currentChildCount * chatCloudDimensions.y) + (layoutInfo.spacing * (currentChildCount - 1));

       
    }
}

