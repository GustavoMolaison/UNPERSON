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

    public Level runTimeLevel()
    {
        Level levelInstance = Instantiate(this);
        levelInstance.suspectsList = new List<Suspect>();
        foreach(Suspect susp in this.suspectsList)
        {
            levelInstance.suspectsList.Add(susp.CreateRuntimeInstance());
        }
        return levelInstance;
    }
}
