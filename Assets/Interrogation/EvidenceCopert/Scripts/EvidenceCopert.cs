using System;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class EvidenceCopert : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
    private Vector3 positionVelocity;
    [SerializeField] private Vector3 baseTargetPosition;
    private Vector3 targetPosition;
    [SerializeField] private float baseTargetLocalSize;
    private Vector3 targetLocalSize;
    [SerializeField] private float smoothTime = 5f;
    
    [Header("Hold settings")]
    [SerializeField] public float holdDuration = 0.5f; 
    private bool isPointerDown = false;
    private float pointerDownTimer = 0f;
    private bool hasTriggeredHold = false;
    [SerializeField] public float holdTreshTime = 0.2f;

    [Header("Mouse follow settings")]
    private Camera eventCamera;
    private RectTransform rectTransform;
    private RectTransform parentRectTransform;
    private Canvas parentCanvas;
    private Vector2 pointerOffset;

    [Header("Object State")]
    private bool open = false;

    [Header("Closing settings")]
    [Range(0.1f, 0.5f)] 
    public float leftZoneThreshold = 0.1f; // Pierwsze 30% szerokości uznawane za lewą krawędź
    public float minDragDistance = 500f;
    private bool isDragValid = false;
    private Vector2 startDragPosition;
    private Animator animator;
    
    [Header("Children")]
    VerticalLayoutGroup markslayout;
    
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentRectTransform = transform.parent as RectTransform;
        parentCanvas = GetComponentInParent<Canvas>();
        animator = GetComponent<Animator>();

        markslayout = GetComponentInChildren<VerticalLayoutGroup>();
        markslayout.gameObject.SetActive(false);
    }

    // public void onClick()
    // {
        
    //     if (!isPointerDown && open == false)
    //     {
    //        open = true;
    //        targetPosition = baseTargetPosition; 
    //        targetLocalSize = baseTargetLocalSize * Vector3.one;
    //     }
        
    // } 
    
    public void Start()
    {
        targetLocalSize =  transform.localScale;
        targetPosition = transform.localPosition;
    }
    public void Update()
    {
        if (isPointerDown == true && isDragValid == false)
        {
            

            // Przeliczamy pozycję myszy na przestrzeń rodzica
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRectTransform,
                Input.mousePosition,
                eventCamera,
                out Vector2 localPoint))
            {
                // Pozycja lokalna z uwzględnieniem offsetu kliknięcia
                Vector2 calc = localPoint - pointerOffset;
                // Vector2 calc = localPoint;
                Vector3 mouseTargetPosition = new Vector3(calc.x, calc.y, transform.localPosition.z); 
                transform.localPosition = mouseTargetPosition;
                targetPosition = transform.localPosition;
            }
        }
        else
        {
            // transform.localPosition = Vector3.SmoothDamp(transform.localPosition, targetPosition, ref positionVelocity, smoothTime);
            transform.localScale = Vector3.SmoothDamp(transform.localScale, targetLocalSize, ref positionVelocity, smoothTime);
        }
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        pointerDownTimer = Time.time;
        hasTriggeredHold = false;

        rectTransform = GetComponent<RectTransform>(); // pobieramy bo po otwarciu zmienia sie rozmiar sprite
        parentRectTransform = transform.parent as RectTransform;

        Camera eventCamera = parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : parentCanvas.worldCamera;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRectTransform,
            eventData.position,
            eventCamera,
            out Vector2 localClickPoint))
        {
            pointerOffset = localClickPoint - (Vector2)transform.localPosition;
            
        }
        // sprawdzamy czy nie kliknieto lewego rogu zanim wylapie to on being drag to update pozwoli na male szarpniecie obiektu
        // Bo OnBeingDrag ma czasowy jak podejrzewam warunek aby sie uruchomic co daje kilka klatek gdzie isDragValid nie jest aktualizowane
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform, 
            eventData.position, 
            eventData.pressEventCamera, 
            out Vector2 localPoint))
        {
            float width = rectTransform.rect.width;
            // Przeliczenie współrzędnych z układu środkowego na zakres 0.0 (lewo) do 1.0 (prawo)
            float normalizedX = (localPoint.x + rectTransform.pivot.x * width) / width;

            // Sprawdzamy, czy kliknięcie nastąpiło przy lewej krawędzi
            if (normalizedX <= leftZoneThreshold)
            {
                isDragValid = true;
                // startDragPosition = eventData.position;
            }
            else
            {
                isDragValid = false;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        float totalHoldTime = Time.time - pointerDownTimer;
        isPointerDown = false;

        if (totalHoldTime < holdTreshTime)
        {
           if (!isPointerDown && open == false)
          {
           openCopert();
          }
        }
        

        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Sprawdzamy lokalną pozycję kliknięcia względem teczki
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform, 
            eventData.position, 
            eventData.pressEventCamera, 
            out Vector2 localPoint))
        {
            float width = rectTransform.rect.width;
            // Przeliczenie współrzędnych z układu środkowego na zakres 0.0 (lewo) do 1.0 (prawo)
            float normalizedX = (localPoint.x + rectTransform.pivot.x * width) / width;

            // Sprawdzamy, czy kliknięcie nastąpiło przy lewej krawędzi
            if (normalizedX <= leftZoneThreshold)
            {
                isDragValid = true;
                startDragPosition = eventData.position;
            }
            else
            {
                isDragValid = false;
            }
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragValid) return;

        float deltaX = eventData.position.x - startDragPosition.x;

        // Opcjonalnie: jeśli ruch idzie w prawo, możemy kontrolować klatki animacji
        if (deltaX > 0)
        {
            float progress = Mathf.Clamp01(deltaX / minDragDistance);
            // Ustawienie parametru float w Animatorze do płynnego podglądu zamykania:
            // animator.SetFloat("CloseProgress", progress);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragValid) return;

        float totalDeltaX = eventData.position.x - startDragPosition.x;

        if (totalDeltaX >= minDragDistance)
        {
            closeCopert();
        }
        else
        {
            denyClosing();
        }

        isDragValid = false;
    }

    private void closeCopert()
    {
        // Odpalenie stanu zamknięcia w Animatorze
        animator.SetBool("Open", false);
        open = false;
        targetLocalSize =  Vector3.one;
        EvidenceSectionManager.Instance.showContent(false);
        
    }

    private void denyClosing()
    {
        Debug.Log("kiedys to sie zrobi");
        // Przywrócenie stanu otwartego, jeśli gracz puścił za wcześnie
        // animator.SetTrigger("CancelClose");
    }

    private void openCopert()
    {
       animator.SetBool("Open", true);
       open = true;
       targetLocalSize = baseTargetLocalSize * Vector3.one;
       
    }
    
    // ta funkcja jest włączana w animation clip na ostatniej klatce
    private void lastFrameAction()
    {
        if (open)
        {
            markslayout.gameObject.SetActive(true);
        }  
        if (!open)
        {
            markslayout.gameObject.SetActive(false);
        }  
    }
    private void firstFrameAction()
    {
        
    }
}
