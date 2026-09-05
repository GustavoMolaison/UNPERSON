using UnityEngine;

abstract public class GameEvent : ScriptableObject
{

    [Header("Event Properties")]
    [SerializeField]
    private string eventName;
    [SerializeField]
    
    private EventType eventType;

    [Header("Event Conditions")]
    [SerializeField]
    private bool start = false;


    [SerializeField]
    private bool isCompleted = false;

    public string EventName => eventName;
    
    public EventType EventType => eventType;
    public bool IsCompleted
    {
        get => isCompleted;
        set => isCompleted = value;
    }
    public bool Start => start;
    

    abstract public void actionToDo();

    
}
