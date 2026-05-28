using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class DialougeManager : MonoBehaviour
{
    public static DialougeManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private struct ChatRequest
    {
        public List<string> Messages;
        public bool IsPlayerChat;

        public ChatRequest(List<string> messages, bool isPlayerChat)
        {
            Messages = messages;
            IsPlayerChat = isPlayerChat;
        }
    }


    


    private Queue<ChatRequest> chatQueue = new Queue<ChatRequest>();

    private bool isProcessingQueue = false;

    public void chatNewMess(List<string> messages, bool isPlayerChat)
    {
        // Pakujemy dane w paczkê i wrzucamy na koniec kolejki
        chatQueue.Enqueue(new ChatRequest(messages, isPlayerChat));

        // Jeœli system akurat œpi i nic nie robi – odpalamy maszynê przetwarzaj¹c¹ kolejkê
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

    private IEnumerator ProcessQueueRoutine()
    {
        isProcessingQueue = true;

        // Pêtla krêci siê tak d³ugo, jak d³ugo s¹ jakieœ paczki w kolejce
        while (chatQueue.Count > 0)
        {
            // Pobieramy pierwsz¹ paczkê z brzegu i USUWAMY j¹ z kolejki
            ChatRequest currentChat = chatQueue.Dequeue();

            // DOPIERO TUTAJ czyœcimy layout, dok³adnie przed pokazaniem NOWEJ SERII wiadomoœci
            UiDialougeManager.Instance.cleanDialogueLayout(currentChat.IsPlayerChat);

            // S³owo kluczowe: yield return StartCoroutine.
            // Ta korutyna ZATRZYMA SIÊ i poczeka, a¿ ShowMessagesRoutine skoñczy wyœwietlaæ ca³¹ listê!
            yield return StartCoroutine(UiDialougeManager.Instance.ShowMessagesRoutine(currentChat.Messages, currentChat.IsPlayerChat));
        }

        // Kolejka pusta? Maszyna idzie spaæ, czekaj¹c na nowe wywo³ania chatNewMess
        isProcessingQueue = false;
    }
}
