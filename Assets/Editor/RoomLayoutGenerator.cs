using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

// Generuje fizyczny układ pokoju (ściany, podłoga, sufit) na podstawie szkicu.
// Szkic jest zapisany w pikselach obrazka (oś X w prawo, oś Y w dół = do tyłu).
// Kierunek "w górę obrazka" = kierunek patrzenia kamery.
// Skala piksel -> jednostki świata liczona jest z odległości kamera -> monitor / interrogacja.
public class RoomLayoutGenerator : EditorWindow
{
    const string RootName = "RoomLayout";

    // --- punkty odniesienia ze szkicu (piksele) ---
    static readonly Vector2 CameraPx = new Vector2(253, 238);
    const float MonitorRightPx = 310 - 253;       // monitor jest na prawo od kamery
    const float InterrogationFwdPx = 238 - 197;   // interrogacja jest przed kamerą

    // --- narożniki pokoi (piksele) ---
    static readonly Vector2 A  = new Vector2(80, 183);   // lewy koniec ściany działowej
    static readonly Vector2 D  = new Vector2(325, 183);  // prawy koniec ściany działowej
    static readonly Vector2 Split = new Vector2(166, 183); // przerwa w ścianie działowej
    // Górny pokój
    static readonly Vector2 T1 = new Vector2(81, 65);
    static readonly Vector2 T2 = new Vector2(155, 27);
    static readonly Vector2 T3 = new Vector2(327, 27);
    // Dolny pokój
    static readonly Vector2 B1 = new Vector2(165, 258);
    static readonly Vector2 B2 = new Vector2(167, 383);
    static readonly Vector2 B3 = new Vector2(325, 384);

    struct Wall { public string name; public Vector2 a, b; public Wall(string n, Vector2 a, Vector2 b) { name = n; this.a = a; this.b = b; } }

    static readonly Wall[] Walls =
    {
        new Wall("Wall_Top_Left",       A,  T1),
        new Wall("Wall_Top_Diagonal",   T1, T2),
        new Wall("Wall_Top_Back",       T2, T3),
        new Wall("Wall_Top_Right",      T3, D),
        new Wall("Wall_Divider_Left",   A,  Split),
        new Wall("Wall_Divider_Right",  Split, D),
        new Wall("Wall_Bottom_Diagonal",A,  B1),
        new Wall("Wall_Bottom_Left",    B1, B2),
        new Wall("Wall_Bottom_Back",    B2, B3),
        new Wall("Wall_Bottom_Right",   B3, D),
    };

    static readonly Vector2[] TopRoom    = { A, T1, T2, T3, D };
    static readonly Vector2[] BottomRoom = { A, D, B3, B2, B1 };

    // --- ustawienia ---
    Camera cam;
    Transform monitor;
    Transform interrogation;
    bool autoScale = true;
    float manualScale = 10f;          // jednostek świata na piksel
    float eyeHeightPx = 64f;          // ~1.6 m przy założeniu 1 px ≈ 2.5 cm
    float wallHeightPx = 112f;        // ~2.8 m
    float wallThicknessPx = 4f;       // ~10 cm

    [MenuItem("Tools/Room Layout Generator")]
    static void Open() => GetWindow<RoomLayoutGenerator>("Room Layout");

    void OnEnable() => AutoFind();

    void AutoFind()
    {
        if (cam == null) cam = Camera.main;
        if (monitor == null) monitor = GameObject.Find("Monitor1")?.transform;
        if (interrogation == null)
            interrogation = (GameObject.Find("InterrogationWindow Variant 1") ?? GameObject.Find("InterrogationSetup"))?.transform;
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("Punkty odniesienia", EditorStyles.boldLabel);
        cam = (Camera)EditorGUILayout.ObjectField("Kamera", cam, typeof(Camera), true);
        monitor = (Transform)EditorGUILayout.ObjectField("Monitor", monitor, typeof(Transform), true);
        interrogation = (Transform)EditorGUILayout.ObjectField("Interrogacja", interrogation, typeof(Transform), true);
        if (GUILayout.Button("Znajdź automatycznie")) AutoFind();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Skala", EditorStyles.boldLabel);
        autoScale = EditorGUILayout.Toggle("Auto (z monitora/interrogacji)", autoScale);
        using (new EditorGUI.DisabledScope(autoScale))
            manualScale = EditorGUILayout.FloatField("Jednostek na piksel", manualScale);
        if (cam != null) EditorGUILayout.HelpBox($"Aktualna skala: {GetScale():0.###} j/px  →  szerokość dolnego pokoju ≈ {(D.x - B2.x) * GetScale():0.#} j", MessageType.None);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Wymiary (w pikselach szkicu)", EditorStyles.boldLabel);
        eyeHeightPx = EditorGUILayout.FloatField("Wysokość oczu", eyeHeightPx);
        wallHeightPx = EditorGUILayout.FloatField("Wysokość ścian", wallHeightPx);
        wallThicknessPx = EditorGUILayout.FloatField("Grubość ścian", wallThicknessPx);

        EditorGUILayout.Space();
        using (new EditorGUI.DisabledScope(cam == null))
            if (GUILayout.Button("Generuj pokój", GUILayout.Height(30))) Generate();
    }

