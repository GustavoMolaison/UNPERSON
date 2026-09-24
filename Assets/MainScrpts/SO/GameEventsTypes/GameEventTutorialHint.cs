using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;



[CreateAssetMenu(fileName = "NewTutorialHintEvent", menuName = "Events/Tutorial Hint")]
public class GameEventTutorialHint : GameEvent
{

    private bool eventWentOff = false;

    [Header("Type of Hint")]
    [SerializeField] private bool copert;
    

    [Header("Inputs")]
    [SerializeField] private string text;
    [SerializeField] private LocalizedStringTable textTable;
    [SerializeField] private Image visual;


    //[Header("Condition of hint disapearing")]
    //[SerializeReference, SubclassSelector]
    //private EventCondition hintGoneCondition;






    override public void actionToDo(EventCondition condition)
    {


        if (copert)
        {
            Debug.Log(isCompleted);
            Debug.Log("isCompleted");
            if (eventConditionsList.Contains(condition) && !eventWentOff)
            {
                Debug.Log("Zaczyanm");
                TutorialManager.Instance.copertSpaceSetActive(text, () =>
                {
                    isCompleted = true;
                });
                eventWentOff = true;
            }
            else
            {
                Debug.Log("Koncze");
                if (isCompleted)
                {
                    TutorialManager.Instance.copertSpaceSetDisabled();
                }
                else
                {
                    Debug.Log("Zamykamy przed wykonaniem???");

                }
            }

            //Debug.Log("IM TURNIGN ON");
            //if (copert)
            //{
            //    Debug.Log("Coperta");

            //    if (!eventWentOff)
            //    {
            //        Debug.Log("FIRST GO");
            //        TutorialManager.Instance.copertSpaceSetActive(text);
            //        eventWentOff = true;



            //    }
            //    else
            //    {
            //        Debug.Log("nie jest completed");
            //        if (isCompleted)
            //        {
            //            Debug.Log("wylaczam");
            //            TutorialManager.Instance.copertSpaceSetDisabled();
            //        }

            //    }






        }
    }
}
