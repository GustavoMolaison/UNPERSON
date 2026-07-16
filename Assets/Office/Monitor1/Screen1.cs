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
    [SerializeField] private TMP_Text namText;
    [SerializeField] private TMP_Text SurNamText;
    [SerializeField] private TMP_Text birthDateText;
    [SerializeField] private TMP_Text ageText;
    [SerializeField] private TMP_Text occupationText;
    [SerializeField] private TMP_Text genderText;
    [SerializeField] private Image faceImage2;
    [SerializeField] private DropDownStatePick SuspGueesDropDown;
    [SerializeField] private TMP_Dropdown SuspGueesDropDownGO;



    public static Screen1 Instance;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // 1. Włączamy absolutnie wszystkie GameObjecty powiązane z Canvasami
        // // Dzięki temu odpala się na nich logiki z Awake() / Start()
        // ToggleCanvasGameObject(mainCanvas, true);
        // ToggleCanvasGameObject(suspectsCanvas, true);
        // ToggleCanvasGameObject(conclusionCanvas, true);

        // // 2. Natychmiast wyłączamy te, które mają być domyślnie ukryte
        // ToggleCanvasGameObject(suspectsCanvas, false);
        // ToggleCanvasGameObject(conclusionCanvas, false);
    }

    private void ToggleCanvasGameObject(Canvas canvas, bool state)
    {

        if (canvas != null)
        {
            canvas.gameObject.SetActive(state);
        }
    }

    void Start()
    {
         // Dzięki temu odpala się na nich logiki z Awake() / Start()
        // ToggleCanvasGameObject(mainCanvas, true);
        // ToggleCanvasGameObject(suspectsCanvas, true);
        // ToggleCanvasGameObject(conclusionCanvas, true);

        // 2. Natychmiast wyłączamy te, które mają być domyślnie ukryte
        // ToggleCanvasGameObject(suspectsCanvas, false);
        // ToggleCanvasGameObject(conclusionCanvas, false);
        // Every canvas has to be enrolled here manually
        //canvasList = new List<Canvas> { mainCanvas, suspectsCanvas, conclusionCanvas };
        // mainCanvas.gameObject.SetActive(true);
        //suspectsCanvas.enabled = false;

        mainCanvas.enabled = true;
        suspectsCanvas.enabled = false;
        conclusionCanvas.enabled = false;
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
        // SuspectTracker.instance.SuspectGueses[suspectData]
        if (extension.activeSelf) 
            {
           
             namText.text = suspectData.FirstName;
             SurNamText.text = suspectData.LastName;
             birthDateText.text = "Birth" + ":" + suspectData.BirthDate;
             ageText.text = "Age" + ":" + suspectData.Age.ToString(); // Int na string
             occupationText.text = "Occupation" +  ":" +  suspectData.Occupation;
             genderText.text = "Gender" + ":" + suspectData.Gender.ToString(); // Enum na string
             faceImage2.sprite = suspectData.Face;

            SuspGueesDropDown.pickedSuspect = suspectData;
            SuspGuees currentGuess = SuspectTracker.instance.SuspectGueses[suspectData];
            int input;
            switch (currentGuess)
            {
              case SuspGuees.Innocent:
               input = 0;
               break;
              case SuspGuees.Culprit:
               input = 1;
               break;
              case SuspGuees.UnPerson:
               input = 2;
               break;
              case SuspGuees.Accomplice:
               input = 3; 
               break;
              default:
               input = 0; // Wartość domyślna, na wypadek błędu
               break;
            }
            SuspGueesDropDownGO.value = input;


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
