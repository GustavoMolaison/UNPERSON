using TMPro;
using UnityEngine;
using System.Collections.Generic;

// This script is meant to be placed on the parent of the dialogue option windows,
// it will be responsible for creating and deleting them when needed
// I think so idk i write this weeks after writing it
public class DialogueOptionManager : MonoBehaviour
{
    public static DialogueOptionManager Instance;
    public GameObject dialoguePickWindow;
    

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

   
    public void cleanDialogueOptions()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public void hideDialogueOptions()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    
    private List<string> initializedDialogues = new List<string>();

   
    public void dialoguesChange(bool newDialogueSequence, List<DialogueOption> DialogueSequences = null)
    {
    hideDialogueOptions();
    Debug.Log("Zmieniam dialogi");

    List<DialogueOption> optionsToLoad;

    // 1. DECYZJA: Wybieramy listę do załadowania tylko RAZ.
    if (newDialogueSequence && DialogueSequences != null)
    {
        optionsToLoad = DialogueSequences;
    }
    else
    {
        optionsToLoad = SuspectTracker.instance.currentSuspect.DialogueOptions;
    }

     

    // 2. GŁÓWNA PĘTLA: Jedna logika dla wybranej listy.
    foreach (DialogueOption option in optionsToLoad)
    {
        // Jeśli nie mamy jeszcze tego dialogu
        if (!initializedDialogues.Contains(option.ID))
        {
            Debug.Log("NO ID");
            GameObject window = Instantiate(dialoguePickWindow, transform, false);
            DialogueOptionWindow windowScript = window.GetComponent<DialogueOptionWindow>();
            
            windowScript.enrollDialogue(option);
            initializedDialogues.Add(option.ID);
        }
        else
        {
            // Jeśli mamy, szukamy go w dzieciach
            Debug.Log("yes ID");
            foreach (Transform child in transform)
            {
                // Pobieramy komponent raz na iterację
                DialogueOptionWindow windowScript = child.GetComponent<DialogueOptionWindow>();
                
                if (windowScript != null && windowScript.enrolledDialogue.ID == option.ID)
                {
                    child.gameObject.SetActive(true);
                    break; // Znaleźliśmy odpowiednie okno, przerywamy pętlę wewnętrzną, idziemy do kolejnej opcji!
                }
            }
        }
    }
}
}
