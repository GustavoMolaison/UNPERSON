//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//public enum EventType       
//        {
//            Dialogue,
//            Tutorial

//        }


//public class EventManager : MonoBehaviour
//{
//    public static EventManager Instance;

//    void Awake()
//    {
//        if (Instance == null) Instance = this;
//        else Destroy(gameObject);
//    }

//    private Queue<GameEvent> eventQueue = new Queue<GameEvent>();
//    GameEvent currentEvent;
//    private Coroutine queueCoroutine;
//    private bool isProcessing = false;

//public void EnqueueEvent(GameEvent gameEvent, EventCondition condition, bool endAction)
//{
//        if (eventQueue.Count > 0)
//        {
//            Debug.Log("wiecej niz 0");
//            GameEvent currentEvent = eventQueue.Peek();
//            if (!currentEvent.isCompleted && !endAction)
//            {
//                Debug.Log("wiecej niz 1");
//                condition.deleteFromAction();
//            }
//            else if (currentEvent.isCompleted && endAction)
//            {
//                Debug.Log("wiecej niz 2");
//                condition.deleteFromAction();
//            }
//        }



//    if (gameEvent == null) return;
//    Debug.Log("wiecej niz WW");
//    eventQueue.Enqueue(gameEvent);
//    StartQueue(condition, endAction);
//}

//public void StartQueue(EventCondition condition, bool endAction)
//{

//    if (isProcessing) return;
//    Debug.Log("Lets gogo");
//        queueCoroutine = StartCoroutine(ProcessQueueRoutine(condition, endAction));
//}

//    private IEnumerator ProcessQueueRoutine(EventCondition condition, bool endAction)
//    {
//        isProcessing = true;

//        try
//        {
//            while (eventQueue.Count > 0)
//            {
//                GameEvent currentEvent = eventQueue.Dequeue();
//                if (currentEvent == null) continue;

//                bool actionExecuted = false;

//                try
//                {
//                    if (!currentEvent.isCompleted && !endAction)
//                    {
//                        currentEvent.actionToDo(condition);
//                        actionExecuted = true;
//                    }
//                    else if (currentEvent.isCompleted && endAction)
//                    {
//                        currentEvent.actionToDoAtEnd(condition);
//                        actionExecuted = true;
//                    }
//                }
//                catch (System.Exception e)
//                {
//                    Debug.LogError($"Błąd eventu: {e.Message}\n{e.StackTrace}");
//                    continue;
//                }


//                if (actionExecuted && !currentEvent.isCompleted)
//                {
//                    yield return new WaitUntil(() => currentEvent.isCompleted);
//                }
//            }
//        }
//        finally
//        {
//            // To wykona się ZAWSZE, gdy pętla skończy bieg lub rzuci wyjątek
//            Debug.Log("<color=yellow>KONCZE PROCESOWANIE</color>");
//            isProcessing = false;
//            queueCoroutine = null;
//        }
//    }




//private void OnDisable()
//{
//    // Reset stanu, jeśli obiekt z jakiegoś powodu zostanie wyłączony w trakcie
//    isProcessing = false;
//    queueCoroutine = null;
//}
//}



using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    Dialogue,
    Tutorial
}

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    // Struktura wiążąca event z jego parametrami wywołania
    private struct QueuedEventItem
    {
        public GameEvent Event;
        public EventCondition Condition;
        public bool EndAction;

        public QueuedEventItem(GameEvent gameEvent, EventCondition condition, bool endAction)
        {
            Event = gameEvent;
            Condition = condition;
            EndAction = endAction;
        }
    }

    // Klasa zarządzająca pojedynczą kolejką dla danego typu
    private class Channel
    {
        public Queue<QueuedEventItem> Queue = new Queue<QueuedEventItem>();
        public Coroutine Coroutine;
        public bool IsProcessing => Coroutine != null;
    }

    private readonly Dictionary<EventType, Channel> channels = new Dictionary<EventType, Channel>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Inicjalizacja kanałów dla każdego enuma
        foreach (EventType type in Enum.GetValues(typeof(EventType)))
        {
            channels[type] = new Channel();
        }
    }

    public void EnqueueEvent(EventType type, GameEvent gameEvent, EventCondition condition, bool endAction)
    {
        if (gameEvent == null) return;

        if (!channels.TryGetValue(type, out Channel channel))
        {
            Debug.LogError($"Brak zarejestrowanego kanału dla typu: {type}");
            return;
        }

        // Sprawdzanie aktualnie wiszącego na czele kolejki eventu
        if (channel.Queue.Count > 0)
        {
            QueuedEventItem frontItem = channel.Queue.Peek();
            if (!frontItem.Event.isCompleted && !endAction)
            {
                condition?.deleteFromAction();
            }
            else if (frontItem.Event.isCompleted && endAction)
            {
                condition?.deleteFromAction();
            }
        }

        // Pakujemy wszystko razem, żeby późniejsze eventy nie zgubiły swoich parametrów
        channel.Queue.Enqueue(new QueuedEventItem(gameEvent, condition, endAction));

        if (!channel.IsProcessing)
        {
            channel.Coroutine = StartCoroutine(ProcessQueueRoutine(type, channel));
        }
    }

    private IEnumerator ProcessQueueRoutine(EventType type, Channel channel)
    {
        try
        {
            while (channel.Queue.Count > 0)
            {
                QueuedEventItem item = channel.Queue.Dequeue();
                GameEvent currentEvent = item.Event;

                if (currentEvent == null) continue;

                bool actionExecuted = false;

                try
                {
                    if (!currentEvent.isCompleted && !item.EndAction)
                    {
                        currentEvent.actionToDo(item.Condition);
                        actionExecuted = true;
                    }
                    else if (currentEvent.isCompleted && item.EndAction)
                    {
                        currentEvent.actionToDoAtEnd(item.Condition);
                        actionExecuted = true;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"[EventManager - {type}] Błąd wykonania eventu: {e.Message}\n{e.StackTrace}");
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
            // Resetujemy korutynę kanału – pozwala to na ponowne uruchomienie kolejki przy kolejnym Enqueue
            channel.Coroutine = null;
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        foreach (var channel in channels.Values)
        {
            channel.Coroutine = null;
            channel.Queue.Clear();
        }
    }
}