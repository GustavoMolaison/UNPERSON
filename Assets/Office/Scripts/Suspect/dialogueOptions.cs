using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "NewDialouge", menuName = "Dialouge")]
public class dialogueOption : ScriptableObject
{
    public string dialougeName;
    //public List<Dialogue> options = new List<Dialogue>();
    public List<string> DialougeContent = new List<string>();

    public string dialougeTittle => dialougeName;
}
