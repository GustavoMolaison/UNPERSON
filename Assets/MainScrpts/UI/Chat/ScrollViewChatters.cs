using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewChatters : MonoBehaviour
{


    // Update is called once per frame
    private ScrollRect scrollRect;
    private GameObject convs;

    private void Start()
    {
        // 2. Przypisujesz wartoœæ TUTAJ
        scrollRect = GetComponent<ScrollRect>();
    }
    private void Update()
    {
        foreach (Transform child in this.transform)
        {
            
            if (child.gameObject.activeSelf)
            {
                
                RectTransform childRect = child.GetComponent<RectTransform>();

                if (childRect != null)
                {
                    scrollRect.content = childRect;
                    break;
                }
                
                
            }
        }
    }

    


}
