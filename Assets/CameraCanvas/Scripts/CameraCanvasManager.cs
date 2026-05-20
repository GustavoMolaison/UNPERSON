using System;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class CameraCanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject talkMenu;
    [HideInInspector] public bool isTalkMenuOpen = false;
     public static CameraCanvasManager Instance;
    
    

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        talkMenu.transform.localPosition = new Vector3(20, -632, 0);
    }
    public void TalkMenuButton()
    {
        if (isTalkMenuOpen == false)
        {
            
            talkMenu.transform.DOLocalMove(new Vector3(20, -200, 0), 0.5f).SetEase(Ease.OutCubic);
            isTalkMenuOpen = true;
        }
        else
        {
            talkMenu.transform.DOLocalMove(new Vector3(20, -632, 0), 0.5f).SetEase(Ease.OutCubic);
            isTalkMenuOpen = false;
        }
    }
}
