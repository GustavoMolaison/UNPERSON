using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiDialougeManager : MonoBehaviour
{
    

    [SerializeField] private GameObject layoutPlayer;
    [SerializeField] private GameObject layoutSuspect;
    [SerializeField] private GameObject chatCloud;

    private GameObject child;
    private TextMeshProUGUI txt;

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
        for (int i = 0; i <= layoutPlayer.transform.childCount; i++)
        {

        }


    }
    public void chatNewMess(List<string> messeages, bool isPlayerChat)
    {

        cleanDialogueLayout(isPlayerChat);

        if (isPlayerChat)
        {
            for (int i = 0; i < messeages.Count; i++)
            {
                child = Instantiate(chatCloud, layoutPlayer.transform, false);
                txt = child.GetComponentInChildren<TextMeshProUGUI>();
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


    private void ManageChatOverflow(RectTransform layoutRect, Vector2 windowDimensions, Vector2 chatCloudDimensions)
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
                totalContentHeight += childRect.rect.height;
                activeChildCount++;
            }
        }

        if (activeChildCount > 1)
        {
            totalContentHeight += layoutInfo.spacing * (activeChildCount - 1);
        }

        // 3. Pêtla While czyszcz¹ca czat, gdy zawartoœæ przekracza wysokoœæ okna
        float windowHeight = windowDimensions.y;
        if (windowHeight < totalContentHeight)
            
        {
            Debug.Log("Przekracza");
            while (windowHeight < totalContentHeight && layoutRect.childCount > 0)
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

        // 4. Twój dodatkowy warunek sprawdzaj¹cy sztywny rozmiar
        int currentChildCount = layoutRect.childCount;
        float estimatedHeight = (currentChildCount * chatCloudDimensions.y) + (layoutInfo.spacing * (currentChildCount - 1));

       
    }
}

