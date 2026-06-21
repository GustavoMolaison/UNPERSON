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

    

    private void Awake()
    {
        txt = GetComponentInChildren<TextMeshProUGUI>();

        yellowPropertyID = Shader.PropertyToID("_Yellow");
        animationSwitchID = Shader.PropertyToID("_Animation");

        if (graphImage != null)
        {
            Debug.Log("HALOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
            // WA¯NE: 'new Material(...)' tworzy unikaln¹ instancjê w pamiêci,
            // na podstawie materia³u, który Image ma przypisany w Inspektorze.
            instantiatedMaterial = new Material(graphImage.material);
            shaderManager.material = instantiatedMaterial;

            // 3. Przypisujemy tê unikaln¹ kopiê Z POWROTEM do Image.
            // Od teraz ten Image korzysta ze swojej prywatnej wersji materia³u.
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
        changeText(enrolledDialogue.dialogueTitle, enrolledDialogue.hasEvidence);
        initialized = true;
    }
    private void changeText(string newText, bool evidenceConnected = false)
    {
        txt.text = newText;
        if (evidenceConnected)
        {
            txt.color = Color.yellow;
            // To powinno byc w dialouge option Shader ale mi sie kurwa nie chce tego zmieniac
            instantiatedMaterial.SetFloat(yellowPropertyID, 1);
        }
        else
        {
            txt.color = Color.blue;
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
        StartCoroutine(HandleEvidenceSequence());
        //Debug.Log("Klikam");
        //if(enrolledDialogue.hasEvidence)
        //{
        //    Debug.Log("Yellow");
        //    animationControl(false);
        //    DialogueOptionManager.Instance.EvidencehightLight.lightOn();
        //    Case_Monitor.Instance.highLightEvidences();
        //}
        
        //DialogueManager.Instance.StartCoroutine(DialogueManager.Instance.dialogueOptionClicked(enrolledDialogue));

        //if (!clicked)
        //{
        //    Debug.Log("Dziala");
        //    clicked = true;
        //    Image img = GetComponent<Image>();
        //    img.color = new Color(img.color.r, img.color.g, img.color.b, 0.5f); 
        //}
    }

    private IEnumerator HandleEvidenceSequence()
    {
        if (enrolledDialogue.hasEvidence)
        {
            DialogueOptionManager.Instance.dialougePicked = enrolledDialogue;

            Debug.Log("Yellow");
            Holdanimation(true);
            Case_Monitor.Instance.EvidencehightLight.lightOn();
            Case_Monitor.Instance.highLightEvidences(true);

            // TUTAJ GRA SIÊ "ZATRZYMUJE" DLA TEGO SKRYPTU
            // Kod nie ruszy dalej, dopóki funkcja/zmienna wewn¹trz WaitUntil nie zwróci true.
            // Reszta gry normalnie dzia³a i siê renderuje.
            Case_Monitor.Instance.playerIsPickingEvidence = true;
            yield return new WaitUntil(() => didPlayerPickEvidence());
            Case_Monitor.Instance.highLightEvidences(false);

            if (!Case_Monitor.Instance.checkAnswerCorrectness())
            {
                DialogueOptionManager.Instance.dialougePicked = null;
                Holdanimation(false);
                yield break;
            }

            DialogueOptionManager.Instance.dialougePicked = null;
            Holdanimation(false);
        }

        // Ten kod wykona siê dopiero, gdy warunek wy¿ej puœci, 
        // LUB natychmiastowo, jeœli if w ogóle nie by³ spe³niony (bo enrolledDialogue.hasEvidence by³o false).
        DialogueManager.Instance.StartCoroutine(DialogueManager.Instance.dialogueOptionClicked(enrolledDialogue));

        if (!clicked)
        {
            Debug.Log("Dziala");
            clicked = true;
            Image img = GetComponent<Image>();
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0.5f);
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
