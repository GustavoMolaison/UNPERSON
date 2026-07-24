using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomAudioPlayer : MonoBehaviour
{
    [Header("Lista Dźwięków")]
    [SerializeField] private AudioClip[] chairCreaks;

    [Header("Modyfikacja Pitcha (Tonu)")]
    [SerializeField] private float minPitch = 0.9f;
    [SerializeField] private float maxPitch = 1.1f;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Wywołaj tę metodę, gdy gracz poruszy fotelem
    /// </summary>
    public void PlayRandomCreak()
    {
        if (chairCreaks == null || chairCreaks.Length == 0) return;

        // 1. Losujemy klip z tablicy
        int randomIndex = Random.Range(0, chairCreaks.Length);
        AudioClip clipToPlay = chairCreaks[randomIndex];

        // 2. Lekko modyfikujemy pitch, żeby ten sam sampel brzmiał za każdym razem unikalnie
        audioSource.pitch = Random.Range(minPitch, maxPitch);

        // 3. Odtwarzamy bez przerywania obecnie grających dźwięków
        audioSource.PlayOneShot(clipToPlay);
    }
}