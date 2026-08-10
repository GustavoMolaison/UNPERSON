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

    private DialogueOption currentDialogueOption;

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
    
    private List<DialogueOption> disabledOptions = new List<DialogueOption>();
    public void hideDialogueOptions()
    {
        disabledOptions.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;

        // Sprawdzamy, czy obiekt jest aktualnie aktywny
          if (child.activeSelf)
          {
            DialogueOptionWindow window = child.GetComponent<DialogueOptionWindow>();

            // Zawsze sprawdzaj czy komponent istnieje, żeby nie dostać NullReferenceException
            if (window != null && window.enrolledDialogue != null)
                {
                 // Sprawdzamy czy to NIE JEST opcja powrotu
                 if (!window.enrolledDialogue.isBackOption) 
                     {
                        disabledOptions.Add(window.enrolledDialogue);
                     }
                }

            child.SetActive(false); // Wyłączasz go po ogarnięciu logiki
          }
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

    public void initilalizeSuspectOptions()
    {
        hideDialogueOptions();
        turnOnChossenDialouges(SuspectTracker.instance.currentSuspect.DialogueOptions);
    }

    public void dialoguesChange(DialogueOption enrolledDialouge)
{
    bool newDialogueSequence = enrolledDialouge.isNewDialogueSequence;
    List<DialogueOption> DialogueSequences = enrolledDialouge.newDialogueSequence;
    bool back = enrolledDialouge.isBackOption;


    List<DialogueOption> optionsToLoad = new List<DialogueOption>();

    
    if (back)
    {
      if (currentDialogueOption != null)
       {
           
            
            // 1  okej więc bierzemy obecne opcje dialogowe
            // 2 Jeżeli rodzic ich rodzica czyli dziadek
            // jeśli istnieje to bierzemy jego dzieci i dostaniemy wsszystkie dialogi które powinny być przed aktualnymi tak
            // 3 DisabledOptions to poprostu opcje dialogowe ktore teraz przy zmianie wylaczoamy czyli 
            // aktualne opcje dialogowe przed zmiana maja one tych samych starych wszystkie wiec poprostu
            // to hardcoduje i huj
            if (disabledOptions[0].nodeTree.parents[0].parents.Count > 0) // okej więc bierzemy obecne opcje dialogowe
            {
                //sprawdzamy ile dzieci ma dziadek czyli ile pocji dialogowych powinno być przed aktualnymi i je dodajemy do listy
                for(int i = 0; i < disabledOptions[0].nodeTree.parents[0].parents[0].children.Count; i++)
                {
                  optionsToLoad.Add(disabledOptions[0].nodeTree.parents[0].parents[0].children[i].data);
                }
                if (BackOption != null)
                {
                optionsToLoad.Add(BackOption);
                }
            }
            // jeżeli rodzic nie ma rodzica to znaczt jest sigma i jest dialogiem rozpoczynajacym wiec na ostro wlaczamy poczatkowe dialogi przypisane do suspecta
            else
            {
                for(int i = 0; i < DialogueTreeCreator.Instance.startingNodes[SuspectTracker.instance.currentSuspect].Count; i++)    
                {
                   optionsToLoad.Add(DialogueTreeCreator.Instance.startingNodes[SuspectTracker.instance.currentSuspect][i].data);
                }
            }
            
            
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

         if (BackOption != null)
        {
        optionsToLoad.Add(BackOption);
        }
    }

    
    

    turnOnChossenDialouges(optionsToLoad);

    prevDialOptions = getCurrentDialogueOptions();
    currentDialogueOption = enrolledDialouge;

}
public void turnOnChossenDialouges(List<DialogueOption> optionsToLoad)
    {
    foreach (DialogueOption option in optionsToLoad)
    {
        
        if(option.ishidden)
            {
                foreach (Evidence evid in option.evidencesTillHidden)
                {
                    if (!GameManager.Instance.evidenceList.Contains(evid))
                    {
                        continue;
                    }
                }
            }
            
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
    }
}
