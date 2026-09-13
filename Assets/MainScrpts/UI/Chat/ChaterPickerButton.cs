using UnityEngine;

public class ChaterPickerButton : MonoBehaviour
{

    
    public void chatterButton()
    {
        Screen2.Instance.convChatOfOn(this.gameObject);
    }
}
