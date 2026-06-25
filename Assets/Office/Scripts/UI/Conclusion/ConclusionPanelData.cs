using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using TMPro.EditorUtilities;

public class ConclusionPanelData : UIDataOrigin<Suspect>
{
    private Suspect suspectData;
    [SerializeField] private TMP_Text suspName;
    [SerializeField] private Image targetImage;


    public override void ApplyData(Suspect data)
    {
        suspectData = data;
        suspName.text = suspectData.FirstName;
        targetImage.sprite = suspectData.Face;
         
    }
}
