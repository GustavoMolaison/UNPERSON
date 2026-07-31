using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private TMP_Text textComponent;
    [SerializeField] private TMP_Text whoTextComponent;
    [SerializeField] private float timeBetweenCharacters = 0.03f;

    private string targetText;
    private bool isSkipped; // Flaga przerwania

    public bool IsTyping { get; private set; }

    // Opcjonalna flaga, aby kontrolować, czy chcemy wyświetlać "whoText" w trybie konwersacji
    [SerializeField] private bool conversationMode = true;

    public Coroutine SetText(string newText, string whoText = null)
    {
        targetText = newText;
        isSkipped = false; // Resetujemy flagę przed nowym tekstem
        return StartCoroutine(TypeText(newText, whoText));
    }

    private IEnumerator TypeText(string textToType, string whoText = null)
    {
        if (conversationMode)
        {
            // Ustawiamy kolor tesktu wzaleznosci od guessa gracza kim jest rozmowca
            if(whoText != "You")
            {
              Color col = SuspectTracker.instance.guessColors[SuspectTracker.instance.SuspectGueses[SuspectTracker.instance.currentSuspect]];
              whoTextComponent.color = col; 
            }
            whoTextComponent.text = whoText + ":";  
        }

        IsTyping = true;
        textComponent.text = "";

        foreach (char letter in textToType)
        {
            // Jeśli gracz kliknął Skip, wychodzimy z pętli dopisywania liter!
            if (isSkipped)
                break;

            textComponent.text += letter;
            yield return new WaitForSeconds(timeBetweenCharacters);
        }

        // Zawsze ustawiamy pełny tekst na koniec (niezależnie czy wyklikało po literce, czy przeszkoczył Skip)
        textComponent.text = targetText;
        IsTyping = false;
    }

    public void Skip()
    {
        if (!IsTyping) return;

        // Ustawiamy flagę na true – korutyna TypeText sama łagodnie zakończy pracę w następnym kroku
        isSkipped = true;
    }
}