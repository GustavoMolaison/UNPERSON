using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "NewDialouge", menuName = "Dialouge")]
public class dialogueOption : ScriptableObject
{

    [Header("Current Sequence")]
    [SerializeField] private string DialougeName;
    [SerializeField] private List<string> DialougeContent = new List<string>();
    [Header("New Sequence")]
    [SerializeField] private bool IsNewDialogueSequence;
    [SerializeField] private List<string> NewDialogueSequence = new List<string>();

    public string dialougeTittle => DialougeName;
    public List<string> dialougeContent => DialougeContent;
    public bool isNewDialogueSequence => isNewDialogueSequence;
    public List<string> newdialogueSequence => NewDialogueSequence;
}
