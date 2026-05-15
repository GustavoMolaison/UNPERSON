using UnityEngine;
using UnityEngine.UI; // Potrzebne do obs�ugi Image
using TMPro;
using TMPro.EditorUtilities;

public class SuspectPanel : UIDataOrigin<Suspect>
{
    [Header("UI References")]
    [SerializeField] private TMP_Text namText;
    //[SerializeField] private TMP_Text namText2;
    //[SerializeField] private TMP_Text birthDateText;
    //[SerializeField] private TMP_Text ageText;
    //[SerializeField] private TMP_Text occupationText;
    //[SerializeField] private TMP_Text genderText;
    [SerializeField] private Image faceImage;
    //[SerializeField] private Image faceImage2;
    

    [Header("Data Asset")]
    [SerializeField] private Suspect suspectData;

    void Start()
    {
        if (suspectData != null)
        {
            ApplyData(suspectData);
        }
        else
        {
            Debug.LogError($"Brak przypisanego Assetu Suspect na obiekcie!");
        }
    }

    // Metoda, kt�ra wype�nia UI danymi z obiektu
    override public void ApplyData(Suspect data)
    {
        namText.text = data.FirstName + " " + data.LastName;
        //namText2.text = data.Nam + " " + data.Surnam;
        //birthDateText.text = data.BirthDate;
        //ageText.text = data.Age.ToString(); // Int na string
        //occupationText.text = data.Occupation;
        //genderText.text = data.Gender.ToString(); // Enum na string
        faceImage.sprite = data.Face;
        //faceImage2.sprite = data.Face;
    }


    private void OnMouseDown()
    {
        Debug.Log("Panel click");
        //Debug.Log("what");
        //Screen1.Instance.suspectPanelExtensionSwitch(suspectData);
        SuspectTracker.instance.changeCurrentSuspect(suspectData);
    }

}
