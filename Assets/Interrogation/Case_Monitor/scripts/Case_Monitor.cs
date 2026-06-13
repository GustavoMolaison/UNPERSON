using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Case_Monitor : MonitorBase
{
    

    public static Case_Monitor Instance;
    

    [SerializeField] public GameObject viewContent;
    [SerializeField] public GameObject evidencePanelPF;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        foreach (Evidence e in GameManager.Instance.currentLevel.EvidenceList)
        {
            GameObject newpanel = Instantiate(evidencePanelPF, viewContent.transform, false);
            EvidencePanelManager panelManager = newpanel.GetComponent<EvidencePanelManager>();
            panelManager.enrollEvidence(e);
        }
    }

    private void changeEvidenceSection()
    {
        //EvidenceSectionManager.Instance.changeAllContent()
    }


}
