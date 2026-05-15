using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScrollRectContentGenerate : MonoBehaviour
{
    [SerializeField] private GameObject contentList;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject container;

    public enum DataType {Suspect}
    [SerializeField] private DataType selectedType;

    private List<ScriptableObject> iterateOver;

    [SerializeField] private bool OffOnStart = false;

    // Code designed for suspect ScriptableObjects
    public void generateContent(DataType dataType)
    {
        GameObject mother = Instantiate(contentList, container.transform, false);
        ScrollRect scrollRect = container.GetComponent<ScrollRect>();
        scrollRect.content = mother.GetComponent<RectTransform>();

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
                         childData.ApplyData(iterateOver[i] as Suspect);
                     }                 
                    
                }
                break;
            
          }       

        if (OffOnStart)
        {
            mother.gameObject.SetActive(false);
        }

    }

    private void Start()
    {
        generateContent(selectedType);
    }
}
