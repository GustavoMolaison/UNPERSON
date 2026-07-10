using UnityEngine;

public enum SuspGuees
{
    Innocent,
    Culprit,
    UnPerson,
    Accomplice
}
public class DropDownStatePick : MonoBehaviour
{
    [HideInInspector] public Suspect pickedSuspect;


    public void handleOutput(int input)
    {
        if(input == 0)
        {
            SuspectTracker.instance.SetSuspectGuess(pickedSuspect, SuspGuees.Innocent);
        }
        if (input == 1)
        {
            SuspectTracker.instance.SetSuspectGuess(pickedSuspect, SuspGuees.Culprit);
        }
        if(input == 2)
        {
            SuspectTracker.instance.SetSuspectGuess(pickedSuspect, SuspGuees.UnPerson);
        }
        if( input == 3)
        {
            SuspectTracker.instance.SetSuspectGuess(pickedSuspect, SuspGuees.Accomplice);
        }
        Debug.Log(SuspectTracker.instance.SuspectGueses[pickedSuspect]);

    }
}
