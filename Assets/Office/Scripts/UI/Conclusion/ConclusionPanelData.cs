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
    [SerializeField] private SuspGuees statusForThisButton1 = SuspGuees.Innocent;
    [SerializeField] private Button myButton2;
    [SerializeField] private SuspGuees statusForThisButton2 = SuspGuees.Culprit;
    [SerializeField] private Button myButton3;
    [SerializeField] private SuspGuees statusForThisButton3 = SuspGuees.UnPerson;
    [SerializeField] private Button myButton4;
    [SerializeField] private SuspGuees statusForThisButton4 = SuspGuees.Accomplice;

    [SerializeField] private Image shaderImage1;
    [SerializeField] private Image shaderImage2;
    [SerializeField] private Image shaderImage3;
    [SerializeField] private Image shaderImage4;

    [SerializeField] private DOPSimpleShaderManager shaderManager1;
    [SerializeField] private DOPSimpleShaderManager shaderManager2;
    [SerializeField] private DOPSimpleShaderManager shaderManager3;
    [SerializeField] private DOPSimpleShaderManager shaderManager4;
    private int isInnocentID;
    private int isCulpritID;
    private int isUnPersonID;
    private int isAccompliceID;

    private int isInnocent;
    private int isCulprit;
    private int isUnPerson;
    private int isAccomplice;

    private Material instantiatedMaterial1;
    private Material instantiatedMaterial2;
    private Material instantiatedMaterial3;
    private Material instantiatedMaterial4;

    [SerializeField] private Image ColorRect; 
    private void Awake()
    {
        isInnocentID = Shader.PropertyToID("_Innocent");
        isCulpritID = Shader.PropertyToID("_Victim");
        isUnPersonID = Shader.PropertyToID("_UnPerson");
        isAccompliceID = Shader.PropertyToID("_Accomplice");


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

        Debug.Log("COOOOO");
        myButton1.onClick.RemoveAllListeners();
        myButton1.onClick.AddListener(() => pickStatus(statusForThisButton1));
        myButton2.onClick.RemoveAllListeners();
        myButton2.onClick.AddListener(() => pickStatus(statusForThisButton2));
        myButton3.onClick.RemoveAllListeners();
        myButton3.onClick.AddListener(() => pickStatus(statusForThisButton3));
        myButton4.onClick.RemoveAllListeners();
        myButton4.onClick.AddListener(() => pickStatus(statusForThisButton4));
    }
    public override void ApplyData(Suspect data)
    {
        suspectData = data;
        suspName.text = suspectData.FirstName;
        targetImage.sprite = suspectData.Face;

        //    myButton1.onClick.RemoveAllListeners();
        //    myButton1.onClick.AddListener(() => pickStatus(statusForThisButton1));
        //    myButton2.onClick.RemoveAllListeners();
        //    myButton2.onClick.AddListener(() => pickStatus(statusForThisButton2));
        //    myButton3.onClick.RemoveAllListeners();
        //    myButton3.onClick.AddListener(() => pickStatus(statusForThisButton3));
        //    myButton4.onClick.RemoveAllListeners();
        //    myButton4.onClick.AddListener(() => pickStatus(statusForThisButton4));
    }

    public void pickStatus(SuspGuees guees)
    {
        changeGraphValues(guees);
        SuspectTracker.instance.SuspectGueses[suspectData] = guees;

        //if(isInnocent == 1)
        //{
        //    ColorRect.color = Color.blue;
        //}
        //if (isCulprit == 1)
        //{
        //    ColorRect.color = Color.red;
        //}
        //if (isUnPerson == 1)
        //{
        //    ColorRect.color = Color.green;
        //}
        //if (isAccomplice == 1)
        //{
        //    ColorRect.color = Color.orange;
        //}
        //if(guees == SuspGuees.Innocent)
        //{

        //    instantiatedMaterial1.SetFloat(isInnocentID, "_Innocent");
        //}
        //if(guees == SuspGuees.Culprit)
        //{
        //    isCulprit = 1;
        //}
        //if (guees == SuspGuees.UnPerson)
        //{
        //    isUnPerson = 1;
        //}
        //if (guees == SuspGuees.Accomplice)
        //{
        //    isAccomplice = 1;
        //}

    }

    private void changeGraphValues(SuspGuees guees)
    {
        if(guees == SuspGuees.Innocent)
        {
            if(isInnocent == 0)
            {
                isInnocent = 1;
                isCulprit = 0;
                isAccomplice = 0;
            }
            else
            {
                isInnocent = 0;
            }

                instantiatedMaterial1.SetFloat(isInnocentID, isInnocent);
            instantiatedMaterial2.SetFloat(isCulpritID, isCulprit);
            instantiatedMaterial4.SetFloat(isAccompliceID, isAccomplice);

        }

        if (guees == SuspGuees.Culprit)
        {
            if(isCulprit == 0)
            {
                isInnocent = 0;
                isCulprit = 1;
                isAccomplice = 0;
            }
            else
            {
                isCulprit = 0;
            }
               
            instantiatedMaterial1.SetFloat(isInnocentID, isInnocent);
            instantiatedMaterial2.SetFloat(isCulpritID, isCulprit);
            instantiatedMaterial4.SetFloat(isAccompliceID, isAccomplice);

        }

        if (guees == SuspGuees.UnPerson)
        {
            if (isUnPerson == 0)
                isUnPerson = 1;
            else
            {
                isUnPerson = 0;
            }
                instantiatedMaterial3.SetFloat(isUnPersonID, isUnPerson);
        }

        if (guees == SuspGuees.Accomplice)
        {
            if (isAccomplice == 0)
            {
                isInnocent = 0;
                isAccomplice = 1;
                isCulprit = 0;
            }
            else
            {
                isAccomplice = 0;
            }

            instantiatedMaterial4.SetFloat(isAccompliceID, isAccomplice);
            instantiatedMaterial2.SetFloat(isCulpritID, isCulprit);
            instantiatedMaterial1.SetFloat(isInnocentID, isInnocent);

        }
        Debug.Log("ZMIENIAM");
        


    }
}
