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

public void EnqueueEvent(GameEvent gameEvent, EventCondition condition, bool endAction)
{
    if (gameEvent == null) return;

    eventQueue.Enqueue(gameEvent);
    StartQueue(condition, endAction);
}

public void StartQueue(EventCondition condition, bool endAction)
{
        
    //if (isProcessing) return;
       
        queueCoroutine = StartCoroutine(ProcessQueueRoutine(condition, endAction));
}

private IEnumerator ProcessQueueRoutine(EventCondition condition, bool endAction)
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
                
                if (!currentEvent.isCompleted && !endAction)
                {
                    currentEvent.actionToDo(condition);
                    condition.deleteFromAction();
                }
                else if(currentEvent.isCompleted && endAction)
                {
                    currentEvent.actionToDoAtEnd(condition);
                    condition.deleteFromAction();
                }
            
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
