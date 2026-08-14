using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DialogueTreeCreator : MonoBehaviour
{
    [SerializeField] public DialogueOption backPreFabb;
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
         
        public bool back;
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
            this.back = data.isBackOption;
            if (data == null)
            {
                Debug.LogError("DialogueOption data is null. Cannot create NodeTree.2222");
                return;
            }
            
            data.nodeTree = this;
            if(data.unlockedDialouge != null)
            {
                NodeTree hiddenNode = new NodeTree(data.unlockedDialouge);
            }

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
            if(child == null)
            {
                Debug.LogError("DZIECKO NIE ISTNIEJE");
            }
            children.Add(child);
           
            if(this.data.newDialogueSequence == null)
            {
               
            }
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
                // TU KOD SIE ZATRZYMUJE JA
                parent.AddChild(child);
                Debug.Log("ojcom robie dzieci222222222");
            }
        }

       
    }
    
    public Dictionary<Suspect, Branch> startingNodes = new Dictionary<Suspect, Branch>();
    

    public void bulidTree(List<Suspect> suspects)
    {
        NodeTree nodee = new NodeTree(backPreFabb);
        // Debug.Log(suspects.Count + " suspects found. Building dialogue trees...");
        
        foreach (Suspect suspect in suspects)
        {
            // Debug.Log(suspect.DialogueOptions.Count + " dialogue options found for suspect: " + suspect.name);
            
            foreach (DialogueOption option in suspect.DialogueOptions)
            {
                Debug.Log("Creating node tree for suspect: " + suspect.name + " with starting dialogue option: " + option.name);
                Debug.Log(option);
                NodeTree node = new NodeTree(option);
                Debug.Log(option);
                if (!startingNodes.ContainsKey(suspect))
                {
                    
                    startingNodes[suspect] = new Branch(null, backPreFabb);
                    Debug.Log("2");
                }
                startingNodes[suspect].AddToBranch(node);
                Debug.Log("4");

            }

            
        }
//         var climbers = startingNodes;

// if (climbers == null || climbers.Count == 0)
// {
//     Debug.Log("Słownik treeClimbers jest pusty!");
// }
// else
// {
//     foreach (var pair in climbers)
//     {
//         Debug.Log($"Podejrzany: {pair.Key} | Climber: {pair.Value}");
//     }
// }
    }

    public class Branch
    {
        public readonly List<NodeTree> content = new List<NodeTree>();
        public readonly List<Branch> childBranches = new List<Branch>();
        public readonly List<Branch> parentBranches = new List<Branch>();

        [SerializeField] private DialogueOption backPreFab;

        public Branch(List<NodeTree> cont = null, DialogueOption backPreFab = null)
        {
            content = cont ?? new List<NodeTree>();
            if(backPreFab.nodeTree == null){
              Debug.LogError("NULLL");  
            }
            if (backPreFab != null && backPreFab.nodeTree != null)
            {
                Debug.Log("dodjae BAck");
                content.Add(backPreFab.nodeTree);
            }

            foreach(NodeTree node in content)
            {
                if(node.children.Count > 0)
                {
                    Branch newBranch = new Branch(node.children, backPreFab);
                    this.AddChildrenToBranch(newBranch);

                }
            }
        }

        public void BuildTree(DialogueOption backPreFab)
        {
        foreach (NodeTree node in content)
        {
            if (node != null && node.children != null && node.children.Count > 0)
            {
                
                Branch childBranch = new Branch(node.children, backPreFab);
                
                
                this.AddChildrenToBranch(childBranch);

                
                childBranch.BuildTree(backPreFab);
            }
        }
        }
        public void AddToBranch(NodeTree cont)
        {
            content.Add(cont);
        }

        public void AddChildrenToBranch(Branch child)
        {
            if (!childBranches.Contains(child))
            {
                childBranches.Add(child); 
            }
            if (!child.parentBranches.Contains(this))
            {
               child.AddParentToBranch(this);
            }
        }

        public void AddParentToBranch(Branch parent)
        {
            if (!parentBranches.Contains(parent))
            {
                parentBranches.Add(parent);
            }
            if (!parent.childBranches.Contains(this))
            {
                parent.AddChildrenToBranch(this);
            }
            
        }
    }

    
    
        
    
}
