using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    
    public static DialogueManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private struct ChatRequest
    {
        public DialogueOption DialOption;
        

        public ChatRequest(DialogueOption dialOption)
        {
            DialOption = dialOption;
            
        }
    }


    


    private Queue<ChatRequest> chatQueue = new Queue<ChatRequest>();

    [HideInInspector] public bool isProcessingQueue = false;

    public void chatNewMess(DialogueOption dialOption)
    {
        // Pakujemy dane w paczk� i wrzucamy na koniec kolejki
        chatQueue.Enqueue(new ChatRequest(dialOption));

        // Je�li system akurat �pi i nic nie robi � odpalamy maszyn� przetwarzaj�c� kolejk�
        if (!isProcessingQueue)
        {
            Debug.Log("Kolejka czysta");
            StartCoroutine(ProcessQueueRoutine());
        }
        else
        {
            Debug.Log("Kolejka zajeta");

        }
    }

    // public void dialogueOptionClicked(DialogueOption enrolledDialouge)
    // {
    //     StartCoroutine(onClickWait(enrolledDialouge));
    // }

    public IEnumerator dialogueOptionClicked(DialogueOption enrolledDialouge)
    {
        ConversationManager.Instance.chatNewMess(enrolledDialouge); //THIS FRIST
        DialogueOptionManager.Instance.hideDialogueOptions(); // THIS SECOND
        yield return new WaitUntil(() => isProcessingQueue == false); // THIS THIRD

        // ConversationManager.Instance.chatNewMess(enrolledDialouge.dialogueContent);
        if(enrolledDialouge.isNewDialogueSequence)
        {
            DialogueOptionManager.Instance.dialoguesChange(true, enrolledDialouge.newDialogueSequence);
        }
        else
        {
            DialogueOptionManager.Instance.dialoguesChange(false);
        }
    } 

    private IEnumerator ProcessQueueRoutine()
    {
        isProcessingQueue = true;

        // P�tla kr�ci si� tak d�ugo, jak d�ugo s� jakie� paczki w kolejce
        while (chatQueue.Count > 0)
        {
            // Pobieramy pierwsz� paczk� z brzegu i USUWAMY j� z kolejki
            ChatRequest currentChat = chatQueue.Dequeue();

            // DOPIERO TUTAJ czy�cimy layout, dok�adnie przed pokazaniem NOWEJ SERII wiadomo�ci
            // UiDialougeManager.Instance.cleanDialogueLayout(currentChat.IsPlayerChat);

            // S�owo kluczowe: yield return StartCoroutine. 
            // Ta korutyna ZATRZYMA SIĘ i poczeka, aż ShowMessagesRoutine sko�czy wy�wietla� ca�� list�!
            yield return StartCoroutine(UiDialougeManager.Instance.ShowMessagesRoutine(currentChat.DialOption.dialogueTable));
            
            // Dodawanie dowodów po rozmowie jeżeli flaga to true
            if (currentChat.DialOption.hasEvidenceGained)
            {
                Debug.Log("Dodaje dowod: " + currentChat.DialOption.evidenceGained.name);
                Case_Monitor.Instance.addEvidence(currentChat.DialOption.evidenceGained);
            }
            
        }

        // Kolejka pusta? Maszyna idzie spa, czekajc na nowe wywoania chatNewMess
        isProcessingQueue = false;
    }
}
