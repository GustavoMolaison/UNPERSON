using UnityEngine;


[System.Serializable]
public class ChatMessage
{
    public string senderName; // Who sent it?
    [TextArea(3, 10)]
    public string content;    // What did they say?
    public string timestamp;  // e.g., "14:20"
}
