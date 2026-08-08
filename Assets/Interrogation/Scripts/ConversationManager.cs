using UnityEngine;

using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ConversationManager : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    
    
    public static ConversationManager Instance;
    [SerializeField] private Transform chatContainer;
    [SerializeField] private float holdThreshold = 0.3f;
    private bool pointerDown = false;
    private float holdTimer;
    private bool isHolding;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (!pointerDown)
        {
            UiDialougeManager.Instance.messageCooldown = UiDialougeManager.Instance.originalmessageCooldown;
            return;
        }  

        holdTimer += Time.deltaTime;
        if (holdTimer >= holdThreshold)
        {
            isHolding = true;
            UiDialougeManager.Instance.messageCooldown = 0.05f;
            // Pomija animacje pisania przy przytrzymaniu
            TypewriterEffect[] typewriters = chatContainer.GetComponentsInChildren<TypewriterEffect>();
            foreach (var tw in typewriters)
            {
                tw.Skip();
            }
        }

        
        
    }

    public void chatNewMess(DialogueOption dialOption)
    {
        // Debug.Log("mess:" +  messeages);
        DialogueManager.Instance.chatNewMess(dialOption);
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

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;
        holdTimer = 0f;
        isHolding = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointerDown = false;
        isHolding = false;
    }

}
