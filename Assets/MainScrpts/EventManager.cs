using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EventType       
        {
            Dialogue,
            
        }


public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private Queue<GameEvent> eventQueue = new Queue<GameEvent>();
    GameEvent currentEvent;
    private Coroutine queueCoroutine;
private bool isProcessing = false;

public void EnqueueEvent(GameEvent gameEvent)
{
    if (gameEvent == null) return;

    eventQueue.Enqueue(gameEvent);
    StartQueue();
}

public void StartQueue()
{
    if (isProcessing) return;

    queueCoroutine = StartCoroutine(ProcessQueueRoutine());
}

private IEnumerator ProcessQueueRoutine()
{
    isProcessing = true;

    while (eventQueue.Count > 0)
    {
        GameEvent currentEvent = eventQueue.Dequeue();

        if (currentEvent == null || currentEvent.IsCompleted)
            continue;

        // Bezpieczne wywołanie - błąd w evencie nie może zablokować całej kolejki
        try
        {
            currentEvent.actionToDo();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Błąd podczas wykonywania eventu: {e.Message}\n{e.StackTrace}");
            continue; 
        }

        // Czekaj na zakończenie
        yield return new WaitUntil(() => currentEvent.IsCompleted);
    }

    // Sprzątanie po zakończeniu pętli
    isProcessing = false;
    queueCoroutine = null;
}

private void OnDisable()
{
    // Reset stanu, jeśli obiekt z jakiegoś powodu zostanie wyłączony w trakcie
    isProcessing = false;
    queueCoroutine = null;
}
}
