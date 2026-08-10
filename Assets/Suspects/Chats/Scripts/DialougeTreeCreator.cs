using System.Collections.Generic;
using UnityEngine;

public class DialogueTreeCreator : MonoBehaviour
{
    public static DialogueTreeCreator Instance;
    void Awake()
    {
        
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        // LevelsContentInfo.Instance.initilize();
        // MonitorCameraTracker.Instance.initilize();
        // SuspectTracker.instance.initilize();
    }
    public class NodeTree
    {
        // By adding starting dialouge option we create whole tree structture of nodes
        public DialogueOption data;
        public List<NodeTree> children;
        public List<NodeTree> parents;

        public NodeTree(DialogueOption data)
        {
            if (data == null)
            {
                Debug.LogError("DialogueOption data is null. Cannot create NodeTree.11111");
                return;
            }
            this.data = data;
            this.children = new List<NodeTree>();
            this.parents = new List<NodeTree>();
            if (data == null)
            {
                Debug.LogError("DialogueOption data is null. Cannot create NodeTree.2222");
                return;
            }
            
            data.nodeTree = this;

            foreach (DialogueOption child in data.newDialogueSequence)
            {
                if(child != null)
                {
                    NodeTree childNode = new NodeTree(child);
                    AddChild(childNode);
                }
                
                
                
            }
        }

        public void displayTree(string indent = "")
        {
            Debug.Log(indent + data.name);
            foreach (NodeTree child in children)
            {
                child.displayTree(indent + "  ");
            }
        }

        public void AddChild(NodeTree child)
        {
            children.Add(child);
            if (!this.data.newDialogueSequence.Contains(child.data))
            {
                this.data.newDialogueSequence.Add(child.data);
            }
            child.parents.Add(this);
        }

        public void RemoveChild(NodeTree child)
        {
            children.Remove(child);
            child.parents.Remove(this);
        }

        public void AddParent(NodeTree parent)
        {
            parents.Add(parent);
            parent.children.Add(this);
        }

        public void RemoveParent(NodeTree parent)
        {
            parents.Remove(parent);
            parent.children.Remove(this);
        }

        public void AddChildrenToParents( NodeTree child)
        {
            foreach(NodeTree parent in this.parents)
            {
                Debug.Log("ojcom robie dzieci");
                parent.AddChild(child);
            }
        }

       
    }
    
    public Dictionary<Suspect, List<NodeTree>> startingNodes = new Dictionary<Suspect, List<NodeTree>>();
    

    public void bulidTree(List<Suspect> suspects)
    {
        Debug.Log(suspects.Count + " suspects found. Building dialogue trees...");
        
        foreach (Suspect suspect in suspects)
        {
            Debug.Log(suspect.DialogueOptions.Count + " dialogue options found for suspect: " + suspect.name);
            
            foreach (DialogueOption option in suspect.DialogueOptions)
            {
                Debug.Log("Creating node tree for suspect: " + suspect.name + " with starting dialogue option: " + option.name);
                Debug.Log(option);
                NodeTree node = new NodeTree(option);
                Debug.Log(option);
                if (!startingNodes.ContainsKey(suspect))
                {
                    Debug.Log("2");
                    startingNodes[suspect] = new List<NodeTree>();
                    Debug.Log("3");
                }
                startingNodes[suspect].Add(node);
                Debug.Log("4");

                if(option.unlockedDialouge != null)
                {
                    NodeTree hiddenNode = new NodeTree(option.unlockedDialouge);
                    
                }
            }

            
        }
    }

    
    
        
    
}
