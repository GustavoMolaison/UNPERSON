using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
public class DialogueOptionWindow : MonoBehaviour
{
    TextMeshProUGUI txt;
    [HideInInspector] public DialogueOption enrolledDialogue;
    private bool clicked = false;
    [HideInInspector] public bool initialized = false;

    [HideInInspector] private bool correctEvidencePicked;

    [SerializeField] private float maxWrongGuesses = 3;
    public float howManyWrongGuesses { get; private set; }

    public bool dialogueDisabled { get; private set; } = false;
    public void addOneWrongGuess()
    {
        if (howManyWrongGuesses <= maxWrongGuesses)
        {
            howManyWrongGuesses = howManyWrongGuesses + 1;
        }
        if (howManyWrongGuesses >= maxWrongGuesses)
        {
            Debug.Log("ZMIENIAM NA TRUE");
            dialogueDisabled = true;
        }

    }



    private void Awake()
    {
        txt = GetComponentInChildren<TextMeshProUGUI>();
    }

 


    public void enrollDialogue(DialogueOption dial)
    {
        Debug.Log("Enrolling dialogue: ");
        enrolledDialogue = dial;
        changeText(enrolledDialogue.dialogueTitle, enrolledDialogue.hasEvidenceCheck);
        initialized = true;
    }
    private void changeText(string newText, bool evidenceConnected = false)
    {
        txt.text = newText;


        if (evidenceConnected)
        {
            txt.color = new Color(1f, 1f, 0.8f);
            
        }
        else
        {
            txt.color = Color.cyan;
          
        }
    }

    

    public void onClick()
    {
        StartCoroutine(HandleNewDialogueSequence());
    }

    private IEnumerator HandleNewDialogueSequence()
    {
       
       
        if (!dialogueDisabled)
        {
            
            if (enrolledDialogue.hasEvidenceCheck)
            {
               
                DialogueOptionManager.Instance.dialougePicked = enrolledDialogue;

              
                //Holdanimation(true);
                Case_Monitor.Instance.EvidencehightLight.lightOn();
                Case_Monitor.Instance.highLightEvidences(true);

                // TUTAJ GRA SI� "ZATRZYMUJE" DLA TEGO SKRYPTU
                // Kod nie ruszy dalej, dop�ki funkcja/zmienna wewn�trz WaitUntil nie zwr�ci true.
                // Reszta gry normalnie dzia�a i si� renderuje.
                Case_Monitor.Instance.playerIsPickingEvidence = true;
                yield return new WaitUntil(() => didPlayerPickEvidence());
                Case_Monitor.Instance.highLightEvidences(false);

                if (!Case_Monitor.Instance.checkAnswerCorrectness())
                {
                    addOneWrongGuess();
                    //shaderManager.wrongAnswerReact(howManyWrongGuesses / maxWrongGuesses);
                    DialogueOptionManager.Instance.dialougePicked = null;
                    //Holdanimation(false);
                    yield break;
                }

                DialogueOptionManager.Instance.dialougePicked = null;
                //Holdanimation(false);
            }

            // Ten kod wykona si� dopiero, gdy warunek wy�ej pu�ci, 
            // LUB natychmiastowo, je�li if w og�le nie by� spe�niony (bo enrolledDialogue.hasEvidence by�o false).
            DialogueManager.Instance.StartCoroutine(DialogueManager.Instance.dialogueOptionClicked(enrolledDialogue));

            if (!clicked)
            {
               
                clicked = true;
                Image img = GetComponent<Image>();
                img.color = new Color(img.color.r, img.color.g, img.color.b, 0.5f);
            }
        }
    }

    private bool didPlayerPickEvidence()
    {
        //if(!Case_Monitor.Instance.playerIsPickingEvidence)
        //{
        //    Case_Monitor.Instance.playerIsPickingEvidence = true;
        //}
        
        
        return (Case_Monitor.Instance.checkAnswerState());
        

    }

   
}
