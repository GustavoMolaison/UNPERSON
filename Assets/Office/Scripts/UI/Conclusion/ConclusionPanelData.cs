using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using TMPro.EditorUtilities;

public class ConclusionPanelData : UIDataOrigin<Suspect>
{
    private Suspect suspectData;
    [SerializeField] private TMP_Text suspName;
    [SerializeField] private Image targetImage;

    [SerializeField] private Button myButton1;
    [SerializeField] private SuspGuees statusForThisButton1;
    [SerializeField] private Button myButton2;
    [SerializeField] private SuspGuees statusForThisButton2;
    [SerializeField] private Button myButton3;
    [SerializeField] private SuspGuees statusForThisButton3;
    [SerializeField] private Button myButton4;
    [SerializeField] private SuspGuees statusForThisButton4;

    [SerializeField] private Image shaderImage1;
    [SerializeField] private Image shaderImage2;
    [SerializeField] private Image shaderImage3;
    [SerializeField] private Image shaderImage4;

    [SerializeField] private DOPSimpleShaderManager shaderManager1;
    [SerializeField] private DOPSimpleShaderManager shaderManager2;
    [SerializeField] private DOPSimpleShaderManager shaderManager3;
    [SerializeField] private DOPSimpleShaderManager shaderManager4;
    private int isInnocent;
    private int isCulprit;
    private int isUnPerson;
    private int isAccomplice;

    private Material instantiatedMaterial1;
    private Material instantiatedMaterial2;
    private Material instantiatedMaterial3;
    private Material instantiatedMaterial4;
    private void Awake()
    {
        isInnocent = Shader.PropertyToID("_Innocent");
        isCulprit = Shader.PropertyToID("_Victim");
        isUnPerson = Shader.PropertyToID("_Unperson");
        isAccomplice = Shader.PropertyToID("_Accomplice");


        instantiatedMaterial1 = new Material(shaderImage1.material);
        instantiatedMaterial2 = new Material(shaderImage2.material);
        instantiatedMaterial3 = new Material(shaderImage3.material);
        instantiatedMaterial4 = new Material(shaderImage4.material);

        shaderManager1.material = instantiatedMaterial1;
        shaderManager2.material = instantiatedMaterial2;
        shaderManager3.material = instantiatedMaterial3;
        shaderManager4.material = instantiatedMaterial4;

        shaderImage1.material = instantiatedMaterial1;
        shaderImage2.material = instantiatedMaterial2;
        shaderImage3.material = instantiatedMaterial3;
        shaderImage4.material = instantiatedMaterial4;
    }
    public override void ApplyData(Suspect data)
    {
        suspectData = data;
        suspName.text = suspectData.FirstName;
        targetImage.sprite = suspectData.Face;

        myButton1.onClick.RemoveAllListeners();
        myButton1.onClick.AddListener(() => pickStatus(statusForThisButton1));
        myButton2.onClick.RemoveAllListeners();
        myButton2.onClick.AddListener(() => pickStatus(statusForThisButton2));
        myButton3.onClick.RemoveAllListeners();
        myButton3.onClick.AddListener(() => pickStatus(statusForThisButton3));
        myButton4.onClick.RemoveAllListeners();
        myButton4.onClick.AddListener(() => pickStatus(statusForThisButton4));
    }

    public void pickStatus(SuspGuees guees)
    {
        changeGraphValues(guees);
        SuspectTracker.instance.SuspectGueses[suspectData] = guees;
        if(guees == SuspGuees.Innocent)
        {
            
            instantiatedMaterial1.SetFloat("Innocent", isInnocent);
        }
        if(guees == SuspGuees.Culprit)
        {
            isCulprit = 1;
        }
        if (guees == SuspGuees.UnPerson)
        {
            isUnPerson = 1;
        }
        if (guees == SuspGuees.Accomplice)
        {
            isAccomplice = 1;
        }

    }

    private void changeGraphValues(SuspGuees guees)
    {
        if(guees == SuspGuees.Innocent)
        {
            isInnocent = 1;
            isCulprit = 0;
            isAccomplice = 0;
            instantiatedMaterial1.SetFloat("Innocent", isInnocent);
        }

        if (guees == SuspGuees.Culprit)
        {
            isInnocent = 0;
            isCulprit = 1;
            isAccomplice = 0;
            instantiatedMaterial2.SetFloat("Culprit", isCulprit);
        }

        if (guees == SuspGuees.UnPerson)
        {
            isUnPerson = 1;
            instantiatedMaterial3.SetFloat("UnPerson", isUnPerson);
        }

        if (guees == SuspGuees.Accomplice)
        {
            isInnocent = 0;
            isAccomplice = 1;
            isAccomplice = 0;
            instantiatedMaterial4.SetFloat("Accomplice", isAccomplice);
        }

      
    }
}
