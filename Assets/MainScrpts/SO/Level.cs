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
    private List<GameEvent> gameEventsList = new List<GameEvent>();

    [SerializeField] private List<GameEvent> supervisorEventsList = new List<GameEvent>();

    [SerializeField] private List<GameEvent> tutorialEventsList = new List<GameEvent>();

    public List<Suspect> SuspectsList => suspectsList;
    public List<Evidence> EvidenceList => evidenceList;
    public List<GameEvent> GameEventsList => gameEventsList;
    public List<GameEvent> SupervisorEventsList => supervisorEventsList;
    public List<GameEvent> TutorialEventsList => tutorialEventsList;



    public Level runTimeLevel()
    {
        gameEventsList.Clear();
        gameEventsList.Capacity = supervisorEventsList.Count + tutorialEventsList.Count;
        gameEventsList.AddRange(supervisorEventsList);
        gameEventsList.AddRange(tutorialEventsList);


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
        levelInstance.gameEventsList = new List<GameEvent>();
        foreach(GameEvent gameEvent in this.gameEventsList)
        {
            
            levelInstance.gameEventsList.Add(gameEvent.GameEventRuntime());
        }
        return levelInstance;
    }
}
