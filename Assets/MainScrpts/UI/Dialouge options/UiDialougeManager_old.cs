// using System.Collections;
// using System.Collections.Generic;
// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;

// public class UiDialougeManager_old : MonoBehaviour
// {
    

//     [SerializeField] private GameObject layoutPlayer;
//     [SerializeField] private GameObject layoutSuspect;
//     [SerializeField] private GameObject chatCloud;
//     [SerializeField] private float messageCooldown;

   

//     Vector2 playerDimensions;
//     Vector2 SuspectDimensions;
//     Vector2 chatCloudDimensions;
//     TalkWindow layoutCode;


    


//     public static UiDialougeManager_old Instance;
//     private void Awake()
//     {
//         if (Instance == null) Instance = this;
//         else Destroy(gameObject);
//     }

    
//         private void Start()
//     {
//         playerDimensions = GetUiDimensions(layoutPlayer);
//         SuspectDimensions = GetUiDimensions(layoutSuspect);
//         chatCloudDimensions = GetUiDimensions(chatCloud);

//         layoutCode = layoutPlayer.GetComponent<TalkWindow>();
//     }
    
//     public IEnumerator ShowMessagesRoutine(List<DialogueLine> messages)
//     {

//         for (int i = 0; i < messages.Count; i++)
//         {
//                 // Jeśli speaker to Player używamy layoutPlayer w przeciwnym razie layoutSuspect
//                 TalkWindow targetLayoutCode = messages[i].speaker == SpeakerType.Player ? layoutPlayer.GetComponent<TalkWindow>() : layoutSuspect.GetComponent<TalkWindow>();
//                 // Sprawdzamy czy mowi player czy suspcet huj wie czemu nie uzywamy tego wczesniej zeby usalic jaki jest talk window xd
//                 bool isPlayer = messages[i].speaker == SpeakerType.Player ? true : false;

//                 // Wbrew nazwie nie czyści to layouta poprostu przygotowuje go do wyświetlenia następnej wiadomości
//                 // Ta funkcja chyba odpowiada jak to faktycznie wizualnie sie prezentuje
//                 cleanDialogueLayout(isPlayer);
//                 // To poprostu przekazuje stringa jakims pojebanym sposobem bo tak dziala lokalizakcja w unity idk
//                 messages[i].text.GetLocalizedStringAsync().Completed += handle =>
//                 {
//                   targetLayoutCode.addMessage(handle.Result);
//                 };
                

//                 // Jeśli nie jesteśmy na ostatnim elemencie czekamy chwile zanim wysiwetlimy nastepna wiadomość
//                 if (i < messages.Count - 1) 
//                 {
//                 yield return new WaitForSeconds(messageCooldown);
//                 }
            



//     }
//     }

//     public void forceCleanChat()
//     {
//         foreach (Transform child in layoutPlayer.transform)
//         {
//             Destroy(child.gameObject);
//         }

//         foreach (Transform child in layoutSuspect.transform)
//         {
//             Destroy(child.gameObject);
//         }
//     }

//     public void cleanDialogueLayout(bool isPlayerChat)
//     {
//         if (isPlayerChat)
//         {
//             RectTransform layoutRect = layoutPlayer.GetComponent<RectTransform>();
//             ManageChatOverflow(layoutRect, playerDimensions, chatCloudDimensions);
//         }
//         else
//         {
//             RectTransform layoutRect = layoutSuspect.GetComponent<RectTransform>();
//             ManageChatOverflow(layoutRect, SuspectDimensions, chatCloudDimensions);
//         }
           

//     }



//     private Vector2 GetUiDimensions(GameObject go)
//     {
//         // 1. Sprawdzenie, czy sam obiekt nie jest nullem
//         if (go == null)
//         {
//             Debug.LogError("Przekazany GameObject jest pusty (null)!");
//             return Vector2.zero;
//         }

