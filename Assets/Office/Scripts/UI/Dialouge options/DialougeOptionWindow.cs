using System.Collections;
using TMPro;
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
    private Material instantiatedMaterial;

    private void Awake()
    {
        txt = GetComponentInChildren<TextMeshProUGUI>();

        yellowPropertyID = Shader.PropertyToID("_Yellow");

        if (graphImage != null)
        {
            Debug.Log("HALOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
            // WA¯NE: 'new Material(...)' tworzy unikaln¹ instancjê w pamiêci,
            // na podstawie materia³u, który Image ma przypisany w Inspektorze.
            instantiatedMaterial = new Material(graphImage.material);

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
    public void changeText(string newText, bool evidenceConnected = false)
    {
        txt.text = newText;
        if (evidenceConnected)
        {
            txt.color = Color.yellow;
            instantiatedMaterial.SetFloat(yellowPropertyID, 1);
        }
        else
        {
            txt.color = Color.black;
            if(instantiatedMaterial == null)
            {
                Debug.Log("xdd");
            }
            instantiatedMaterial.SetFloat(yellowPropertyID, 0);
        }
    }

    public void onClick()
    {
        
        DialogueManager.Instance.StartCoroutine(DialogueManager.Instance.dialogueOptionClicked(enrolledDialogue));

        if (!clicked)
        {
            clicked = true;
            Image img = GetComponent<Image>();
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0.5f); 
        }
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
