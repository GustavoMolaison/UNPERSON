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
    protected  List<EventCondition> eventConditionsList = new List<EventCondition>();

    [SerializeReference, SubclassSelector]
    protected  List<EventCondition> eventEndConditionsList = new List<EventCondition>();



    public bool isCompleted  = false;

    // Pointers //////////////////////
    public string EventName => eventName;
    
    public EventType EventType => eventType;


    
   
    abstract public void actionToDo(EventCondition condition);

    virtual public void actionToDoAtEnd()
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







