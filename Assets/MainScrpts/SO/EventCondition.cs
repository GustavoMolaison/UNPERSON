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
        isConditionMet = Condition();
        if (isConditionMet)
        {
            EventManager.Instance.EnqueueEvent(gameEvent, this);
            deleteFromAction();
        }
        
    }
    
}
