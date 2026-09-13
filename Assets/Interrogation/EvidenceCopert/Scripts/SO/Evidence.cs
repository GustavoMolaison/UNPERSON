using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

[CreateAssetMenu(fileName = "Evidence", menuName = "Scriptable Objects/Evidence")]
public class Evidence : ScriptableObject
{
    [Header("Content")]

    [SerializeField] private LocalizedStringTable table;
    [SerializeField] private LocalizedString title;
    [SerializeField] private LocalizedString details;
    [SerializeField] private Sprite sprite;

    [Header("Type")]
    [SerializeField] private bool isVisible;

    [Header("Evidence Update section")]
    [SerializeField] private List<Evidence> updatedEvidenceVersion;
    public int currentEvidenceUpdateState = -1;
    // type ??? visibilityCondition
   
    public Sprite Sprite => sprite;
    public bool IsVisible => isVisible;

    
    public List<Evidence> UpdatedEvidenceVersion => updatedEvidenceVersion;


    public string Title => table != null
        ? table.GetTable()?.GetEntry("Title")?.GetLocalizedString() ?? title.GetLocalizedString()
        : title.GetLocalizedString();

    public string Details => table != null
        ? table.GetTable()?.GetEntry("Details")?.GetLocalizedString() ?? details.GetLocalizedString()
        : details.GetLocalizedString();

}