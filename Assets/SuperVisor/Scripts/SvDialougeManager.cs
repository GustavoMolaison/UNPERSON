using TMPro;
using UnityEngine;

public class SvDialougeManager : MonoBehaviour
{
    [SerializeField] private GameObject dialougePanelGO;
    // [SerializeField] private TextMeshProUGUI dialougeText;
    public static SvDialougeManager Instance { get; private set; }
    private TypewriterEffect typewriterEffect;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        typewriterEffect = GetComponent<TypewriterEffect>();
    }
    public void newDialouge(string message)
    {
        dialougePanelGO.SetActive(true);
        typewriterEffect.SetText(message);
        
    }



}
