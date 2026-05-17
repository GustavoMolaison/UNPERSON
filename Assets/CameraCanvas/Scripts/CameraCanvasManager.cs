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
    public void TalkMenuButton()
    {
        if (isTalkMenuOpen == false)
        {
            
            talkMenu.transform.DOLocalMove(new Vector3(-33, -400, 0), 0.5f).SetEase(Ease.OutCubic);
            isTalkMenuOpen = true;
        }
        else
        {
            talkMenu.transform.DOLocalMove(new Vector3(-33, -936, 0), 0.5f).SetEase(Ease.OutCubic);
            isTalkMenuOpen = false;
        }
    }
}
