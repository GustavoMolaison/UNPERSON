using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Screen2 : MonitorBase
{
    public static Screen2 Instance;

    public Dictionary<Suspect, GameObject> groupToSuspect;
    public Dictionary<GameObject, GameObject> chatterToConv;

    public List<string> chatersList;

    private GameObject prevConvs;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Dict for picking which convesration should be shown based on currently picked suspect
        groupToSuspect = new Dictionary<Suspect, GameObject>();
        chatterToConv = new Dictionary<GameObject, GameObject>();
    }

    private void Start()
    {
        foreach (Suspect sus in SuspectTracker.instance.currentSuspects)
        {
            ChattersUICreator.instance.CreateChatersPreFab(sus);
        }
    }

   


    public void backButton()
    {
        canvasChanger(prevCanvas);
    }
    
    public void chatterGroupOfOn()
    {
           
        var currentSuspect = SuspectTracker.instance.currentSuspect;

        GameObject suspectGroup = groupToSuspect[currentSuspect].gameObject;

        suspectGroup.SetActive(!suspectGroup.activeSelf);

        if (SuspectTracker.instance.previousSuspect != null && SuspectTracker.instance.previousSuspect != SuspectTracker.instance.currentSuspect)
        {
            var prevSusp = SuspectTracker.instance.previousSuspect;

            GameObject suspectGroup2 = groupToSuspect[prevSusp].gameObject;

            suspectGroup2.SetActive(!suspectGroup2.activeSelf);
        }
    }

    public void convChatOfOn(GameObject chatter)
    {
        if(prevConvs != null)
        {
            prevConvs.SetActive(false);
        }
        

        var currentSuspect = SuspectTracker.instance.currentSuspect;

        GameObject conv = chatterToConv[chatter].gameObject;

        conv.SetActive(!conv.activeSelf);

        prevConvs = conv;

        
    }



}

    
