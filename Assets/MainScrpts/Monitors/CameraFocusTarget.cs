using UnityEngine;

// Opcjonalne ustawienia kadru dla obiektu, na który patrzy kamera biurka (canvas, monitor, szyba...).
// Bez tego komponentu kamera sama liczy kadr z RectTransform (canvas) albo z Rendererów (obiekt 3D).
public class CameraFocusTarget : MonoBehaviour
{
    // Strona obiektu, z której patrzy kamera (oś lokalna, w którą "patrzy" ekran)
    public enum Facing { Auto, Forward, Back, Right, Left, Up, Down }

    [Tooltip("Auto: canvas -> Back, obiekt 3D -> najcieńsza oś, od strony biurka")]
    public Facing facing = Facing.Auto;

    [Tooltip("Renderer do kadrowania (np. sama tafla ekranu). Puste = wszystkie aktywne Renderery w dzieciach")]
    public Renderer focusRenderer;

    [Tooltip("Odległość kamery od powierzchni. 0 = dopasuj tak, żeby obiekt wypełnił kadr")]
    public float distance = 0f;

    [Tooltip("Margines przy automatycznej odległości (1 = obiekt na styk z krawędziami ekranu)")]
    public float padding = 1.05f;

    [Tooltip("Przesunięcie kamery w osiach widoku (x = prawo, y = góra, z = do przodu)")]
    public Vector3 positionOffset;

    [Tooltip("Dodatkowy obrót kamery (stopnie)")]
    public Vector3 rotationOffset;
}
