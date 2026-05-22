using TMPro;
using UnityEngine;

public class DialougeOptionManager : MonoBehaviour
{
    public static DialougeOptionManager Instance;
    public GameObject dialougePickWindow;
    

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

   

    public void dialougesChange()
    {


        
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            
            
                Destroy(transform.GetChild(i).gameObject);
            
        }
    

        Suspect intSusp = InterrogationManager.Instance.interrogatedSuspect;
        for (int i = 0; i < intSusp.DialogueOptions.Count; i++)
        {
            
            GameObject window = Instantiate(dialougePickWindow, transform, false);
            DialougeOptionWindow windowScript = window.GetComponent<DialougeOptionWindow>();
            //Debug.Log(intSusp.DialogueOptions[i].dialougeTittle);
            windowScript.enrollDialouge(intSusp.DialogueOptions[i]);

        }

        
    }


}
