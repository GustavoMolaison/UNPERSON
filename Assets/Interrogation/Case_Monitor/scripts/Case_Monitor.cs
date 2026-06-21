using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using System.Runtime.InteropServices.WindowsRuntime;

public class Case_Monitor : MonitorBase
{
    

    public static Case_Monitor Instance;
    

    [SerializeField] public GameObject viewContent;
    [SerializeField] public GameObject evidencePanelPF;

    [HideInInspector] public bool playerIsPickingEvidence = false;
    [HideInInspector] private bool evidenceIsDecided = false;
    [HideInInspector] private bool correctAnswer = false;

    [SerializeField] public HighLight EvidencehightLight;
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




    private void changeEvidenceSection()
    {
        //EvidenceSectionManager.Instance.changeAllContent()
    }


}
