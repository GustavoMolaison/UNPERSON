using UnityEngine;

// Bazowa klasa dla wszystkiego, co gracz może użyć klawiszem E podczas chodzenia.
// Wymaga collidera na sobie albo na dzieciach.
public abstract class Interactable : MonoBehaviour
{
    public abstract string Prompt { get; }
    public abstract void Interact(PlayerController player);
}
