using UnityEngine;
using System.Collections.Generic;

public class ScrollRectContentGenerate : MonoBehaviour
{
    [SerializeField] private GameObject contentList;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject container;
    [SerializeField] private List<ScriptableObject> iterateOver;

    [SerializeField] private bool OffOnStart = false;


    public void generateContent()
    {
        GameObject mother = Instantiate(contentList, container.transform, false);

        for (int i = 0; i < iterateOver.Count; i++)
        {
            GameObject child = Instantiate(content, mother.transform, false);
            child.ApplyData()
        }

        if (OffOnStart)
        {
            mother.gameObject.SetActive(false);
        }

    }
}
