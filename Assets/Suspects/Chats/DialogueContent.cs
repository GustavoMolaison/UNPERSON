using UnityEngine;


    public enum SpeakerType 
   {
    Player,
    Suspect
    }


    [System.Serializable]
    public struct DialogueLine 
    {
    public SpeakerType speaker;
    
    [TextArea(2, 5)] 
    public string text;
    
    }

