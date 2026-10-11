using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 100f;
    [HideInInspector] public bool mouseMovementAllowed;
    private bool wasMouseAllowedLastFrame = false;

    private float pitch = 0f; // Góra / dó³ (oœ X)
    private float yaw = 0f;   // Lewo / prawo (oœ Y)
    private quaternion rotationSave;


    public static MouseLook Instance; // Statyczna referencja do instancji

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined; // Kursor nie ucieka z okna gry
        Cursor.visible = true;                      // Kursor jest widoczny

        Vector3 currentAngles = transform.localEulerAngles;
        yaw = currentAngles.y;
        pitch = currentAngles.x;
    }

    void Update()
    {
        if (mouseMovementAllowed)
        {
            // 1. Jeœli sterowanie w³aœnie zosta³o w³¹czone, natychmiast zczytaj pozycjê z cam movera
            if (!wasMouseAllowedLastFrame)
            {
                Vector3 current = transform.localEulerAngles;

                yaw = current.y;

                // Unity zwraca k¹ty w zakresie 0..360, wiêc musimy przekonwertowaæ oœ X na zakres -180..180 dla Clamp
                pitch = current.x > 180f ? current.x - 360f : current.x;
            }

            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            yaw += mouseX;
            pitch -= mouseY;

            pitch = Mathf.Clamp(pitch, -90f, 90f);

            transform.localRotation = Quaternion.Euler(pitch, yaw, 0f);
        }

        // Zapamiêtujemy stan na koniec klatki
        wasMouseAllowedLastFrame = mouseMovementAllowed;
    }


    public void mouseMovementUnlock()
    {
        mouseMovementAllowed = !mouseMovementAllowed;

        if(mouseMovementAllowed == true)
        {
            rotationSave = transform.localRotation;
            CameraMover.Instance.isMoving = false;
        }
        else
        {
            transform.localRotation = rotationSave;
            CameraMover.Instance.isMoving = true;
        }
    }
}