//         // 2. Bezpieczna pr�ba wyci�gni�cia RectTransform
//         if (go.TryGetComponent<RectTransform>(out RectTransform rectTransform))
//         {
//             // Je�li obiekt ma RectTransform, zwracamy jego wymiary
//             float width = rectTransform.rect.width;
//             float height = rectTransform.rect.height;
//             return new Vector2(width, height);
//         }
//         else
//         {
//             // Krok krytyczny: Je�li obiekt to np. zwyk�y Cube 3D, a nie element UI
//             Debug.LogError($"Obiekt '{go.name}' nie posiada komponentu RectTransform! Czy to na pewno element UI Canvasa?");
//             return Vector2.zero;
//         }
//     }

// // Ta funkcja ma zadbac by dialogi pięknie i ładnie się wyświetlały
//     public void ManageChatOverflow(RectTransform layoutRect, Vector2 windowDimensions, Vector2 chatCloudDimensions)
//     {
//         Debug.Log("Czyścimy");
//         if (layoutRect == null) return;

//         // 1. Pobieramy komponent VerticalLayoutGroup
//         if (!layoutRect.TryGetComponent<VerticalLayoutGroup>(out VerticalLayoutGroup layoutInfo))
//         {
//             Debug.LogError($"Obiekt {layoutRect.name} nie ma komponentu VerticalLayoutGroup!");
//             return;
//         }

//         // 2. Obliczamy aktualną wysokość zawartości czatu
//         float totalContentHeight = layoutInfo.padding.top + layoutInfo.padding.bottom;
//         int activeChildCount = 0;

//         foreach (Transform child in layoutRect)
//         {
//             if (!child.gameObject.activeSelf) continue;

//             if (child.TryGetComponent<RectTransform>(out RectTransform childRect))
//             {
//                 //Debug.Log("Adding height of cloud");
//                 totalContentHeight += childRect.rect.height;
//                 activeChildCount++;
//             }
//         }

//         if (activeChildCount > 1)
//         {
//             //Debug.Log("Adding Spacing");
//             totalContentHeight += layoutInfo.spacing * (activeChildCount - 1);
//         }

//         // Okej od tego momenu mamy obliczone wszystkie wartości
        

//         // 3. Pętla While czyszcząca czat, gdy zawartość przekracza wysokość okna
//         float windowHeight = windowDimensions.y;
//         if (windowHeight < totalContentHeight + 50)
            
//         {
//             Debug.Log("Przekracza");
//             while (windowHeight < totalContentHeight + 50 && layoutRect.childCount > 0)
//             {
//                 GameObject oldestCloud = layoutRect.GetChild(0).gameObject;
//                 RectTransform cloudRect = oldestCloud.GetComponent<RectTransform>();

//                 totalContentHeight -= (cloudRect.rect.height + layoutInfo.spacing);
//                 oldestCloud.transform.SetParent(null);
//                 Debug.Log("USUNIECIE");
//                 Destroy(oldestCloud);

//                 // Wymuszamy aktualizacj� layoutu, by dane w p�tli by�y poprawne
//                 LayoutRebuilder.ForceRebuildLayoutImmediate(layoutRect);
//             }

//             // Dodatkowe czyszczenie ("na zapas")
//             if (layoutRect.childCount > 0)
//             {
//                 GameObject oldestCloud1 = layoutRect.GetChild(0).gameObject;
//                 RectTransform cloudRect1 = oldestCloud1.GetComponent<RectTransform>();
//                 totalContentHeight -= cloudRect1.rect.height;

//                 oldestCloud1.transform.SetParent(null);
//                 Destroy(oldestCloud1);

//                 // Finalna aktualizacja po usuni�ciu bonusowej chmurki
//                 LayoutRebuilder.ForceRebuildLayoutImmediate(layoutRect);
//             }
//         }

        
//         //int currentChildCount = layoutRect.childCount;
//         //float estimatedHeight = (currentChildCount * chatCloudDimensions.y) + (layoutInfo.spacing * (currentChildCount - 1));

       
//     }
// }

