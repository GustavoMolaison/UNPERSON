using System.Collections.Generic;
using UnityEngine;




abstract public class GameEvent : ScriptableObject
{

    [Header("Event Properties")]
    [SerializeField]
    private string eventName;
    
    
    protected EventType eventType;

    [Header("Event Conditions")]
    

    [SerializeReference, SubclassSelector]
    public  List<EventCondition> eventConditionsList  = new List<EventCondition>();

    [SerializeReference, SubclassSelector]
    public  List<EventCondition> eventEndConditionsList = new List<EventCondition>();


    private bool eventWentOff = false;
    public bool isCompleted  = false;

    // Pointers //////////////////////
    public string EventName => eventName;
    
    public EventType EventType => eventType;


    
   
    abstract public void actionToDo(EventCondition condition);

    virtual public void actionToDoAtEnd(EventCondition condition)
    {

    }


    public void enrollConditions()
    {
        if (eventType == EventType.Dialogue)
        {
            eventConditionsList.ForEach(condition => condition.gameEvent = this);
            eventConditionsList.ForEach(condition => condition.appendToAction());
        }
        if (eventType == EventType.Tutorial)
        {
            eventConditionsList.ForEach(condition => condition.gameEvent = this);
            eventConditionsList.ForEach(condition => condition.appendToAction());

            eventEndConditionsList.ForEach(condition => condition.gameEvent = this);
            eventEndConditionsList.ForEach(condition => condition.appendToAction());
        }
    }


    public GameEvent GameEventRuntime()
    {
        GameEvent gameEventInstance = Instantiate(this);
        gameEventInstance.isCompleted = false;
        return gameEventInstance;
    }

    
}







