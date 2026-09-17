using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;


[AttributeUsage(AttributeTargets.Field)]
public class SubclassSelectorAttribute : PropertyAttribute { }





[Serializable]
public class EventConditionTimer : EventCondition
{
    [SerializeField] float timeToStart = 5f;
    
    public override bool Condition()
    {
        if(Time.time >= timeToStart)
            {
                
                
                return true;
            }
            else
            {
                return false;
            }
        
    }
    public override void appendToAction()
    {
        GameManager.Instance.actionPerFrame += CheckCondition;
    }
    public override void deleteFromAction()
    {
        GameManager.Instance.actionPerFrame -= CheckCondition;
    }
}


[Serializable]
public class EventConditionSuspectInterrogated : EventCondition
{
    [SerializeField] string suspectInterrogatedName;
    
    public override bool Condition()
    {
        

        if (SuspectTracker.instance.currentSuspect != null && InterrogationManager.Instance.interrogatedSuspect.FirstName == suspectInterrogatedName)
            {
                
                return true;
            }
            else
            {
                return false;
            }
    }
    public override void appendToAction()
    {
        InterrogationManager.Instance.OnInterrogatedSuspectChanged += CheckCondition;
    }
    public override void deleteFromAction()
    {
        InterrogationManager.Instance.OnInterrogatedSuspectChanged -= CheckCondition;
    }
}

[Serializable]
public class EventConditionDialogueOptionPicked : EventCondition
{
    [SerializeField] private List<DialogueOption> dialogueOptionsPickedTargets = new List<DialogueOption>();
    [SerializeField] private TypewriterEffect typeWriterEffect;

    public override bool Condition()
    {
        // 1. Sprawdź blokady - jeśli cokolwiek pisze/przetwarza, od razu ucinamy
        if (typeWriterEffect != null && typeWriterEffect.IsTyping)
        {
            return false;
        }

        if (typeWriterEffect == null && DialogueManager.Instance.isProcessingQueue)
        {
            return false;
        }

        var currentOption = DialogueOptionManager.Instance.currentDialogueOption;
        if (currentOption == null)
        {
            return false;
        }

        
        foreach (var target in dialogueOptionsPickedTargets)
        {
            if (target == null) continue;

            if (typeWriterEffect != null)
            {
                
                if (currentOption.dialogueTitle == target.dialogueTitle)
                {
                    return true;
                }
            }
            else
            {
               
                if (currentOption.dialogueTitle == target.dialogueTitle)
                {
                    return true;
                }
            }
        }

        return false;
    }


    public override void appendToAction()
        {
            DialogueOptionManager.Instance.OnCurrentDialogueOptionChanged += CheckCondition;
        }
    public override void deleteFromAction()
        {
            DialogueOptionManager.Instance.OnCurrentDialogueOptionChanged -= CheckCondition;
        }
    }

[Serializable]
    public class EventConditionDialogueOptionClicked : EventCondition
    {
        [SerializeField] DialogueOption dialougeOptionClickedTarget;
        [SerializeField] TypewriterEffect typeWriterEffect;
        
        public override bool Condition()
        {
            if(typeWriterEffect!= null)
            {
                if(typeWriterEffect.IsTyping == false)
               {
                    if (DialogueOptionManager.Instance.currentDialogueOption == dialougeOptionClickedTarget && !DialogueManager.Instance.isProcessingQueue)
                    {
                        
                        return true;
                    }
                    else
                    {       
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else 
            {
                if(!DialogueManager.Instance.isProcessingQueue && DialogueOptionManager.Instance.dialougueClicked != null && DialogueOptionManager.Instance.dialougueClicked.dialogueTitle == dialougeOptionClickedTarget.dialogueTitle)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public override void appendToAction()
        {
            DialogueOptionManager.Instance.OnCurrentDialogueClickedChanged += CheckCondition;
        }
        public override void deleteFromAction()
        {
            DialogueOptionManager.Instance.OnCurrentDialogueClickedChanged -= CheckCondition;
        }
    }
[Serializable]
    public class EventConditionDialogueOptionsPickedAtLeastOnce : EventCondition
{
        [SerializeField] bool allOptions = false;
        [SerializeField] List<DialogueOption> optionsToCheck;

    public override bool Condition()
    {
        
        var currentDialogues = OptionTreeManager.Instance.ClimbersEveryDialouge[SuspectTracker.instance.currentSuspect];
        bool allMatch = optionsToCheck.All(opt => currentDialogues.Any(d => d.dialogueTitle == opt.dialogueTitle));
       
        if(allOptions)
        {
            Branch branch =  DialogueTreeCreator.Instance.startingNodes[SuspectTracker.instance.currentSuspect];
            List<DialogueOption> allOptions = OptionTreeManager.Instance.ClimbersEveryDialouge[SuspectTracker.instance.currentSuspect];
            if (!DialogueManager.Instance.isProcessingQueue && allOptions.All(opt => opt.pickedAtLeastOnce))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (!DialogueManager.Instance.isProcessingQueue && allMatch)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
       
    }
    public override void appendToAction()
        {
            DialogueOptionManager.Instance.OnCurrentDialogueOptionChanged += CheckCondition;
        }
    public override void deleteFromAction()
        {
            DialogueOptionManager.Instance.OnCurrentDialogueOptionChanged -= CheckCondition;
        }
}

    

