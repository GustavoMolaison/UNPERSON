using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EventType       
        {
            Dialogue,
            Tutorial
            
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

public void EnqueueEvent(GameEvent gameEvent, EventCondition condition)
{
    if (gameEvent == null) return;

    eventQueue.Enqueue(gameEvent);
    StartQueue(condition);
}

public void StartQueue(EventCondition condition)
{
        
    //if (isProcessing) return;
       
        queueCoroutine = StartCoroutine(ProcessQueueRoutine(condition));
}

private IEnumerator ProcessQueueRoutine(EventCondition condition)
{
    isProcessing = true;

    while (eventQueue.Count > 0)
    {
            Debug.Log("iteruje wielkosc kolejki:" + eventQueue.Count);
        GameEvent currentEvent = eventQueue.Dequeue();

        if (currentEvent == null)
            {
                Debug.Log("null i hu jhxdxdd");
                continue;
            }
            

        // Bezpieczne wywołanie - błąd w evencie nie może zablokować całej kolejki
        try
        {
                Debug.Log("przsada");
            currentEvent.actionToDo(condition);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Błąd podczas wykonywania eventu: {e.Message}\n{e.StackTrace}");
            continue; 
        }

        // Czekaj na zakończenie
        yield return new WaitUntil(() => currentEvent.isCompleted);
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
