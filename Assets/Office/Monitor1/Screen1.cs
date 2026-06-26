using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Screen1 : MonitorBase
{

    

    [SerializeField]
    private Canvas mainCanvas;
    [SerializeField]
    private Canvas suspectsCanvas;
    [SerializeField]
    private Canvas conclusionCanvas;





    [Header("UI References")]
    [SerializeField] private GameObject extension;
    [SerializeField] private TMP_Text namText2;
    [SerializeField] private TMP_Text birthDateText;
    [SerializeField] private TMP_Text ageText;
    [SerializeField] private TMP_Text occupationText;
    [SerializeField] private TMP_Text genderText;
    [SerializeField] private Image faceImage2;
    [SerializeField] private DropDownStatePick SuspGueesDropDown;



    public static Screen1 Instance;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Every canvas has to be enrolled here manually
        //canvasList = new List<Canvas> { mainCanvas, suspectsCanvas, conclusionCanvas };
        mainCanvas.enabled = true;
        //suspectsCanvas.enabled = false;
    }
    

    public void suspectButton()
    {
        canvasChanger(suspectsCanvas);
    }
    
    public void backButton()
    {
        Debug.Log("Back button clicked");
        canvasChanger(prevCanvas);
    }

    public void conclusionButton()
    {
        canvasChanger(conclusionCanvas);
    }

    

    public void suspectPanelExtensionSwitch(Suspect suspectData)
    {
        if(extension.activeSelf == false)
        {
            extension.SetActive(true);
        }
        
        if (extension.activeSelf) 
            {
           
             namText2.text = suspectData.FirstName + " " + suspectData.LastName;
             birthDateText.text = "Birth" + "\n" + suspectData.BirthDate;
             ageText.text = "Age" + "\n" + suspectData.Age.ToString(); // Int na string
             occupationText.text = "occupation" +  "\n" +  suspectData.Occupation;
             genderText.text = "Gender" + "\n" + suspectData.Gender.ToString(); // Enum na string
             faceImage2.sprite = suspectData.Face;

            SuspGueesDropDown.pickedSuspect = suspectData;
            //Debug.Log(SuspectTracker.instance.SuspectGueses[suspectData]);

             }
    }

    public void endInvestigation()
    {
        bool good = true;

        foreach(Suspect susp in SuspectTracker.instance.currentSuspects)
        {
            if(susp.Role != SuspectTracker.instance.SuspectGueses[susp])
            {
                good = false;
            }
        }

        print("misja wykonana" + good);

    }
}
