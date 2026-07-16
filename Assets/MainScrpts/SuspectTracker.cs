using System;
using System.Collections.Generic;
using UnityEngine;

public class SuspectTracker : MonoBehaviour
{
    [HideInInspector] public List<Suspect> currentSuspects;
    [HideInInspector] public List<ConclusionPanelData> conclusionPanels;
    [HideInInspector] public Dictionary<Suspect, SuspGuees> SuspectGueses = new Dictionary<Suspect, SuspGuees>();

    public void SetSuspectGuess(Suspect suspect, SuspGuees newGuess)
    {

        SuspectGueses[suspect] = newGuess;
        // Funcions we wont to be called when changing dict values
        foreach (var panel in conclusionPanels)
        {
            if (panel.suspectData == suspect)
            {
                panel.changeGraphValues(newGuess);
            }
        }   
        
    }
    public IReadOnlyDictionary<Suspect, SuspGuees> SuspectGuesses => SuspectGueses;
    [HideInInspector] public Suspect currentSuspect;
    [HideInInspector] public Suspect previousSuspect;

    public static SuspectTracker instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        //currentSuspect = currentSuspects[0];
        //previousSuspect = currentSuspects[0];
    }

    //private void Start()
    //{
    //    currentSuspects = LevelsContentInfo.Instance.levelsList[0].SuspectsList;
        
    //    currentSuspect = currentSuspects[0];
    //    previousSuspect = currentSuspects[0];

    //}

    private bool initilized = false;
    public void initilize()
    {
        // if (instance == null) instance = this;
        // else Destroy(gameObject);

        currentSuspects = LevelsContentInfo.Instance.levelsList[0].SuspectsList;

        currentSuspect = currentSuspects[0];
        previousSuspect = currentSuspects[0];

        SuspectGueses.Clear();

        foreach (Suspect suspect in currentSuspects)
        {
            if (suspect != null)
            {
                SuspectGueses[suspect] = default(SuspGuees);
            }
        }

        initilized = true;
    }

    //private void Update()
    //{
    //    if (currentSuspect != previousSuspect)
    //    {
    //        Screen1.Instance.suspectPanelExtensionSwitch(currentSuspect);
    //    }


    //    previousSuspect = currentSuspect;
    //}


    public void changeCurrentSuspect(Suspect susp)
    {
        // All function that react to changing the current suspect should be here.
        if (currentSuspects.Contains(susp))
        {
            Debug.Log("Panel click");
            // THIS GOES FRIST IT CHANGES THE CURRRENT SUSPECT
            previousSuspect = currentSuspect;
            currentSuspect = susp;
            Screen1.Instance.suspectPanelExtensionSwitch(currentSuspect);
            // InterrogationManager.Instance.changeSuspectPng();
            Screen2.Instance.chatterGroupOfOn();

    

        }


    }

    
}
