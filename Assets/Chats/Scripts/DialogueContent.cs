using UnityEngine;
using UnityEngine.Localization;

    public enum SpeakerType 
   {
    Player,
    Suspect
    }


    [System.Serializable]
public struct DialogueLine 
{
    public SpeakerType speaker;
    
    // Zamiast 'public string text;' dajesz to:
    public LocalizedString text;
}

