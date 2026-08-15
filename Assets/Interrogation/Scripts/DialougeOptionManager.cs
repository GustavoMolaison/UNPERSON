using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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
    public List<DialogueOption> optionsToLoad = new List<DialogueOption>();

    [HideInInspector] public bool optionAddedOutsideTheLoop = false;
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

        
          if (child.activeSelf)
          {
            DialogueOptionWindow window = child.GetComponent<DialogueOptionWindow>();

            
            if (window != null && window.enrolledDialogue != null)
                {
                 // Sprawdzamy czy to NIE JEST opcja powrotu
                 if (!window.enrolledDialogue.isBackOption) 
                     {
                        disabledOptions.Add(window.enrolledDialogue);
                     }
                }

            child.SetActive(false); 
          }
        }
    }
    
    private List<DialogueOption> getCurrentDialogueOptions()
    {
        List<DialogueOption> currentOptions = new List<DialogueOption>();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

           Debug.Log("dodam tych skurwysynow");
            if (child.gameObject.activeSelf)
            {
                Debug.Log("hm,mm");
            }
            
           if (child.gameObject.activeSelf && child.TryGetComponent<DialogueOptionWindow>(out var window))
              {
                Debug.Log("zaraz dodaje skurwysynow");
               if (window.enrolledDialogue != null)
               {
                Debug.Log("dodaje skurwysynow");
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
        turnOnChossenDialogues(OptionTreeManager.Instance.treeClimbers[SuspectTracker.instance.currentSuspect].startingBranch.branchToOptions());
    }


    public void dialoguesChange2(DialogueOption enrolledDialouge)
    {
        optionsToLoad = OptionTreeManager.Instance.treeClimbers[SuspectTracker.instance.currentSuspect].DecideDirection(enrolledDialouge.nodeTree);
        turnOnChossenDialogues(optionsToLoad);
        currentDialogueOption = enrolledDialouge;
    }


//     public void dialoguesChange(DialogueOption enrolledDialouge)
// {
//     bool newDialogueSequence = enrolledDialouge.isNewDialogueSequence;
//         // List<DialogueOption> DialogueSequences = enrolledDialouge.newDialogueSequence;
//     List<DialogueOption> DialogueSequences = new List<DialogueOption>();
//     if (newDialogueSequence)
//         {
//             List<DialogueOption> dataList = enrolledDialouge.nodeTree.children.Select(child => child.data).ToList();
//             DialogueSequences = dataList;
//         }
        
    
//     bool back = enrolledDialouge.isBackOption;

//     // Jeżeli dodaliśmy opcje dialogowe zpoza standardowych inputów gracza
//     // Naprzykład przez klase DialougeShowup (połączenie dowodu z dialogiem)
//     // Nie resetujemy opcji dialogowych do stworzenia aby nowy dowód dodany zpoza standardowej pętli
//     // Nie zniknął po zresetowaniu listy. W ten sposób odrazu zapisze się w bazie opcji.
//     // Bez tej flagi po kliknieciu nowej opcji odrazu po zakonczeniu dialogu opcja zniknie
//     // i pojawi sie dopiero  po załadowaniu go z parenta czyli kliknięciu back i powrot do
//     // Tego samego stanu w drzewku dialogowym
//     // Ta flaga sprawdza czy takie dodanie miało miejsce
//     if (!optionAddedOutsideTheLoop)
//         {
//              optionsToLoad = new List<DialogueOption>();
//         }
//     // Póżniej jeżeli gracz po odblokowaniu nowej opcji zamiast ją kliknąć wybierze inną opcje która
//     // Zmienia pozycje w drzewku dialogowym aby nie dodawać tej opcji dodanej spoza loopa do 
//     // zlego miejsca w drzewku dialogowym sprawdzamy czy flaga to true jeżeli tak usuwamy nowo dodana opcje
//     // z opcji do zaladowania.
//     // Czyli przypadki w których to sie dzieje, to back == true lub newDialogueSequence == true

//     // Cała ta funkcjonalność naprawia tylko jednego buga ktory nie niszczy w sumie gry ale jest poprostu upierdliwy
    

    
//     if (back)
//     {
//       if (optionAddedOutsideTheLoop)
//         {
//             optionsToLoad = new List<DialogueOption>();
//         }

//       if (currentDialogueOption != null)
//        {
        
           
            
//             // 1  okej więc bierzemy obecne opcje dialogowe
//             // 2 Jeżeli rodzic ich rodzica czyli dziadek
//             // jeśli istnieje to bierzemy jego dzieci i dostaniemy wsszystkie dialogi które powinny być przed aktualnymi tak
//             // 3 DisabledOptions to poprostu opcje dialogowe ktore teraz przy zmianie wylaczoamy czyli 
//             // aktualne opcje dialogowe przed zmiana maja one tych samych starych wszystkie wiec poprostu
//             // to hardcoduje i huj
//             if (disabledOptions[0].nodeTree.parents[0].parents.Count > 0) // okej więc bierzemy obecne opcje dialogowe
//             {
//                 Debug.Log("???????????>");
//                 //sprawdzamy ile dzieci ma dziadek czyli ile pocji dialogowych powinno być przed aktualnymi i je dodajemy do listy
//                 for(int i = 0; i < disabledOptions[0].nodeTree.parents[0].parents[0].children.Count; i++)
//                 {
//                   optionsToLoad.Add(disabledOptions[0].nodeTree.parents[0].parents[0].children[i].data);
//                 }
//                 if (BackOption != null)
//                 {
//                 optionsToLoad.Add(BackOption);
//                 }
//             }
//             // jeżeli rodzic nie ma rodzica to znaczt jest sigma i jest dialogiem rozpoczynajacym wiec na ostro wlaczamy poczatkowe dialogi przypisane do suspecta
//             else
//             {
//                 Debug.Log("!!!!!!!!!!!!>");
//                 for(int i = 0; i < DialogueTreeCreator.Instance.startingNodes[SuspectTracker.instance.currentSuspect].Count; i++)    
//                 {
//                    optionsToLoad.Add(DialogueTreeCreator.Instance.startingNodes[SuspectTracker.instance.currentSuspect][i].data);
//                 }
//             }
            
            
//        }
//     }
//     else
//     {
        
//         // Jeśli dialog prowadzi do nowego drzewka do załadunku dajemy nowe sekwencje
//         if (newDialogueSequence && DialogueSequences != null)
//         {
//             if (optionAddedOutsideTheLoop)
//            {
//             optionsToLoad = new List<DialogueOption>();
//            }

//             optionsToLoad.AddRange(DialogueSequences);
            
//         }
//         // Jeśli nie prowadzi nowych sekwecji to załadowujemy poprzednie opcje zapisane przed zminą (kliknięciem)
//         else if (prevDialOptions != null)
//         {
//             Debug.Log("TO SIE DZIEJE");
//             optionsToLoad.AddRange(prevDialOptions);
//         }
//         // Jeśli nic z tego nie jest prawdziwe to wracamy do startowych kwesti suspecta
//         // To ogólnie nigdy nie powinno sie zdarzać ale jest w razie jakiś problemów aby gra sie cała nie wysypała
//         else if (SuspectTracker.instance.currentSuspect != null)
//         {
//             if (optionAddedOutsideTheLoop)
//            {
//             optionsToLoad = new List<DialogueOption>();
//            }
//             optionsToLoad.AddRange(SuspectTracker.instance.currentSuspect.DialogueOptions);
//         }

//         // Opcje Back dodajemy zawsze
//         if (BackOption != null)
//         {
//         optionsToLoad.Add(BackOption);
//         }
//     }

    
    
//     // Na początku włączamy opcje dialogowe które wybraliśmy w tej funkcji
//     turnOnChossenDialouges(optionsToLoad);

//     // Zapisujemy jakie po zmianie mamy opcje dialogowe
//     // Przy następnej zmiane posłużą jako zapis poprzednich opcji dialogowych
//     prevDialOptions = getCurrentDialogueOptions();
//     // Zapisujemy jaka opcja została ostatnio kliknięta
//     currentDialogueOption = enrolledDialouge;

//     // Jeżeli dodaliśmy opcje dialogowe zpoza standardowych inputów gracza
//     // Naprzykład przez klase DialougeShowup (połączenie dowodu z dialogiem)
//     // Wyłączamy flage, że to się stało na następną zmiane dialogów
//         if (optionAddedOutsideTheLoop)
//         {
//             optionAddedOutsideTheLoop = false;
//         }

// }
public void turnOnChossenDialogues(List<DialogueOption> optionsToLoad)
 {
    
    foreach (DialogueOption option in optionsToLoad)
    {
        
        if(option.ishidden)
            {
                foreach (Evidence evid in option.evidencesTillHidden)
                {
                    if (!GameManager.Instance.evidenceList.Contains(evid))
                    {
                        break;
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
