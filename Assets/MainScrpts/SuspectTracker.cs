using System.Collections.Generic;
using UnityEngine;

public class SuspectTracker : MonoBehaviour
{
    public List<Suspect> currentSuspects;
    [HideInInspector] public Suspect currentSuspect;
    [HideInInspector] public Suspect previousSuspect;

    public static SuspectTracker instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        currentSuspect = currentSuspects[0];
        previousSuspect = currentSuspects[0];

    }

    private void Update()
    {
        if (currentSuspect != previousSuspect)
        {
            Screen1.Instance.suspectPanelExtensionSwitch(currentSuspect);
        }


        previousSuspect = currentSuspect;
    }

    
    public void changeCurrentSuspect(Suspect susp)
    {
        if (currentSuspects.Contains(susp))
        {
            currentSuspect = susp;
            Screen1.Instance.suspectPanelExtensionSwitch(susp);

            Screen2.Instance.chatterGroupOfOn();
        }
    }
}
