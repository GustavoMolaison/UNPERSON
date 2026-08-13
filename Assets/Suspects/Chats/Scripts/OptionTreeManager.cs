using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using NUnit.Framework.Constraints;

public class OptionTreeManager : MonoBehaviour
{
    public readonly Dictionary<Suspect, TreeClimber> treeClimbers = new Dictionary<Suspect, TreeClimber>();
    
    public static OptionTreeManager Instance;
    void Awake()
    {
        
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    
    }
    public void  initialize()
    {
        Debug.Log(SuspectTracker.instance.currentSuspects.Count);
        foreach(Suspect suspect in SuspectTracker.instance.currentSuspects)
        {
          
            Debug.Log("jedziemy222");
            treeClimbers[suspect] = new TreeClimber(suspect);
            treeClimbers[suspect].startingBranch.BuildTree(DialogueTreeCreator.Instance.backPreFabb);
        }
    }

    public class TreeClimber
    {
        public Suspect enrolledSuspect;
        public DialogueTreeCreator.Branch startingBranch;

        public DialogueTreeCreator.Branch currentBranch;


        public TreeClimber(Suspect suspect)
        {
                   
            enrolledSuspect = suspect;
            startingBranch = DialogueTreeCreator.Instance.startingNodes[suspect];
            currentBranch = DialogueTreeCreator.Instance.startingNodes[suspect];
        }

        public List<DialogueOption> DecideDirection(DialogueTreeCreator.NodeTree node)
        {
            if(currentBranch.content.Contains(node))
            {
                if(node.children.Count > 0)
                {
                    //napieramy
                    return Advance(node.children);
                }
                if(node.back)
                {
                    //cofamy
                    return FallBack();
                }
                else
                {
                  //zostajemy
                  return currentBranch.content.Select(child => child.data).ToList();
                }
            }
            else
            {
                if (node.data.hasEvidenceCheckDialougeLine)
                {
                    currentBranch.AddToBranch(node.data.unlockedDialouge.nodeTree);
                    return currentBranch.content.Select(child => child.data).ToList();
                }
                else
                {
                    Debug.LogError("Przekazana opcja nie istnieje w aktualnie dostępnych opcjach dialogowych!");
                    return null;
                }
               
            }
        }

        private List<DialogueOption> Advance(List<DialogueTreeCreator.NodeTree> childs)
        {
            foreach(DialogueTreeCreator.Branch childBranch in currentBranch.childBranches)
            {
                if(childBranch.content == childs)
                {
                    currentBranch = childBranch;
                }
            }

            
            List<DialogueOption> dataList = currentBranch.content.Select(child => child.data).ToList();
            return dataList;
        }

        private List<DialogueOption> FallBack()
        {
            List<DialogueOption> backdata = currentBranch.parentBranches[0].content.Select(child => child.data).ToList();
            currentBranch = currentBranch.parentBranches[0];
            return backdata;
        }

    }
}
