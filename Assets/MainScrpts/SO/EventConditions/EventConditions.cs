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
    [SerializeField] List<Suspect> suspectsToCheck;
    

     

    public override bool Condition()
    {
        // W pamięci tworzymy kopie SO suspectów
        // Seriliaze Field bierze ich otyginalne instancje
        // Musimy więc zmapować je na kopie
        var trackerList = SuspectTracker.instance.currentSuspects;

        for (int i = 0; i < suspectsToCheck.Count; i++)
        {
            var target = suspectsToCheck[i];
            if (target == null) continue;

            var match = trackerList.Find(s => s != null && s.FirstName == target.FirstName);
            if (match != null)
            {
                suspectsToCheck[i] = match;
            }
        }

        if (DialogueManager.Instance.isProcessingQueue)
            return false;

        
        if (suspectsToCheck == null || suspectsToCheck.Count == 0)
            return false;

        
        return suspectsToCheck.All(suspect =>
        {
            Debug.Log("DoBTY POCZATEK");
            if (!OptionTreeManager.Instance.ClimbersEveryDialogue.TryGetValue(suspect, out var suspectDialogues) || suspectDialogues == null)
                return false;

            if (allOptions)
            {
                
                return suspectDialogues.Count > 0 && suspectDialogues.All(opt => opt.pickedAtLeastOnce);
            }
            else
            {
                Debug.Log("no dawaj");

                return optionsToCheck.All(targetOpt =>
                    suspectDialogues.Any(d => d.dialogueTitle == targetOpt.dialogueTitle && d.pickedAtLeastOnce));
            }
        });
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
public class CopertOpenedFirstTime : EventCondition
{
    

    public override bool Condition()
    {


        if (EvidenceCopert.Instance.IsFirstOpeningActive)
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
        EvidenceCopert.Instance.onOpeningCopert += CheckCondition;
    }
    public override void deleteFromAction()
    {
        EvidenceCopert.Instance.onOpeningCopert -= CheckCondition;
    }
}

[Serializable]
public class CopertClosedFirstTime : EventCondition
{


    public override bool Condition()
    {


        if (!EvidenceCopert.Instance.hasEverBeenClosed)
        {
            Debug.Log("prawda");
            return true;
        }
        else
        {
            Debug.Log("fałsz");
            return false;
        }
    }
    public override void appendToAction()
    {
        
        EvidenceCopert.Instance.onClosingCopert += CheckCondition;
    }
    public override void deleteFromAction()
    {
        EvidenceCopert.Instance.onClosingCopert -= CheckCondition;
    }
}

[Serializable]
public class EvidenceCheckFirstTime : EventCondition
{


    public override bool Condition()
    {

        
        if (DialogueOptionManager.Instance.firstEvidenceCheckDone)
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
        DialogueOptionManager.Instance.onClickingDialOption += CheckCondition;
    }
    public override void deleteFromAction()
    {
        DialogueOptionManager.Instance.onClickingDialOption -= CheckCondition;
    }
}