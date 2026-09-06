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
    

    

    public void EnqueueEvent(GameEvent gameEvent)
    {
        
           
           eventQueue.Enqueue(gameEvent);
           StartQueue(); 
        
        
    }
    public void StartQueue()
    {
        // jak chodzi to nie włączamy kolejnej
        if (queueCoroutine == null)
        {
            queueCoroutine = StartCoroutine(ProcessQueueRoutine());
        }
    }

    private IEnumerator ProcessQueueRoutine()
    {
        while (eventQueue.Count > 0)
        {
            GameEvent currentEvent = eventQueue.Dequeue();
            currentEvent.actionToDo();

            // Czeka klatka po klatce aż isCompleted zmieni się na true
            yield return new WaitUntil(() => currentEvent.IsCompleted);
        }

        queueCoroutine = null; 
    }
}
