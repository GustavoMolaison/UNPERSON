using UnityEngine;
using UnityEngine.Localization;
[CreateAssetMenu(fileName = "NewSupervisorDialogueEvent", menuName = "Events/Supervisor Dialogue")]
public class GameEventSupervisorDialouge : GameEvent
{
    [Header("Supervisor Dialogue Event Properties")]
    [SerializeField] private LocalizedStringTable dialogueTable;
    override public void actionToDo()
    {
        SvDialougeManager.Instance.newDialouge(dialogueTable);
        IsCompleted = true;
    }
    
}
