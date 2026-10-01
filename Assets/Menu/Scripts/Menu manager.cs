using UnityEngine;
using UnityEngine.SceneManagement;

public class Menumanager : MonoBehaviour
{
    public void playButton()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
