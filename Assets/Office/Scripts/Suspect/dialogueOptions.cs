using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Localization;
using System;
[CreateAssetMenu(fileName = "NewDialogue", menuName = "DialogueOption")]
public class DialogueOption : ScriptableObject
{

    // Pole [SerializeField] sprawia, że ID zapisze się w pliku na dysku.
    // Dajemy mu atrybut [ReadOnly] (jeśli masz taki customowy) albo po prostu go nie modyfikujemy.
    [SerializeField] private string uniqueID;

    // Publiczny dostępnik, żeby inne skrypty mogły tylko czytać ID, ale nie nadpisywać.
    public string ID => uniqueID;

    // OnValidate wywołuje się automatycznie w Edytorze Unity:
    // 1. Kiedy tworzysz ten asset.
    // 2. Kiedy zmieniasz w nim jakąś wartość w Inspektorze.
    private void OnValidate()
    {
        // Sprawdzamy, czy ID jest puste (czyli obiekt został dopiero co stworzony)
        if (string.IsNullOrEmpty(uniqueID))
        {
            // Generujemy unikalny ciąg znaków, np. "b4a1b32f-5d6a-4c2e-9d2a-1b2c3d4e5f6a"
            uniqueID = Guid.NewGuid().ToString();
            
            // To mówi Edytorowi Unity: "Hej, zmodyfikowałem ten plik skryptem, zapisz te zmiany na dysk".
            // Bez tego Unity mogłoby zgubić wygenerowane ID po zrestartowaniu edytora.
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }

    [Header("Current Sequence")]
    [SerializeField] private string DialogueName;
    // [SerializeField] private List<DialogueLine> DialogueContent = new List<DialogueLine>();

    [SerializeField] private LocalizedStringTable DialogueTable;

    [Header("New Sequence")]
    [SerializeField] private bool IsNewDialogueSequence;
    
    [SerializeField] private List<DialogueOption> NewDialogueSequence = new List<DialogueOption>();

    [SerializeField] private Evidence EvidenceCheck = null;
    private bool HasEvidenceCheck => EvidenceCheck != null;

    [SerializeField] private Evidence EvidenceCheckDialougeLine = null;
    private bool HasEvidenceCheckDialougeLine => EvidenceCheckDialougeLine != null;

    [SerializeField] private DialogueOption UnlockedDialouge = null;

    [SerializeField] private Evidence EvidenceGained = null;
    private bool HasEvidenceGained => EvidenceGained != null;

    [SerializeField] private Evidence EvidenceToUpdate = null;
    private bool HasEvidenceToUpdate => EvidenceToUpdate != null;

    [SerializeField] private List<Evidence> EvidencesTillHidden = null;
    private bool Ishidden => EvidencesTillHidden.Count > 0;

    [SerializeField] private int EvidenceUpdateIndex = 0;


    [SerializeField] private bool IsBackOption = false;

    [HideInInspector] public DialogueTreeCreator.NodeTree nodeTree;


    



    public string dialogueTitle => DialogueName;
    // public List<DialogueLine> dialogueContent => DialogueContent;
    public LocalizedStringTable dialogueTable => DialogueTable;
    public bool isNewDialogueSequence => IsNewDialogueSequence;
    public List<DialogueOption> newDialogueSequence => NewDialogueSequence;
    public Evidence evidenceCheck => EvidenceCheck;
    public bool hasEvidenceCheck => HasEvidenceCheck;

    public Evidence evidenceCheckDialougeLine => EvidenceCheckDialougeLine;
    public bool hasEvidenceCheckDialougeLine => HasEvidenceCheckDialougeLine;

    public DialogueOption unlockedDialouge => UnlockedDialouge;
    public Evidence evidenceGained => EvidenceGained;
    public bool hasEvidenceGained => HasEvidenceGained;
    public Evidence evidenceToUpdate => EvidenceToUpdate;
    public bool hasEvidenceToUpdate => HasEvidenceToUpdate;
    public int evidenceUpdateIndex => EvidenceUpdateIndex;

    public List<Evidence> evidencesTillHidden => EvidencesTillHidden;
    public bool ishidden => Ishidden;

    public bool isBackOption => IsBackOption;
    
    public DialogueOption dialogueOptionRunTimeInstace()
    {
        // Debug.Log("petla");
        DialogueOption dialOptionCopy = Instantiate(this);
        dialOptionCopy.NewDialogueSequence = new List<DialogueOption>();
        foreach(DialogueOption originalDialOption in this.NewDialogueSequence)
        {
            if(originalDialOption != null)
            {
                dialOptionCopy.NewDialogueSequence.Add(originalDialOption.dialogueOptionRunTimeInstace());
            }
            
        }

        if(this.UnlockedDialouge != null)
        {
            Debug.Log("KTORY TO JUZ DEBUG KURWA");
            dialOptionCopy.UnlockedDialouge = this.UnlockedDialouge.dialogueOptionRunTimeInstace();
        }
        

        return dialOptionCopy;




        
    }
}
