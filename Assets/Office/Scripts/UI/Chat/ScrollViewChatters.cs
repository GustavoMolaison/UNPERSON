using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewChatters : MonoBehaviour
{


    // Update is called once per frame
    private ScrollRect scrollRect;

    private void Start()
    {
        // 2. Przypisujesz wartoœæ TUTAJ
        scrollRect = GetComponent<ScrollRect>();
    }
    private void Update()
    {
        foreach (Transform child in this.transform)
        {
            // 1. Sprawdzasz czy obiekt jest aktywny (zamiast .enabled)
            if (child.gameObject.activeSelf)
            {
                // 2. Pobierasz RectTransform z dziecka
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
