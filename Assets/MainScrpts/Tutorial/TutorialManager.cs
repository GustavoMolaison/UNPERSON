
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;
    //[SerializeField] private EvidenceCopert copert;
    [SerializeField] private GameObject copertTutorialUi;
    [SerializeField] private GameObject generalTutorialUi;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }


    public void copertSpaceSetActive(string text = null, System.Action onComplete = null)
    {

        if (text != null)
        {
            TextMeshProUGUI textSpace = copertTutorialUi.GetComponent<TextMeshProUGUI>();
            textSpace.text = text;
            textSpace.enabled = true;
            onComplete();
        }
    }
    public void copertSpaceSetDisabled()
    {

        
         TextMeshProUGUI textSpace = copertTutorialUi.GetComponent<TextMeshProUGUI>();
         textSpace.enabled = false;
        
    }

    public void generalSpaceSetActive(string text = null, System.Action onComplete = null)
    {

        if (text != null)
        {
            TextMeshProUGUI textSpace = generalTutorialUi.GetComponent<TextMeshProUGUI>();
            textSpace.text = text;
            textSpace.enabled = true;
            onComplete();
        }
    }
    public void generalSpaceSetDisabled()
    {


        TextMeshProUGUI textSpace = generalTutorialUi.GetComponent<TextMeshProUGUI>();
        textSpace.enabled = false;

    }
}