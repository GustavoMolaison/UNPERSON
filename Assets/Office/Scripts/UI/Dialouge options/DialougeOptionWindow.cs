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


    [SerializeField] private Image graphImage;
    private int yellowPropertyID;
    private int animationSwitchID;
    private Material instantiatedMaterial;
    [SerializeField] private DialougeOptionShader shaderManager;
    //private DialougeOptionShader
    //private DialougeOptionShader

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

        yellowPropertyID = Shader.PropertyToID("_Yellow");
        animationSwitchID = Shader.PropertyToID("_Animation");

        if (graphImage != null)
        {
            
            // WA�NE: 'new Material(...)' tworzy unikaln� instancj� w pami�ci,
            // na podstawie materia�u, kt�ry Image ma przypisany w Inspektorze.
            instantiatedMaterial = new Material(graphImage.material);
            shaderManager.material = instantiatedMaterial;

            // 3. Przypisujemy t� unikaln� kopi� Z POWROTEM do Image.
            // Od teraz ten Image korzysta ze swojej prywatnej wersji materia�u.
            graphImage.material = instantiatedMaterial;
        }
    }

    private void OnDestroy()
    {
        if (instantiatedMaterial != null)
        {
            Destroy(instantiatedMaterial);
        }
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
            txt.color = new Color(1f, 1f, 0.9f);
            // To powinno byc w dialouge option Shader ale mi sie kurwa nie chce tego zmieniac
            instantiatedMaterial.SetFloat(yellowPropertyID, 1);
        }
        else
        {
            txt.color = Color.cyan;
            if(instantiatedMaterial == null)
            {
                Debug.Log("xdd");
            }
            // To powinno byc w dialouge option Shader ale mi sie kurwa nie chce tego zmieniac
            instantiatedMaterial.SetFloat(yellowPropertyID, 0);
        }
    }

    private void Holdanimation(bool hold)
    {
        Debug.Log("ANIMACJA");
        if (hold)
        {
            Debug.Log("HOLD");
            shaderManager.isPlaying = false;
        }
        else
        {
            Debug.Log("NIEGOLD");
            
            shaderManager.isPlaying = true;
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

              
                Holdanimation(true);
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
                    shaderManager.wrongAnswerReact(howManyWrongGuesses / maxWrongGuesses);
                    DialogueOptionManager.Instance.dialougePicked = null;
                    Holdanimation(false);
                    yield break;
                }

                DialogueOptionManager.Instance.dialougePicked = null;
                Holdanimation(false);
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

    // public IEnumerator onClickWait()
    // {
    //     ConversationManager.Instance.chatNewMess(enrolledDialogue.dialogueContent); //THIS FRIST
    //     DialogueOptionManager.Instance.cleanDialogueOptions(); // THIS SECOND
    //     yield return new WaitUntil(() => DialogueManager.Instance.isProcessingQueue == false); // THIS THIRD

    //     // ConversationManager.Instance.chatNewMess(enrolledDialogue.dialogueContent);
    //     if(enrolledDialogue.isNewDialogueSequence)
    //     {
    //         DialogueOptionManager.Instance.dialoguesChange(true, enrolledDialogue.newDialogueSequence);
    //     }
    //     else
    //     {
    //         DialogueOptionManager.Instance.dialoguesChange(false);
    //     }
    // } 
}
