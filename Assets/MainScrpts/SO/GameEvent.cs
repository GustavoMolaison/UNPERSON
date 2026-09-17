using System.Collections.Generic;
using UnityEngine;




abstract public class GameEvent : ScriptableObject
{

    [Header("Event Properties")]
    [SerializeField]
    private string eventName;
    [SerializeField]
    
    private EventType eventType;

    [Header("Event Conditions")]
    

    [SerializeReference, SubclassSelector]
     private List<EventCondition> eventConditionsList = new List<EventCondition>();

   

    public bool isCompleted  = false;

    // Pointers //////////////////////
    public string EventName => eventName;
    
    public EventType EventType => eventType;


    
   
    abstract public void actionToDo();


    public void enrollConditions()
    {
        eventConditionsList.ForEach(condition => condition.gameEvent = this);
        eventConditionsList.ForEach(condition => condition.appendToAction()); 
    }


    public GameEvent GameEventRuntime()
    {
        GameEvent gameEventInstance = Instantiate(this);
        gameEventInstance.isCompleted = false;
        return gameEventInstance;
    }

    
}







