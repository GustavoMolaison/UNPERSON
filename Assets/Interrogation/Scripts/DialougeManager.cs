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
        public List<DialogueLine> Messages;
        

        public ChatRequest(List<DialogueLine> messages)
        {
            Messages = messages;
            
        }
    }


    


    private Queue<ChatRequest> chatQueue = new Queue<ChatRequest>();

    [HideInInspector] public bool isProcessingQueue = false;

    public void chatNewMess(List<DialogueLine> messages)
    {
        // Pakujemy dane w paczk� i wrzucamy na koniec kolejki
        chatQueue.Enqueue(new ChatRequest(messages));

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
        ConversationManager.Instance.chatNewMess(enrolledDialouge.dialougeContent); //THIS FRIST
        DialougeOptionManager.Instance.cleanDialogueOptions(); // THIS SECOND
        yield return new WaitUntil(() => isProcessingQueue == false); // THIS THIRD

        // ConversationManager.Instance.chatNewMess(enrolledDialouge.dialougeContent);
        if(enrolledDialouge.isNewDialogueSequence)
        {
            DialougeOptionManager.Instance.dialougesChange(true, enrolledDialouge.newdialogueSequence);
        }
        else
        {
            DialougeOptionManager.Instance.dialougesChange(false);
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
            // Ta korutyna ZATRZYMA SI� i poczeka, a� ShowMessagesRoutine sko�czy wy�wietla� ca�� list�!
            yield return StartCoroutine(UiDialougeManager.Instance.ShowMessagesRoutine(currentChat.Messages));
            
        }

        // Kolejka pusta? Maszyna idzie spa�, czekaj�c na nowe wywo�ania chatNewMess
        isProcessingQueue = false;
    }
}
