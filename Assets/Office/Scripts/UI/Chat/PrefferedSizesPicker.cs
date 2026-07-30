using UnityEngine;
using TMPro;
using UnityEngine.UI;



public class ChatMesseageCloud : UIDataOrigin<Suspect>

{

    [SerializeField] private RectTransform layout;

    private LayoutElement layoutElement;
    [HideInInspector] public TextMeshProUGUI Txt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()

    {
        Txt = GetComponentInChildren<TextMeshProUGUI>();
        //Debug.Log("halopolicja");
        //Debug.Log(LayoutUtility.GetPreferredHeight(layout));
        //layoutElement.preferredHeight = LayoutUtility.GetPreferredHeight(layout);

    }

    override public void ApplyData(Suspect susp)
    {
        Debug.Log("xd");
        //Txt.text = susp;
    }

    private void Update()
    {
        layoutElement = GetComponent<LayoutElement>();
        layoutElement.preferredHeight = LayoutUtility.GetPreferredHeight(layout);
    }


}