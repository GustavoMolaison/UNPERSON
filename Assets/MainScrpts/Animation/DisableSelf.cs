using Unity.VisualScripting;
using UnityEngine;

public class DisableSelf : MonoBehaviour
{
    private void turnYourselfOff()
    {
        this.gameObject.SetActive(false);
    }
}
