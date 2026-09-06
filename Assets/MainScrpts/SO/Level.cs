using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "Level", menuName = "Scriptable Objects/Level")]
public class Level : ScriptableObject
{
    [Header("Suspects")]
    [SerializeField] private List<Suspect> suspectsList = new List<Suspect>();

    [Header("Evidence")]
    [SerializeField] private List<Evidence> evidenceList = new List<Evidence>();

    [Header("Game Events")]
    [SerializeField] private List<GameEvent> gameEventsList = new List<GameEvent>();

    public List<Suspect> SuspectsList => suspectsList;
    public List<Evidence> EvidenceList => evidenceList;
    public List<GameEvent> GameEventsList => gameEventsList;

    public Level runTimeLevel()
    {
        Level levelInstance = Instantiate(this);
        levelInstance.suspectsList = new List<Suspect>();
        foreach(Suspect susp in this.suspectsList)
        {
            levelInstance.suspectsList.Add(susp.CreateRuntimeInstance());
        }
        // foreach(Evidence evid in this.evidenceList)
        // {
        //     levelInstance.evidenceList.Add(evid.CreateRuntimeInstance());
        // }
        foreach(GameEvent gameEvent in this.gameEventsList)
        {
            levelInstance.gameEventsList = new List<GameEvent>();
            levelInstance.gameEventsList.Add(gameEvent.GameEventRuntime());
        }
        return levelInstance;
    }
}
