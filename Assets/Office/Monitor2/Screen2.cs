using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Screen2 : MonitorBase
{
    public static Screen2 Instance;

    public List<string> chatersList;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        foreach (Suspect sus in SuspectTracker.instance.currentSuspects)
        {
            ChattersUICreator.instance.CreateChatersPreFab(sus);
        }
    }

    private void OnMouseDown()
    {
        GameManager.Instance.changeCamera(GameManager.Instance.inMonitor2);
    }


    public void backButton()
    {
        screenChanger(prevCanvas);
    }

    public void chatterGroupOfOn()
    {
        //ChattersUICreator.instance.groupToSuspect[SuspectTracker.instance.previousSuspect].gameObject.SetActive(false);
        ChattersUICreator.instance.groupToSuspect[SuspectTracker.instance.currentSuspect].gameObject.SetActive(true);
    }


}

    
