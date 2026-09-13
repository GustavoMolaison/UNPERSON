using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentGenerator : MonoBehaviour
{

    public enum DataType { Suspect }

    [Header("--- UI Hierarchy Settings ---")]
    [Tooltip("Already existing gameobject where 'contentList' will be placed.")]
    [SerializeField] private GameObject container;

    [Tooltip("GameObject where all the 'content' gameobjects will be stored.")]
    [SerializeField] private GameObject contentList;

    [Tooltip("Prefab/GameObject that will be put inside content list.")]
    [SerializeField] private GameObject content;


    [Header("--- Data Settings ---")]
    [Tooltip("Decides on what type of data we will iterate while applying data.")]
    [SerializeField] private DataType selectedType;


    

    private List<ScriptableObject> iterateOver;

    [SerializeField] private bool OffStart = false;
    [SerializeField] private bool isDataConv = false;


    // container: -> Already existing gameobject where "contentList will be placed.
    // content list: -> gameobject where all the "content" gameobject will be stored.
    // content: -> gameobject that will be put inside content list.
    //
    // selectedType: -> Decides on what type of data we will iterate while applying data using "UIDataOrigin" abstract ApplyData funciotn on "content"
    //{
    //  for enum =
    //  Suspect -> SuspectTracker.instance.currentSuspects
    //{


    public void generateContent(DataType dataType)
    {
        GameObject mother = Instantiate(contentList, container.transform, false);
        

        switch (selectedType)
        {
            case DataType.Suspect:

                iterateOver = SuspectTracker.instance.currentSuspects.ConvertAll(x => (ScriptableObject)x);


                for (int i = 0; i < iterateOver.Count; i++)
                {
                    GameObject child = Instantiate(content, mother.transform, false);
                    UIDataOrigin<Suspect> childData = child.GetComponent<UIDataOrigin<Suspect>>();
                    if (childData != null)
                    {
                        if (isDataConv)
                        {
                            Debug.Log("wit");
                        }
                        else
                        {
                            childData.ApplyData(iterateOver[i] as Suspect);
                        }
                            
                    }

                }
                break;

        }

        if (OffStart)
        {
            mother.gameObject.SetActive(false);
        }

    }

    private void Start()
    {
        generateContent(selectedType);
    }
}