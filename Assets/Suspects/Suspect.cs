using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Suspect", menuName = "Scriptable Objects/Suspect")]
public class Suspect : ScriptableObject
{
    [Header("Identity")]
    [SerializeField] private string firstName;
    [SerializeField] private string lastName;
    [SerializeField] private string birthDate;
    [SerializeField] private int age;
    [SerializeField] private string occupation;

    public enum GenderType { Male, Female, Other }
    [SerializeField] private GenderType gender;
    [SerializeField] private Sprite face;
    [SerializeField] private Sprite face_interrogation;

    [Header("Evidence & Logs")]
    [SerializeField] private List<Conversation> chatHistory = new List<Conversation>();
    [SerializeField] private List<DialogueOption> dialogueOptions = new List<DialogueOption>();


    [Header("Who")]
    [SerializeField] private SuspGuees role;    
    public string FirstName => firstName;
    public string LastName => lastName;
    public string FullName => $"{firstName} {lastName}";
    public string BirthDate => birthDate;
    public int Age => age;
    public string Occupation => occupation;
    public GenderType Gender => gender;
    public Sprite Face => face;
    public Sprite Face_interrogation => face_interrogation;
    public List<Conversation> ChatHistory => chatHistory;

    public List<DialogueOption> DialogueOptions => dialogueOptions;
    public SuspGuees Role => role;

    public Suspect CreateRuntimeInstance()
    {
        // 1. Klonujemy samego Suspecta
        Suspect suspectInstance = Instantiate(this);

        // 2. Tworzymy nową listę na sklonowane SO
        suspectInstance.dialogueOptions = new List<DialogueOption>();

        // 3. Klonujemy każdy zagnieżdżony DialogueOption osobno
        foreach (var originalOption in this.dialogueOptions)
        {
            if (originalOption != null)
            {
                DialogueOption optionInstance = originalOption.dialogueOptionRunTimeInstace();
                suspectInstance.dialogueOptions.Add(optionInstance);
            }
        }

        return suspectInstance;
    }
}