    // Płaski kierunek "przód" i "prawo" kamery.
    void Basis(out Vector3 fwd, out Vector3 right)
    {
        fwd = Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up).normalized;
        if (fwd.sqrMagnitude < 0.001f) fwd = Vector3.forward;
        right = Vector3.Cross(Vector3.up, fwd);
    }

    static Vector3 WorldCenter(Transform t)
    {
        var rends = t.GetComponentsInChildren<Renderer>();
        if (rends.Length > 0)
        {
            var b = rends[0].bounds;
            foreach (var r in rends) b.Encapsulate(r.bounds);
            return b.center;
        }
        if (t is RectTransform rt)
        {
            var c = new Vector3[4];
            rt.GetWorldCorners(c);
            return (c[0] + c[2]) * 0.5f;
        }
        return t.position;
    }

    float GetScale()
    {
        if (!autoScale || cam == null) return manualScale;
        Basis(out var fwd, out var right);
        var samples = new List<float>();
        if (monitor != null)
        {
            float d = Vector3.Dot(WorldCenter(monitor) - cam.transform.position, right);
            if (d > 0.001f) samples.Add(d / MonitorRightPx);
        }
        if (interrogation != null)
        {
            float d = Vector3.Dot(WorldCenter(interrogation) - cam.transform.position, fwd);
            if (d > 0.001f) samples.Add(d / InterrogationFwdPx);
        }
        if (samples.Count == 0) return manualScale;
        float sum = 0; foreach (var s in samples) sum += s;
        return sum / samples.Count;
    }

    void Generate()
    {
        float s = GetScale();
        Basis(out var fwd, out var right);
        Vector3 origin = cam.transform.position;
        float floorY = origin.y - eyeHeightPx * s;
        float height = wallHeightPx * s;
        float thick = wallThicknessPx * s;

        Vector3 ToWorld(Vector2 px, float y)
        {
            var p = origin + right * ((px.x - CameraPx.x) * s) + fwd * ((CameraPx.y - px.y) * s);
            p.y = y;
            return p;
        }

        var old = GameObject.Find(RootName);
        if (old != null) Undo.DestroyObjectImmediate(old);

        var root = new GameObject(RootName);
        Undo.RegisterCreatedObjectUndo(root, "Generate Room Layout");
        var wallsParent = new GameObject("Walls").transform; wallsParent.SetParent(root.transform, false);
        var floorsParent = new GameObject("Floors").transform; floorsParent.SetParent(root.transform, false);
        var ceilingsParent = new GameObject("Ceilings").transform; ceilingsParent.SetParent(root.transform, false);

        foreach (var w in Walls)
        {
            Vector3 a = ToWorld(w.a, floorY), b = ToWorld(w.b, floorY);
            Vector3 dir = b - a;
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = w.name;
            go.transform.SetParent(wallsParent, false);
            go.transform.position = (a + b) * 0.5f + Vector3.up * (height * 0.5f);
            go.transform.rotation = Quaternion.LookRotation(dir.normalized, Vector3.up);
            // + grubość, żeby narożniki się zamykały
            go.transform.localScale = new Vector3(thick, height, dir.magnitude + thick);
            go.isStatic = true;
        }

        var mat = GraphicsSettings.currentRenderPipeline != null
            ? GraphicsSettings.currentRenderPipeline.defaultMaterial
            : AssetDatabase.GetBuiltinExtraResource<Material>("Default-Material.mat");

        CreateSurface("Floor_TopRoom", TopRoom, floorY, false, floorsParent, mat, ToWorld);
        CreateSurface("Floor_BottomRoom", BottomRoom, floorY, false, floorsParent, mat, ToWorld);
        CreateSurface("Ceiling_TopRoom", TopRoom, floorY + height, true, ceilingsParent, mat, ToWorld);
        CreateSurface("Ceiling_BottomRoom", BottomRoom, floorY + height, true, ceilingsParent, mat, ToWorld);

        Selection.activeGameObject = root;
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(root.scene);
        Debug.Log($"[RoomLayout] Wygenerowano pokój. Skala {s:0.###} j/px, podłoga Y={floorY:0.#}, wysokość {height:0.#}.");
    }

    // Wypukły wielokąt -> płaski mesh (triangulacja wachlarzowa) z MeshColliderem.
    static void CreateSurface(string name, Vector2[] poly, float y, bool faceDown, Transform parent, Material mat,
                              System.Func<Vector2, float, Vector3> toWorld)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);
        go.isStatic = true;

        var verts = new Vector3[poly.Length];
        Vector3 center = Vector3.zero;
        for (int i = 0; i < poly.Length; i++) { verts[i] = toWorld(poly[i], y); center += verts[i]; }
        center /= poly.Length;
        go.transform.position = center;
        for (int i = 0; i < verts.Length; i++) verts[i] -= center;

        var tris = new List<int>();
        for (int i = 1; i < verts.Length - 1; i++) { tris.Add(0); tris.Add(i); tris.Add(i + 1); }

        // Ustal orientację tak, żeby normalna była w górę (podłoga) albo w dół (sufit).
        Vector3 n = Vector3.Cross(verts[1] - verts[0], verts[2] - verts[0]);
        bool up = n.y > 0;
        if (up == faceDown)
            for (int i = 0; i < tris.Count; i += 3) (tris[i + 1], tris[i + 2]) = (tris[i + 2], tris[i + 1]);

        var mesh = new Mesh { name = name, vertices = verts, triangles = tris.ToArray() };
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        go.AddComponent<MeshFilter>().sharedMesh = mesh;
        go.AddComponent<MeshRenderer>().sharedMaterial = mat;
        go.AddComponent<MeshCollider>().sharedMesh = mesh;
    }
}
