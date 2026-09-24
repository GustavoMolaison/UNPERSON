using UnityEngine;
using System;

[Serializable]
public abstract class EventCondition 
{
    public bool isConditionMet { get; private set; }
    public GameEvent gameEvent { get; set; }
    public void SetConditionMet(bool met)
    {
        isConditionMet = met;
    }

    abstract public void appendToAction();
    abstract public void deleteFromAction();

    abstract public bool Condition();

    public void CheckCondition()
    {
        bool endAction;
        isConditionMet = Condition();
        if (gameEvent.eventConditionsList.Contains(this))
        {
            endAction = false;
        }
        else
        {
            endAction = true;
        }


        if (isConditionMet)
        {
            EventManager.Instance.EnqueueEvent(gameEvent, this, endAction);

            //deleteFromAction();
        }
        
    }
    
}
