using UnityEngine;

public enum SuspGuees
{
    Innocent,
    Culprit,
    UnPerson,
    Undecided
}
public class DropDownStatePick : MonoBehaviour
{
    [HideInInspector] public Suspect pickedSuspect;


    public void handleOutput(int input)
    {
        if(input == 0)
        {
            SuspectTracker.instance.SuspectGueses[pickedSuspect] = SuspGuees.Innocent;
        }
        if (input == 1)
        {
            SuspectTracker.instance.SuspectGueses[pickedSuspect] = SuspGuees.Culprit;
        }
        if(input == 2)
        {
            SuspectTracker.instance.SuspectGueses[pickedSuspect] = SuspGuees.UnPerson;
        }
        if( input == 3)
        {
            SuspectTracker.instance.SuspectGueses[pickedSuspect] = SuspGuees.Undecided;
        }
        Debug.Log(SuspectTracker.instance.SuspectGueses[pickedSuspect]);

    }
}
