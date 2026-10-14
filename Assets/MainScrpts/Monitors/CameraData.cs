using UnityEngine;
using Facing = CameraFocusTarget.Facing;

public class CameraData
{
    public MonoBehaviour monitorScript;
    public bool active;
    public bool zoomed;

    // 0 = odległość dobierana automatycznie, żeby obiekt wypełnił kadr
    public float distanceFromMonitor;
    public bool isMonitor;

    // Używane, gdy na obiekcie nie ma CameraFocusTarget (albo ma facing = Auto)
    public Facing defaultFacing;

    private static readonly Vector3[] axes = { Vector3.right, Vector3.up, Vector3.forward };

    public CameraData(MonoBehaviour monitorScript, bool active, bool zoom, bool isMonitor, float distanceFromMonitor = 0f, Facing defaultFacing = Facing.Auto)
    {
        this.monitorScript = monitorScript;
        this.active = active;
        this.zoomed = zoom;
        this.distanceFromMonitor = distanceFromMonitor;
        this.isMonitor = isMonitor;
        this.defaultFacing = defaultFacing;
    }

    /// <summary>
    /// Liczy pozycję i obrót kamery tak, żeby patrzyła prosto na obiekt i obejmowała go w kadrze.
    /// Liczone z aktualnego transformu, więc przesunięty/obrócony obiekt daje od razu nowy kadr.
    /// referencePoint (miejsce przy biurku) wybiera stronę obiektu przy facing = Auto.
    /// </summary>
    public bool TryGetViewPose(Camera cam, Vector3 referencePoint, out Vector3 position, out Quaternion rotation, out float orthoSize)
    {
        position = Vector3.zero;
        rotation = Quaternion.identity;
        orthoSize = cam.orthographicSize;

        if (monitorScript == null) return false;

        Transform t = monitorScript.transform;
        monitorScript.TryGetComponent(out CameraFocusTarget settings);
        bool isCanvas = t is RectTransform;

        if (!TryGetLocalBounds(t, settings, out Bounds bounds)) return false;

        Facing facing = settings != null && settings.facing != Facing.Auto ? settings.facing : defaultFacing;
        if (facing == Facing.Auto && isCanvas) facing = Facing.Back; // UI jest czytelne patrząc wzdłuż +Z canvasu

        Vector3 worldSize = Vector3.Scale(bounds.size, Abs(t.lossyScale));

        // Oś normalna (prostopadła do "ekranu") i jej zwrot
        int normalAxis;
        float sign;
        if (facing == Facing.Auto)
        {
            normalAxis = 0;
            for (int i = 1; i < 3; i++)
                if (worldSize[i] < worldSize[normalAxis]) normalAxis = i;

            Vector3 toReference = referencePoint - t.TransformPoint(bounds.center);
            sign = Vector3.Dot(t.TransformDirection(axes[normalAxis]), toReference) >= 0f ? 1f : -1f;
        }
        else
        {
            GetAxis(facing, out normalAxis, out sign);
        }

        Vector3 normal = t.TransformDirection(axes[normalAxis] * sign).normalized;
        Vector3 faceCenterLocal = bounds.center + axes[normalAxis] * (sign * bounds.extents[normalAxis]);
        Vector3 faceCenter = t.TransformPoint(faceCenterLocal);

        // Oś "góra" kadru: z dwóch pozostałych osi ta, która jest bliższa pionowi świata
        int a = (normalAxis + 1) % 3;
        int b = (normalAxis + 2) % 3;
        Vector3 dirA = t.TransformDirection(axes[a]).normalized;
        Vector3 dirB = t.TransformDirection(axes[b]).normalized;
        int upAxis = isCanvas && normalAxis == 2 ? 1 : (Mathf.Abs(Vector3.Dot(dirA, Vector3.up)) >= Mathf.Abs(Vector3.Dot(dirB, Vector3.up)) ? a : b);
        Vector3 up = upAxis == a ? dirA : dirB;
        if (Vector3.Dot(up, Vector3.up) < 0f && !isCanvas) up = -up;
        int sideAxis = upAxis == a ? b : a;

        float halfHeight = worldSize[upAxis] * 0.5f;
        float halfWidth = worldSize[sideAxis] * 0.5f;
        float padding = settings != null ? settings.padding : 1.05f;

        Quaternion baseRotation = Quaternion.LookRotation(-normal, up);

        float distance = settings != null && settings.distance > 0f ? settings.distance : distanceFromMonitor;
        if (cam.orthographic)
        {
            orthoSize = Mathf.Max(halfHeight, halfWidth / cam.aspect) * padding;
            if (distance <= 0f) distance = Mathf.Max(halfHeight, halfWidth) + cam.nearClipPlane;
        }
        else if (distance <= 0f)
        {
            float tanHalfFov = Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
            distance = Mathf.Max(halfHeight / tanHalfFov, halfWidth / (tanHalfFov * cam.aspect)) * padding;
        }

        Vector3 positionOffset = settings != null ? settings.positionOffset : Vector3.zero;
        Vector3 rotationOffset = settings != null ? settings.rotationOffset : Vector3.zero;

        position = faceCenter + normal * distance + baseRotation * positionOffset;
        rotation = baseRotation * Quaternion.Euler(rotationOffset);
        return true;
    }

    // Granice obiektu w lokalnym układzie t (niezależne od obrotu, w przeciwieństwie do Renderer.bounds)
    private static bool TryGetLocalBounds(Transform t, CameraFocusTarget settings, out Bounds bounds)
    {
        bounds = default;

        if (t is RectTransform rectTransform)
        {
            Rect rect = rectTransform.rect;
            bounds = new Bounds(rect.center, new Vector3(rect.width, rect.height, 0f));
            return true;
        }

        Renderer[] renderers = settings != null && settings.focusRenderer != null
            ? new[] { settings.focusRenderer }
            : t.GetComponentsInChildren<Renderer>();

        bool found = false;
        foreach (Renderer renderer in renderers)
        {
            if (!renderer.enabled || renderer is ParticleSystemRenderer) continue;

            Bounds rb = renderer.localBounds;
            Transform rt = renderer.transform;
            for (int i = 0; i < 8; i++)
            {
                Vector3 corner = rb.center + Vector3.Scale(rb.extents, new Vector3((i & 1) == 0 ? -1 : 1, (i & 2) == 0 ? -1 : 1, (i & 4) == 0 ? -1 : 1));
                Vector3 local = t.InverseTransformPoint(rt.TransformPoint(corner));
                if (!found) { bounds = new Bounds(local, Vector3.zero); found = true; }
                else bounds.Encapsulate(local);
            }
        }

        if (!found) Debug.LogWarning($"CameraData: {t.name} nie ma ani RectTransform, ani Renderera - nie da się wykadrować.");
        return found;
    }

    private static void GetAxis(Facing facing, out int axis, out float sign)
    {
        switch (facing)
        {
            case Facing.Right: axis = 0; sign = 1f; break;
            case Facing.Left: axis = 0; sign = -1f; break;
            case Facing.Up: axis = 1; sign = 1f; break;
            case Facing.Down: axis = 1; sign = -1f; break;
            case Facing.Back: axis = 2; sign = -1f; break;
            default: axis = 2; sign = 1f; break;
        }
    }

    private static Vector3 Abs(Vector3 v) => new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
}
