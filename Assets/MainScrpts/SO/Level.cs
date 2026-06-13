using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "Level", menuName = "Scriptable Objects/Level")]
public class Level : ScriptableObject
{
    [Header("Suspects")]
    [SerializeField] private List<Suspect> suspectsList = new List<Suspect>();

    [Header("Evidence")]
    [SerializeField] private List<Evidence> evidenceList = new List<Evidence>();



    public List<Suspect> SuspectsList => suspectsList;
    public List<Evidence> EvidenceList => evidenceList;
}
