using UnityEngine;

using UnityEngine.UI;



public class ChatMesseageCloud : MonoBehaviour

{

    [SerializeField] private RectTransform layout;

    private LayoutElement layoutElement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()

    {
        //layoutElement = GetComponent<LayoutElement>();
        //Debug.Log("halopolicja");
        //Debug.Log(LayoutUtility.GetPreferredHeight(layout));
        //layoutElement.preferredHeight = LayoutUtility.GetPreferredHeight(layout);

    }

    private void Update()
    {
        layoutElement = GetComponent<LayoutElement>();
        layoutElement.preferredHeight = LayoutUtility.GetPreferredHeight(layout);
    }


}