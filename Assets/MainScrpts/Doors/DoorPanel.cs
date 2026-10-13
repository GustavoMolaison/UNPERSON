using UnityEngine;

// Panel przy drzwiach - E otwiera / zamyka przypisane drzwi.
// Można go postawić gdziekolwiek, wystarczy podpiąć drzwi w inspektorze.
public class DoorPanel : Interactable
{
    [SerializeField] private ElectricDoor door;

    public override string Prompt => door != null && door.IsOpen ? "Zamknij drzwi" : "Otwórz drzwi";

    public override void Interact(PlayerController player)
    {
        if (door != null) door.Toggle();
    }
}
