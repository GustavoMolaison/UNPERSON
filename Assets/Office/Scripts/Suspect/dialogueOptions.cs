using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "NewDialouge", menuName = "Dialouge")]
public class dialogueOption : ScriptableObject
{

    [Header("Current Sequence")]
    [SerializeField] private string DialougeName;
    [SerializeField] private List<DialogueLine> DialougeContent = new List<DialogueLine>();

    [Header("New Sequence")]
    [SerializeField] private bool IsNewDialogueSequence;
    // [SerializeField] private List<string> xd = new List<string>();
    // To chyba powinna byc lista dialougeOption nie stringow
    [SerializeField] private List<string> NewDialogueSequence = new List<string>();

    public string dialougeTittle => DialougeName;
    public List<DialogueLine> dialougeContent => DialougeContent;
    public bool isNewDialogueSequence => isNewDialogueSequence;
    public List<string> newdialogueSequence => NewDialogueSequence;

    // public List<string> xd1 => xd;
}
