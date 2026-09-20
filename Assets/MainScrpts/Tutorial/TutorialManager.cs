
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;
    //[SerializeField] private EvidenceCopert copert;
    [SerializeField] private GameObject copertTutorialUi;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }


    public void copertSpaceSetActive(string text = null)
    {

        if (text != null)
        {
            TextMeshProUGUI textSpace = copertTutorialUi.GetComponent<TextMeshProUGUI>();
            textSpace.text = text;
            textSpace.enabled = true;
        }
    }
    public void copertSpaceSetDisabled()
    {

        
         TextMeshProUGUI textSpace = copertTutorialUi.GetComponent<TextMeshProUGUI>();
         textSpace.enabled = false;
        
    }
}