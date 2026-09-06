using UnityEngine;

abstract public class GameEvent : ScriptableObject
{

    [Header("Event Properties")]
    [SerializeField]
    private string eventName;
    [SerializeField]
    
    private EventType eventType;

    [Header("Event Conditions")]

    [Header("Timer conditions")]
    [Tooltip("If true, the event will start when the game time is less than TimeToStart variable")]
    [SerializeField]
    private bool timer = false;
    [SerializeField]
    private float timeToStart = 5f;


    
    private bool isCompleted = false;



    // Pointers //////////////////////
    public string EventName => eventName;
    
    public EventType EventType => eventType;
    public bool IsCompleted
    {
        get => isCompleted;
        set => isCompleted = value;
    }
    public bool Timer => timer;
    
    // Pointers  END////////////////////// 

    abstract public void actionToDo();


    public bool areConditionsMet()
    {
       
        if (timer)
        {
            
            if(Time.time >= timeToStart)
            {
                
                timer = false;
                return true;
            }
            else
            {
                return false;
            }
            
    
        }
        else
        {
            return false;
        }
    }


    public GameEvent GameEventRuntime()
    {
        GameEvent gameEventInstance = Instantiate(this);
        gameEventInstance.isCompleted = false;
        return gameEventInstance;
    }

    
}
