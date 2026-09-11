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

    [Header("Suspect is interrogated conditions")]
    [Tooltip("If true, the event will start when the chosen suspect is interrogated")]
    [SerializeField]
    private bool suspectInterrogated = false;
    [SerializeField]
    private string suspectInterrogatedName;
    
    [Header("Dialouge Option picked conditions")]
    [Tooltip("If true, the event will start when the chosen Dialouge Option is picked and finished playing")]

    [SerializeField]
    private bool dialougeOptionPicked = false;
    [SerializeField]
    private DialogueOption dialougeOptionPickedTarget;
    [Space(15)]
    [SerializeField]
    [Tooltip("If true, the event will start when the chosen Dialouge Option is clicked and its dialouge wont play instantly")]
    private bool dialougeOptionClicked = false;

    [SerializeField]
    private  DialogueOption dialougeOptionClickedTarget;

    



    [Header("Observed Components")]
    [Tooltip("Components what will be observed and used for conditions")]
    
    [SerializeField]
    private TypewriterEffect typeWriterEffect;

    private bool isCompleted = false;



    // Pointers //////////////////////
    public string EventName => eventName;
    
    public EventType EventType => eventType;
    public bool IsCompleted
    {
        get => isCompleted;
        set => isCompleted = value;
    }
    // public bool Timer => timer;


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
        if (suspectInterrogated)
        {
            if (SuspectTracker.instance.currentSuspect != null && InterrogationManager.Instance.interrogatedSuspect.FirstName == suspectInterrogatedName)
            {
                suspectInterrogated = false;
                return true;
            }
            else
            {
                return false;
            }
        }
        if(dialougeOptionPicked && typeWriterEffect!= null)
        {
            
            
                if(typeWriterEffect.IsTyping == false)
                {
                    if (DialogueOptionManager.Instance.currentDialogueOption == dialougeOptionPickedTarget)
                    {
                       dialougeOptionPicked = false;
                        return true;
                    }
                    else
                    {       
                        return false;
                    }
                }
            
            
            
        }
        
        if(dialougeOptionPicked && !DialogueManager.Instance.isProcessingQueue && typeWriterEffect == null)
        {
             
             if (DialogueOptionManager.Instance.currentDialogueOption != null && DialogueOptionManager.Instance.currentDialogueOption.dialogueTitle == dialougeOptionPickedTarget.dialogueTitle)
                    {
                       dialougeOptionPicked = false;
                        return true;
                    }
                    else
                    {       
                        return false;
                    }
        }
        if(dialougeOptionClicked && !DialogueManager.Instance.isProcessingQueue && typeWriterEffect == null)
        {
           
            if (DialogueOptionManager.Instance.dialougePicked != null && DialogueOptionManager.Instance.dialougePicked.dialogueTitle == dialougeOptionClickedTarget.dialogueTitle)
            {
               
                
                dialougeOptionClicked = false;
                return true;
            }
            else
            {
                return false;
            }
        }
        
           
        
        
        
        return false;
        
    }


    public GameEvent GameEventRuntime()
    {
        GameEvent gameEventInstance = Instantiate(this);
        gameEventInstance.isCompleted = false;
        return gameEventInstance;
    }

    
}
