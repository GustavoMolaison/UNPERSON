using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Evidence", menuName = "Scriptable Objects/Evidence")]
public class Evidence : ScriptableObject
{
    [Header("Content")]
    [SerializeField] private string title;
    [SerializeField] private string cover;
    [SerializeField] private string details;
    [SerializeField] private Sprite sprite;

    [Header("Type")]
    [SerializeField] private bool isVisible;

    [Header("Evidence Update section")]
    [SerializeField] private List<Evidence> updatedEvidenceVersion;
    public int currentEvidenceUpdateState = -1;
    // type ??? visibilityCondition


    public string Title => title;
    public string Cover => cover;
    public string Details => details;
    public Sprite Sprite => sprite;
    public bool IsVisible => isVisible;

    
    public List<Evidence> UpdatedEvidenceVersion => updatedEvidenceVersion;

}
