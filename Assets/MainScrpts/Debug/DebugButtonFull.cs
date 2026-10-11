using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DebugButtonFull : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    IPointerDownHandler, IPointerUpHandler,
    IPointerClickHandler
{
    private Button btn;

    void Awake()
    {
        btn = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData) => Debug.Log("<color=white>[1. HOVER ENTER]</color> Kursor wjecha³ na przycisk!");
    public void OnPointerExit(PointerEventData eventData) => Debug.Log("<color=grey>[HOVER EXIT]</color> Kursor zjecha³!");
    public void OnPointerDown(PointerEventData eventData) => Debug.Log("<color=yellow>[2. POINTER DOWN]</color> Wciœniêto mysz!");
    public void OnPointerUp(PointerEventData eventData) => Debug.Log("<color=orange>[3. POINTER UP]</color> Puszczono mysz!");

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("<color=green>[4. POINTER CLICK]</color> Unity uzna³o pe³ne klikniêcie!");
        Debug.Log($"Stan Buttona -> interactable: {btn.interactable}, OnClick listener count: {btn.onClick.GetPersistentEventCount()}");
    }
}