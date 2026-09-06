using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using System.Collections;
using UnityEngine.Localization.Tables;
using System.Linq;

public class SvDialougeManager : MonoBehaviour
{
    [SerializeField] private GameObject dialougePanelGO;
    private bool isPanelVisible = false;
    [SerializeField] private TextMeshProUGUI dialougeText;
    public static SvDialougeManager Instance { get; private set; }
    private TypewriterEffect typewriterEffect;

  
    [SerializeField] private float hideDelay = 2f; 
    [SerializeField] private float delayBetweenLines = 1.5f; 
    private Coroutine dialogueSequenceCoroutine;
    private Coroutine hideCoroutine;

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

    private void Update()
    {
        // Debug.Log(hideCoroutine);
        // if (isPanelVisible && !typewriterEffect.IsTyping && hideCoroutine == null)
        // {
        //  hideCoroutine = StartCoroutine(HidePanelWithDelay());
        // }
    }

    private System.Collections.IEnumerator HidePanelWithDelay()
    {
     yield return new WaitForSeconds(hideDelay);

     dialougePanelGO.SetActive(false);
     isPanelVisible = false; // pamiętaj zaktualizować stan widoczności!
     hideCoroutine = null;   // reset referencji, żeby timer mógł odpalić ponownie
    }
    public void newDialouge(LocalizedStringTable tableReference)
    {
        // Jeśli leci już jakiś dialog, przerywamy go
        if (dialogueSequenceCoroutine != null)
        {
            StopCoroutine(dialogueSequenceCoroutine);
        }

        dialogueSequenceCoroutine = StartCoroutine(PlayDialogueSequence(tableReference));
        
    }

   

   private IEnumerator PlayDialogueSequence(LocalizedStringTable tableReference)
    {
        // 1. Pobieramy właściwą tabelę dla aktualnego języka
        StringTable table = tableReference.GetTable();
        if (table == null)
        {
            Debug.LogError("Nie udało się załadować tabeli lokalizacji!");
            yield break;
        }

        dialougePanelGO.SetActive(true);
        isPanelVisible = true;

        // 2. Iterujemy po wszystkich wpisach (Entry) w tabeli
        foreach (StringTableEntry entry in table.Values.Reverse())
        {
            Debug.Log("1");
            typewriterEffect.Clean(); // Czyścimy poprzedni tekst przed rozpoczęciem nowego

            string lineText = entry.LocalizedValue;

            // Odpalamy maszynę do pisania i CZEKAMY, aż skończy pisać tę linijkę
            yield return typewriterEffect.SetText(lineText);

            // Pauza na przeczytanie tekstu zanim wjedzie następny wpis
            yield return new WaitForSeconds(delayBetweenLines);
            Debug.Log("2");
        }

        // 3. Po zakończeniu wszystkich wpisów gasimy panel
        dialougePanelGO.SetActive(false);
        isPanelVisible = false;
        dialogueSequenceCoroutine = null;


    }
}
