using UnityEngine;

public class EvidenceManager : MonoBehaviour
{
    [SerializeField] public GameObject evidencePanelPF;
    [SerializeField] public GameObject viewContent;

    [HideInInspector] public bool playerIsPickingEvidence = false;
    [HideInInspector] private bool evidenceIsDecided = false;
    [HideInInspector] private bool correctAnswer = false;

    [HideInInspector] public Evidence currentlyPickedEvidence = null;

    [SerializeField] public HighLight EvidencehightLight;

    public static EvidenceManager Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        foreach (Evidence e in GameManager.Instance.evidenceList)
        {
            e.currentEvidenceUpdateState = -1;
            GameObject newpanel = Instantiate(evidencePanelPF, viewContent.transform, false);
            EvidencePanelManager panelManager = newpanel.GetComponent<EvidencePanelManager>();
            panelManager.enrollEvidence(e);
            
        }
    }
    public void addEvidence(Evidence e, int index = -1)
    {
        if(GameManager.Instance.evidenceList.Contains(e))
        {
            updateEvidence(e, index);
            return;
        }

        GameManager.Instance.evidenceList.Add(e);

        GameObject newpanel = Instantiate(evidencePanelPF, viewContent.transform, false);
        EvidencePanelManager panelManager = newpanel.GetComponent<EvidencePanelManager>();
        panelManager.enrollEvidence(e);
    }

    public void updateEvidence(Evidence e, int index = -1)
    {
        // zabezpieczenie gdy evidencen jest w podstawowej wersji
        if(index == -1)
        {
            return;
        }

        if(!GameManager.Instance.evidenceList.Contains(e))
        {
            addEvidence(e, index);
            return;
        }

        foreach (Transform child in viewContent.transform)
        {
            EvidencePanelManager evidencePanel = child.GetComponent<EvidencePanelManager>();
            if(evidencePanel.enrolledEvidence == e)
            {
                if(e.currentEvidenceUpdateState > index)
                {
                    return;
                }
                evidencePanel.enrollEvidence(e.UpdatedEvidenceVersion[index]);
                // e.currentEvidenceUpdateState = index;
            }
        }
    }

    public void changeAnswerCorrectness(bool state)
    {
        correctAnswer = state;
    }

    public bool checkAnswerCorrectness()
    {
        bool returnValue = correctAnswer;
        correctAnswer = false;
        return returnValue;
    }
    public void changeAnswerState(bool state)
    {
        evidenceIsDecided = state;
    }

    public bool checkAnswerState()
    {
        bool returnValue = evidenceIsDecided;
        evidenceIsDecided = false;
        return returnValue;
    }

    public void highLightEvidences(bool light) 
    {
        foreach (Transform child in viewContent.transform)
        {
            EvidencePanelManager evid = child.GetComponent<EvidencePanelManager>();
            if(evid != null)
            {
                if (light == true)
                {
                    evid.highlight.permaLight();
                }
                else
                {
                    evid.highlight.disableLight();
                }
            }
        }
    }
}
