using UnityEngine;
using System.Collections.Generic;
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
    [SerializeField] private List<DialogueLine> DialogueContent = new List<DialogueLine>();

    [Header("New Sequence")]
    [SerializeField] private bool IsNewDialogueSequence;
    // [SerializeField] private List<string> xd = new List<string>();
    // To chyba powinna byc lista dialougeOption nie stringow
    [SerializeField] private List<DialogueOption> NewDialogueSequence = new List<DialogueOption>();

    public string dialogueTitle => DialogueName;
    public List<DialogueLine> dialogueContent => DialogueContent;
    public bool isNewDialogueSequence => IsNewDialogueSequence;
    public List<DialogueOption> newDialogueSequence => NewDialogueSequence;

    // public List<string> xd1 => xd;
}
