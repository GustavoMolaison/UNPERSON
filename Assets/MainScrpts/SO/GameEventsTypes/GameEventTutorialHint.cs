using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "NewTutorialHintEvent", menuName = "Events/Tutorial Hint")]
public class GameEventTutorialHint : GameEvent
{

    private bool disableHintEvent;

    [Header("Type of Hint")]
    [SerializeField] private bool copert;

    [Header("Inputs")]
    [SerializeField] private string text;
    [SerializeField] private LocalizedStringTable textTable;
    [SerializeField] private Image visual;


    [Header("Condition of hint disapearing")]
    [SerializeReference, SubclassSelector]
    private EventCondition hintGoneCondition;

    public void Initialize(bool disableHintEvent, bool copert)
    {
        this.disableHintEvent = disableHintEvent;
        this.copert = copert;
    }




    override public void actionToDo()
    {
        if (copert)
        {
            if (!disableHintEvent)
            {
                TutorialManager.Instance.copertSpaceSetActive(text);
                GameEventTutorialHint disableEvent = ScriptableObject.CreateInstance<GameEventTutorialHint>();
                disableEvent.Initialize(disableHintEvent: true, copert: true);
                hintGoneCondition.gameEvent = disableEvent;
                hintGoneCondition.appendToAction();
            }
            else
            {
                TutorialManager.Instance.copertSpaceSetDisabled();
            }
            
        }
        

    }
}
