using UnityEngine;
using TMPro;
public class DialougeOptionWindow : MonoBehaviour
{
    TextMeshProUGUI txt;



    private void Awake()
    {
        txt = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void changeText(string newText)
    {
        
        
        txt.text = newText;
    }
}
