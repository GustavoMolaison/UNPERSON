using UnityEngine;

public class DialougeOptionSoundAnimation : MonoBehaviour
{


    public static DialougeOptionSoundAnimation instance;
    
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip optionclickedSound;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    public void optionClicked()
    {
        audioSource.PlayOneShot(optionclickedSound);
    }
}
