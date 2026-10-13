// Obiekt, na który można kliknąć E, żeby usiąść przy biurku (monitor, fotel, biurko...).
public class DeskInteractable : Interactable
{
    public string prompt = "Usiądź przy biurku";

    public override string Prompt => prompt;
    public override void Interact(PlayerController player) => player.SitDown();
}
