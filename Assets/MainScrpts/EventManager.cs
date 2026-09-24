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
        if (eventQueue.Count > 0)
        {
            Debug.Log("wiecej niz 0");
            GameEvent currentEvent = eventQueue.Peek();
            if (!currentEvent.isCompleted && !endAction)
            {
                Debug.Log("wiecej niz 1");
                condition.deleteFromAction();
            }
            else if (currentEvent.isCompleted && endAction)
            {
                Debug.Log("wiecej niz 2");
                condition.deleteFromAction();
            }
        }
        
        

    if (gameEvent == null) return;
    Debug.Log("wiecej niz WW");
    eventQueue.Enqueue(gameEvent);
    StartQueue(condition, endAction);
}

public void StartQueue(EventCondition condition, bool endAction)
{
        
    if (isProcessing) return;
    Debug.Log("Lets gogo");
        queueCoroutine = StartCoroutine(ProcessQueueRoutine(condition, endAction));
}

    private IEnumerator ProcessQueueRoutine(EventCondition condition, bool endAction)
    {
        isProcessing = true;

        try
        {
            while (eventQueue.Count > 0)
            {
                GameEvent currentEvent = eventQueue.Dequeue();
                if (currentEvent == null) continue;

                bool actionExecuted = false;

                try
                {
                    if (!currentEvent.isCompleted && !endAction)
                    {
                        currentEvent.actionToDo(condition);
                        actionExecuted = true;
                    }
                    else if (currentEvent.isCompleted && endAction)
                    {
                        currentEvent.actionToDoAtEnd(condition);
                        actionExecuted = true;
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Błąd eventu: {e.Message}\n{e.StackTrace}");
                    continue;
                }

                
                if (actionExecuted && !currentEvent.isCompleted)
                {
                    yield return new WaitUntil(() => currentEvent.isCompleted);
                }
            }
        }
        finally
        {
            // To wykona się ZAWSZE, gdy pętla skończy bieg lub rzuci wyjątek
            Debug.Log("<color=yellow>KONCZE PROCESOWANIE</color>");
            isProcessing = false;
            queueCoroutine = null;
        }
    }
    
    


private void OnDisable()
{
    // Reset stanu, jeśli obiekt z jakiegoś powodu zostanie wyłączony w trakcie
    isProcessing = false;
    queueCoroutine = null;
}
}
