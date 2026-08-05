using TMPro;
using UnityEngine;
using System.Collections.Generic;

// This script is meant to be placed on the parent of the dialogue option windows,
// it will be responsible for creating and turning them off/on when needed
// 
// If 
public class DialogueOptionManager : MonoBehaviour
{
    public static DialogueOptionManager Instance;
    public GameObject dialoguePickWindow;

    

    [HideInInspector] public DialogueOption dialougePicked;
    [SerializeField] private DialogueOption BackOption;

    private List<DialogueOption> prevDialOptions;
    public List<DialogueOption> backDialOptions;
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
    
    public List<DialogueOption> getCurrentDialogueOptions()
    {
        List<DialogueOption> currentOptions = new List<DialogueOption>();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

           if (child.gameObject.activeSelf && child.TryGetComponent<DialogueOptionWindow>(out var window))
              {
               if (window.enrolledDialogue != null)
               {
                currentOptions.Add(window.enrolledDialogue);
               }
              }
        }
        return currentOptions;
    }

    
    private List<string> initializedDialogues = new List<string>();

   
    public void dialoguesChange(bool newDialogueSequence, List<DialogueOption> DialogueSequences = null, bool back = false)
{
    
    

    List<DialogueOption> optionsToLoad = new List<DialogueOption>();

    
    if (back)
    {
      if (backDialOptions != null)
       {
            optionsToLoad.AddRange(backDialOptions);
        }
    }
    else
    {
        
        if (newDialogueSequence && DialogueSequences != null)
        {
            optionsToLoad.AddRange(DialogueSequences);
        }
        else if (prevDialOptions != null)
        {
            optionsToLoad.AddRange(prevDialOptions);
        }
        else if (SuspectTracker.instance.currentSuspect != null)
        {
            optionsToLoad.AddRange(SuspectTracker.instance.currentSuspect.DialogueOptions);
        }
    }

    
    if (BackOption != null)
    {
        optionsToLoad.Add(BackOption);
    }
    // backDialOptions = getCurrentDialogueOptions();
    // hideDialogueOptions();
     
    


    // 2. GŁÓWNA PĘTLA: Jedna logika dla wybranej listy.
    foreach (DialogueOption option in optionsToLoad)
    {
        // Jeśli nie mamy jeszcze tego dialogu
        if (!initializedDialogues.Contains(option.ID))
        {
            
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
prevDialOptions = getCurrentDialogueOptions();
}
}
