using System.Collections;
using UnityEngine;

// Przełącza między chodzeniem po pokoju a trybem biurka (interrogacja + monitor).
// W trybie biurka kamerą steruje CameraMover, w trybie chodzenia - ten skrypt.
// Obiekt Player stoi stopami na podłodze, kamera jest na wysokości eyeHeight.
[DefaultExecutionOrder(100)]
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public enum Mode { Walking, Desk, Transition }

    public static PlayerController Instance;
    public Mode CurrentMode { get; private set; } = Mode.Walking;
    public bool InDesk => CurrentMode == Mode.Desk;

    [SerializeField] private bool startAtDesk = false;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Wymiary (ustawiane przez Tools/Setup Walking Player)")]
    [SerializeField] private float eyeHeight = 1.6f;

    [Header("Ruch (w wysokościach oczu, więc niezależne od skali sceny)")]
    [SerializeField] private float walkSpeed = 0.9f;          // ~1.4 m/s
    [SerializeField] private float interactDistance = 1.5f;   // ~2.4 m
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float standUpTime = 0.5f;

    private CharacterController controller;
    private Camera cam;
    private float yaw;
    private float pitch;
    private float verticalVelocity;
    private Interactable lookedAt;

    private Vector3 EyePosition => transform.position + Vector3.up * eyeHeight;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        controller = GetComponent<CharacterController>();
        cam = Camera.main;
        ConfigureController(controller, eyeHeight);
    }

    public static void ConfigureController(CharacterController cc, float eye)
    {
        cc.height = eye * 1.1f;
        cc.radius = eye * 0.18f;
        cc.center = Vector3.up * (cc.height * 0.5f);
        cc.stepOffset = eye * 0.2f;
        cc.skinWidth = cc.radius * 0.1f;
        cc.minMoveDistance = 0f;
    }

    void Start()
    {
        // Uruchamia się po Start() CameraMovera (DefaultExecutionOrder 100)
        if (startAtDesk)
        {
            CurrentMode = Mode.Desk;
            SetCursorForDesk();
        }
        else
        {
            BeginWalking(cam.transform.eulerAngles.y);
        }
    }

    void Update()
    {
        switch (CurrentMode)
        {
            case Mode.Walking:
                Look();
                Move();
                FindInteractable();
                if (lookedAt != null && Input.GetKeyDown(interactKey)) lookedAt.Interact(this);
                break;

            case Mode.Desk:
                if (Input.GetKeyDown(interactKey)) StartCoroutine(StandUp());
                break;
        }
    }

    void LateUpdate()
    {
        if (CurrentMode == Mode.Walking)
            cam.transform.SetPositionAndRotation(EyePosition, Quaternion.Euler(pitch, yaw, 0f));
    }

    private void Look()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -85f, 85f);
    }

    private void Move()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        Vector3 move = Quaternion.Euler(0f, yaw, 0f) * Vector3.ClampMagnitude(input, 1f) * (walkSpeed * eyeHeight);

        float gravity = 9.81f * (eyeHeight / 1.6f);
        if (controller.isGrounded && verticalVelocity < 0f) verticalVelocity = -0.1f * gravity;
        verticalVelocity -= gravity * Time.deltaTime;
        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);
    }

    private void FindInteractable()
    {
        lookedAt = null;
        Ray ray = new Ray(EyePosition, Quaternion.Euler(pitch, yaw, 0f) * Vector3.forward);
        var hits = Physics.RaycastAll(ray, interactDistance * eyeHeight, ~0, QueryTriggerInteraction.Collide);
        float closest = float.MaxValue;
        Collider closestCollider = null;
        foreach (var hit in hits)
        {
            if (hit.collider == controller || hit.distance >= closest) continue;
            closest = hit.distance;
            closestCollider = hit.collider;
        }
        // Tylko najbliższy obiekt - ściana zasłania to, co za nią
        if (closestCollider != null) lookedAt = closestCollider.GetComponentInParent<Interactable>();
    }

    public void SitDown()
    {
        CurrentMode = Mode.Desk;
        lookedAt = null;
        SetCursorForDesk();

        var tracker = MonitorCameraTracker.Instance;
        CameraMover.Instance.changeCamera("", tracker.currentCamera ?? tracker.inInterrogation);
    }

    private IEnumerator StandUp()
    {
        CurrentMode = Mode.Transition;
        CameraMover.Instance.isMoving = false;
        MouseLook.Instance.mouseMovementAllowed = false;

        Vector3 startPos = cam.transform.position;
        Quaternion startRot = cam.transform.rotation;
        float targetYaw = startRot.eulerAngles.y;
        Quaternion endRot = Quaternion.Euler(0f, targetYaw, 0f);

        for (float t = 0f; t < 1f; t += Time.deltaTime / standUpTime)
        {
            float k = Mathf.SmoothStep(0f, 1f, t);
            cam.transform.SetPositionAndRotation(Vector3.Lerp(startPos, EyePosition, k), Quaternion.Slerp(startRot, endRot, k));
            yield return null;
        }

        BeginWalking(targetYaw);
    }

    private void BeginWalking(float startYaw)
    {
        CurrentMode = Mode.Walking;
        yaw = startYaw;
        pitch = 0f;
        verticalVelocity = 0f;
        if (CameraMover.Instance != null) CameraMover.Instance.isMoving = false;
        if (MouseLook.Instance != null) MouseLook.Instance.mouseMovementAllowed = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private static void SetCursorForDesk()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    void OnGUI()
    {
        if (CurrentMode == Mode.Walking)
        {
            var center = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
            GUI.Label(new Rect(center.x - 5, center.y - 12, 20, 24), "•");
            if (lookedAt != null)
                GUI.Label(new Rect(center.x - 150, center.y + 20, 300, 24), $"[{interactKey}] {lookedAt.Prompt}",
                          new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter });
        }
        else if (CurrentMode == Mode.Desk)
        {
            GUI.Label(new Rect(10, Screen.height - 30, 200, 24), $"[{interactKey}] Wstań");
        }
    }
}
