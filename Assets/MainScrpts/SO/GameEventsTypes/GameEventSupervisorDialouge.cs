using UnityEngine;
[CreateAssetMenu(fileName = "NewSupervisorDialogueEvent", menuName = "Events/Supervisor Dialogue")]
public class GameEventSupervisorDialouge : GameEvent
{
    [Header("Supervisor Dialogue Event Properties")]
    [SerializeField] private string message;
    override public void actionToDo()
    {
        SvDialougeManager.Instance.newDialouge(message);
        IsCompleted = true;
    }
    
}
