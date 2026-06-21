using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
public class EvidencePanelManager : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public TextMeshProUGUI title;
    [HideInInspector] private Evidence enrolledEvidence;
    [SerializeField] public HighLight highlight;

    


    public void enrollEvidence(Evidence evid)
    {
        enrolledEvidence = evid;
        title.text = evid.Title;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("whatPanelEVidence");
        EvidenceSectionManager.Instance.changeAllContent(enrolledEvidence.Title, enrolledEvidence.Cover, enrolledEvidence.Details, enrolledEvidence.Sprite);
        if (Case_Monitor.Instance.playerIsPickingEvidence)
        {
            if (eventData.clickCount == 2)
            {
                Case_Monitor.Instance.playerIsPickingEvidence = false;
                if (DialogueOptionManager.Instance.dialougePicked.enrolledEvidence == enrolledEvidence)
                {
                    Case_Monitor.Instance.changeAnswerState(true);
                    Case_Monitor.Instance.changeAnswerCorrectness(true);
                }
                else
                {
                    Case_Monitor.Instance.changeAnswerState(true);
                    Case_Monitor.Instance.changeAnswerCorrectness(false);
                }
            }
        }
        

            
    }
}
