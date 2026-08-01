using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Conversation", menuName = "Scriptable Objects/Conversation")]
public class Conversation : ScriptableObject
{
    [SerializeField] private string participantName; // The person the suspect is talking to
    public List<ChatMessage> messages = new List<ChatMessage>();



    public string ParticipantName => participantName;
}
