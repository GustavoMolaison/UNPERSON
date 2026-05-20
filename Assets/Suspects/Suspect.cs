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

    [Header("Evidence & Logs")]
    [SerializeField] private List<Conversation> chatHistory = new List<Conversation>();
    [SerializeField] private List<dialogueOption> dialogueOptions = new List<dialogueOption>();

    
    public string FirstName => firstName;
    public string LastName => lastName;
    public string FullName => $"{firstName} {lastName}";
    public string BirthDate => birthDate;
    public int Age => age;
    public string Occupation => occupation;
    public GenderType Gender => gender;
    public Sprite Face => face;
    public List<Conversation> ChatHistory => chatHistory;

    public List<dialogueOption> DialogueOptions => dialogueOptions;

}