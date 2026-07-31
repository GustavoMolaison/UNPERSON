using UnityEngine;

using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ConversationManager : MonoBehaviour, IPointerClickHandler
{
    
    
    public static ConversationManager Instance;
    [SerializeField] private Transform chatContainer;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void chatNewMess(List<DialogueLine> messeages)
    {
        Debug.Log("mess:" +  messeages);
        DialogueManager.Instance.chatNewMess(messeages);

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Pomija animacje pisania przy kliknięciu
        TypewriterEffect[] typewriters = chatContainer.GetComponentsInChildren<TypewriterEffect>();
        foreach (var tw in typewriters)
        {
        tw.Skip();
        }
    }



}
