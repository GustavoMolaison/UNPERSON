using UnityEngine;
using UnityEngine.Localization;
[CreateAssetMenu(fileName = "NewSupervisorDialogueEvent", menuName = "Events/Supervisor Dialogue")]
public class GameEventSupervisorDialouge : GameEvent
{
    [Header("Supervisor Dialogue Event Properties")]
    [SerializeField] private LocalizedStringTable dialogueTable;


    private void Awake()
    {
        eventType = EventType.Dialogue;
    }
    override public void actionToDo(EventCondition condition)
    {

        SvDialougeManager.Instance.StartDialogue(dialogueTable, () =>
        {
            isCompleted = true;
        });
    }
        
    
    
}
