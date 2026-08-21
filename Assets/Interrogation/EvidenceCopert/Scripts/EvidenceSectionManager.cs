using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EvidenceSectionManager : MonoBehaviour
{
    public static EvidenceSectionManager Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        details.gameObject.SetActive(false);
        img.gameObject.SetActive(false);
    }

    

    
    
    [SerializeField] public TextMeshProUGUI details;
    [SerializeField] public Image img;

    public enum EvidenceElementType
    {
        Title,
        Cover,
        Details,
        Img
    }

    public void changeContentSep(EvidenceElementType part, string txt = null, Sprite imgg = null)
    {
        if (!details.gameObject.activeSelf)
        {
            details.gameObject.SetActive(true);
        }
        if (!img.gameObject.activeSelf)
        {
            img.gameObject.SetActive(true);
        }
       
        if (txt != null)
        {

           
            
            if (part == EvidenceElementType.Details)
            {
                details.text = txt;
            }
        }
        else
        {
            if (img != null)
            {
                img.sprite = imgg;
            }

        }
    }

    public void changeAllContent(string title_txt, string cover_txt, string details_txt, Sprite sprite)
    {
        if (!details.gameObject.activeSelf)
        {
            details.gameObject.SetActive(true);
        }
        if (!img.gameObject.activeSelf)
        {
            img.gameObject.SetActive(true);
        }

        details.text = details_txt;
        img.sprite = sprite;

    }

    public void showContent(bool hide)
    {
     details.gameObject.SetActive(hide);
     img.gameObject.SetActive(hide);
    }
}
