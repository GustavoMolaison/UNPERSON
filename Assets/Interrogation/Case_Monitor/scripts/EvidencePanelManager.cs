using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
public class EvidencePanelManager : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public TextMeshProUGUI title;
    [HideInInspector] public Evidence enrolledEvidence;
    [SerializeField] public HighLight highlight;

    


    public void enrollEvidence(Evidence evid)
    {
        enrolledEvidence = evid;
        title.text = evid.Title;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
        EvidenceManager.Instance.currentlyPickedEvidence = enrolledEvidence;
        EvidenceSectionManager.Instance.changeAllContent(enrolledEvidence.Title, enrolledEvidence.Cover, enrolledEvidence.Details, enrolledEvidence.Sprite);
        if (EvidenceManager.Instance.playerIsPickingEvidence)
        {
            if (eventData.clickCount == 2)
            {
                EvidenceManager.Instance.playerIsPickingEvidence = false;
                if (DialogueOptionManager.Instance.dialougePicked.evidenceCheck == enrolledEvidence)
                {
                    EvidenceManager.Instance.changeAnswerState(true);
                    EvidenceManager.Instance.changeAnswerCorrectness(true);
                }
                else
                {
                    EvidenceManager.Instance.changeAnswerState(true);
                    EvidenceManager.Instance.changeAnswerCorrectness(false);
                }
            }
        }
        

            
    }
}
