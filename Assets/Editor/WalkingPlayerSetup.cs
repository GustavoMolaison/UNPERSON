using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Tworzy obiekt Player (na podłodze pod kamerą) i oznacza biurko/monitor jako DeskInteractable.
public static class WalkingPlayerSetup
{
    static readonly string[] DeskObjects = { "Monitor1", "desk_table" };

    [MenuItem("Tools/Setup Walking Player")]
    static void Setup()
    {
        var cam = Camera.main;
        if (cam == null) { Debug.LogError("[Setup] Brak Main Camera w scenie."); return; }

        Physics.SyncTransforms();
        Vector3 camPos = cam.transform.position;
        float eyeHeight;
        Vector3 feet;
        if (Physics.Raycast(camPos, Vector3.down, out var hit, Mathf.Infinity, ~0, QueryTriggerInteraction.Ignore))
        {
            feet = hit.point;
            eyeHeight = camPos.y - hit.point.y;
        }
        else
        {
            Debug.LogError("[Setup] Pod kamerą nie ma podłogi. Najpierw wygeneruj pokój (Tools/Room Layout Generator).");
            return;
        }

        var player = Object.FindFirstObjectByType<PlayerController>();
        GameObject go;
        if (player != null)
        {
            go = player.gameObject;
            Undo.RecordObject(go.transform, "Setup Walking Player");
        }
        else
        {
            go = new GameObject("Player");
            Undo.RegisterCreatedObjectUndo(go, "Setup Walking Player");
            player = go.AddComponent<PlayerController>();
        }
        go.transform.position = feet;

        var so = new SerializedObject(player);
        so.FindProperty("eyeHeight").floatValue = eyeHeight;
        so.ApplyModifiedProperties();
        var cc = go.GetComponent<CharacterController>();
        Undo.RecordObject(cc, "Setup Walking Player");
        PlayerController.ConfigureController(cc, eyeHeight);

        foreach (var name in DeskObjects)
        {
            var desk = GameObject.Find(name);
            if (desk == null) { Debug.LogWarning($"[Setup] Nie znaleziono obiektu '{name}'."); continue; }
            if (desk.GetComponent<DeskInteractable>() == null) Undo.AddComponent<DeskInteractable>(desk);
            if (desk.GetComponentInChildren<Collider>() == null) AddFittedBoxCollider(desk);
        }

        Selection.activeGameObject = go;
        EditorSceneManager.MarkSceneDirty(go.scene);
        Debug.Log($"[Setup] Player gotowy. Wysokość oczu = {eyeHeight:0.##} jednostek.");
    }

    // BoxCollider obejmujący wszystkie renderery obiektu (w jego lokalnym układzie).
    static void AddFittedBoxCollider(GameObject go)
    {
        var renderers = go.GetComponentsInChildren<Renderer>();
        var box = Undo.AddComponent<BoxCollider>(go);
        if (renderers.Length == 0) return;

        var t = go.transform;
        bool first = true;
        Bounds local = default;
        foreach (var r in renderers)
        {
            var b = r.bounds;
            for (int i = 0; i < 8; i++)
            {
                var corner = new Vector3(
                    (i & 1) == 0 ? b.min.x : b.max.x,
                    (i & 2) == 0 ? b.min.y : b.max.y,
                    (i & 4) == 0 ? b.min.z : b.max.z);
                var p = t.InverseTransformPoint(corner);
                if (first) { local = new Bounds(p, Vector3.zero); first = false; }
                else local.Encapsulate(p);
            }
        }
        box.center = local.center;
        box.size = local.size;
    }
}
