using UnityEngine;
using System.Collections;
using UnityEngine.UI;



public class HighLight : MonoBehaviour
{
    [SerializeField] private float fadeSpeed;
    private Image image;
    private Color originalColor;
    public void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void permaLight()
    {
        this.gameObject.SetActive(true);
    }
    public void disableLight()
    {
        this.gameObject.SetActive(false);
    }
    public void lightOn()
    {
        this.gameObject.SetActive(true);
        image = this.gameObject.GetComponent<Image>();
        originalColor = image.color;
        StartCoroutine(FadeOutRoutine());

    }

    IEnumerator FadeOutRoutine()
    {
        // £apiemy aktualny kolor obrazka
        Color currentColor = image.color;

        // Dopóki alfa jest wiêksza od zera, jedziemy w dó³
        while (currentColor.a > 0f)
        {
            // Matematyka: odejmujemy od alfy wartoœæ zale¿n¹ od czasu klatki i twojej zmiennej fadeSpeed
            currentColor.a -= Time.deltaTime * fadeSpeed;

            // Przypisujemy zmodyfikowany kolor z powrotem do Image (Unity nie pozwala modyfikowaæ bezpoœrednio image.color.a)
            image.color = currentColor;

            // Czekamy na kolejn¹ klatkê (dziêki temu gra siê nie zawiesi i przejœcie bêdzie p³ynne)
            yield return null;
        }

        // Po wyjœciu z pêtli upewniamy siê, ¿e alfa to równe 0
        currentColor.a = 0f;
        

        // Na koniec wy³¹czamy komponent Image, tak jak chcia³eœ
        //image.enabled = false;
        image.color = originalColor;

        // Gdybyœ chcia³ wy³¹czyæ CA£Y obiekt (GameObject), odkomentuj linijkê ni¿ej:
         this.gameObject.SetActive(false);
    }

}
