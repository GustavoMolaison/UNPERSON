using UnityEngine;
using UnityEngine.UI;

public class DialougeOptionShader : MonoBehaviour
{
    
    

    [HideInInspector] public Material material;
    private Vector2 currentOffset = Vector2.zero;

    // Zmieniaj¹c tê wartoœæ na false z innego skryptu, p³ynnie pauzujesz animacjê
    public bool isPlaying = true;

    // To jest twój wektor z grafu (0.2, 0)
    public Vector2 speed = new Vector2(0.2f, 0f);

    private float wrongness = 0;

    
    // Materia³ jest ustalany przez Dialouge Option Window
    void Update()
    {
        if (material == null) return;

        // Jeœli animacja jest zapauzowana, omijamy dodawanie. 
        // Offset zostaje na ostatniej wyliczonej wartoœci.
        if (isPlaying)
        {
            // Dodajemy przesuniêcie oparte na up³ywie czasu z jednej klatki
            currentOffset += speed * Time.deltaTime;

            // Wysy³amy gotowy, wyliczony wektor bezpoœrednio do Shadera
            material.SetVector("_CurrentOffset", currentOffset);
        }
    }

    public void wrongAnswerReact(float percentege)
    {
        // this is percentege of how white the shader will be 100% white => dialouge blocked!
        
        
        if (percentege <= 1)
        {
            wrongness = percentege;
            Debug.Log("WRONGNESS");
            Debug.Log(wrongness);
            Debug.Log("percentege");
            Debug.Log(percentege);
            material.SetFloat("_HowManyMistakes", wrongness);
        }
        
    }
}