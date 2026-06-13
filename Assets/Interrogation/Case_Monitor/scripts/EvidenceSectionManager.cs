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
    }

    [SerializeField] public TextMeshProUGUI title;
    [SerializeField] public TextMeshProUGUI cover;
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
        if (txt != null)
        {

            if (part == EvidenceElementType.Title)
            {
                title.text = txt;
            }
            if (part == EvidenceElementType.Cover)
            {
                cover.text = txt;
            }
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
        title.text = title_txt;
        cover.text = cover_txt;
        details.text = details_txt;
        img.sprite = sprite;

    }
}
