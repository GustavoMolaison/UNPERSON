using UnityEngine;
using TMPro;
public class EvidencePanelManager : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI title;
    [HideInInspector] private Evidence enrolledEvidence;


    public void enrollEvidence(Evidence evid)
    {
        enrolledEvidence = evid;
        title.text = evid.Title;
    }

    public void Onclick()
    {
        Debug.Log("whatPanelEVidence");
        EvidenceSectionManager.Instance.changeAllContent(enrolledEvidence.Title, enrolledEvidence.Cover, enrolledEvidence.Details, enrolledEvidence.Sprite);
    }
}
