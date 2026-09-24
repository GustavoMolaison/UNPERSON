using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;
using static UnityEditor.Searcher.Searcher.AnalyticsEvent;

public enum TutorialType
{
    General,
    Copert
}

[CreateAssetMenu(fileName = "NewTutorialHintEvent", menuName = "Events/Tutorial Hint")]
public class GameEventTutorialHint : GameEvent
{

    private void Awake()
    {
        eventType = EventType.Tutorial;
    }

    private bool eventWentOff = false;

    [SerializeField] private TutorialType tutorialType;



    [Header("Inputs")]
    [SerializeField] private string text;
    [SerializeField] private LocalizedStringTable textTable;
    [SerializeField] private Image visual;
    

    //[Header("Condition of hint disapearing")]
    //[SerializeReference, SubclassSelector]
    //private EventCondition hintGoneCondition;






    override public void actionToDo(EventCondition condition)
    {
        eventType = EventType.Tutorial;

        switch (tutorialType)
    {
        case TutorialType.General:
                TutorialManager.Instance.generalSpaceSetActive(text, () =>
                {
                    isCompleted = true;
                });
                eventWentOff = true;
                break;

         case TutorialType.Copert:
                Debug.Log(isCompleted);
                Debug.Log("isCompleted");

                Debug.Log("Zaczyanm");
                TutorialManager.Instance.copertSpaceSetActive(text, () =>
                {
                    isCompleted = true;
                });
                eventWentOff = true;
                break;
            }
        }
        

           
    public override void actionToDoAtEnd(EventCondition condition)
    {


        switch (tutorialType)
        {
            case TutorialType.General:
                TutorialManager.Instance.generalSpaceSetDisabled();
                eventWentOff = true;
                break;

            case TutorialType.Copert:
                Debug.Log(isCompleted);
                Debug.Log("isCompleted");

                Debug.Log("Zaczyanm");
                TutorialManager.Instance.copertSpaceSetDisabled();
                eventWentOff = true;
                break;
        }
    }
        
        
    
}
