using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextChangeListener : MonoBehaviour
{
    [SerializeField] private TMP_Text tmp;
    [SerializeField] private ScrollRect scrollRect;
    void OnEnable()
    {
        // TMPro_EventManager odpala się globalnie przy każdej zmianie geometrii tekstu
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
    }

    void OnDisable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);
    }

    private void OnTextChanged(Object obj)
    {
        // Sprawdzasz, czy to akurat ten konkretny obiekt zmienił tekst
        if (obj == tmp)
        {
            StartCoroutine(ResetScrollPositionRoutine());
            // Twój kod reagujący na zmianę tekstu
        }
    }

    private IEnumerator ResetScrollPositionRoutine()
    {
        yield return new WaitForEndOfFrame();    
        Canvas.ForceUpdateCanvases();       
        scrollRect.verticalNormalizedPosition = 1f;
        
        
    }
}

