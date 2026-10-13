using System;
using UnityEngine;

// Drzwi rozsuwane na boki (dwa skrzydła chowają się w ścianę).
// Wymiary w lokalnych jednostkach prefabu (1 = 1 metr przed skalowaniem).
public class ElectricDoor : MonoBehaviour
{
    [SerializeField] private Transform leftLeaf;
    [SerializeField] private Transform rightLeaf;
    [SerializeField] private float openDistance = 0.5f;
    [SerializeField] private float openTime = 0.6f;
    [Tooltip("0 = drzwi nie zamykają się same")]
    [SerializeField] private float autoCloseDelay = 0f;

    [Header("Przejście (żeby drzwi nie zamknęły się na graczu)")]
    [SerializeField] private Vector3 doorwayCenter = new Vector3(0f, 1.05f, 0f);
    [SerializeField] private Vector3 doorwayHalfSize = new Vector3(0.5f, 1.05f, 0.4f);

    [Header("Wygląd / dźwięk")]
    [SerializeField] private Renderer[] indicators;
    [SerializeField] private Material openMaterial;
    [SerializeField] private Material closedMaterial;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;

    public bool IsOpen { get; private set; }
    public event Action<bool> OnStateChanged;

    private Vector3 leftClosed;
    private Vector3 rightClosed;
    private float progress;
    private float openedAt;
    private AudioSource audioSource;

    void Awake()
    {
        leftClosed = leftLeaf.localPosition;
        rightClosed = rightLeaf.localPosition;
        audioSource = GetComponent<AudioSource>();
        UpdateIndicators();
    }

    public void Toggle() => SetOpen(!IsOpen);

    public void SetOpen(bool open)
    {
        if (open == IsOpen) return;
        if (!open && SomeoneInDoorway()) return;

        IsOpen = open;
        openedAt = Time.time;
        var clip = open ? openSound : closeSound;
        if (audioSource != null && clip != null) audioSource.PlayOneShot(clip);
        UpdateIndicators();
        OnStateChanged?.Invoke(open);
    }

    void Update()
    {
        if (IsOpen && autoCloseDelay > 0f && Time.time - openedAt > autoCloseDelay)
        {
            if (SomeoneInDoorway()) openedAt = Time.time;
            else SetOpen(false);
        }

        float target = IsOpen ? 1f : 0f;
        if (Mathf.Approximately(progress, target)) return;
        progress = Mathf.MoveTowards(progress, target, Time.deltaTime / openTime);
        float k = Mathf.SmoothStep(0f, 1f, progress);
        leftLeaf.localPosition = leftClosed + Vector3.left * (openDistance * k);
        rightLeaf.localPosition = rightClosed + Vector3.right * (openDistance * k);
    }

    private bool SomeoneInDoorway()
    {
        var hits = Physics.OverlapBox(transform.TransformPoint(doorwayCenter),
                                      Vector3.Scale(doorwayHalfSize, transform.lossyScale),
                                      transform.rotation, ~0, QueryTriggerInteraction.Ignore);
        foreach (var h in hits)
            if (h is CharacterController) return true;
        return false;
    }

    private void UpdateIndicators()
    {
        var mat = IsOpen ? openMaterial : closedMaterial;
        if (mat == null || indicators == null) return;
        foreach (var r in indicators)
            if (r != null) r.sharedMaterial = mat;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.8f, 0f, 0.4f);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(doorwayCenter, doorwayHalfSize * 2f);
    }
}
