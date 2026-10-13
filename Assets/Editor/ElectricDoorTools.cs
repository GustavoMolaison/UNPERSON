using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Tools/Doors:
//  - Insert Electric Door In Selected Wall: wycina otwór w zaznaczonej ścianie i wstawia drzwi.
//    Panel ląduje po stronie ściany zwróconej do Main Camera (obróć drzwi o 180°, żeby zamienić strony).
//  - Rebuild Electric Door Prefab: buduje prefab od nowa (instancje w scenach się zaktualizują).
public static class ElectricDoorTools
{
    const string Folder = "Assets/Prefabs/Doors";
    const string PrefabPath = Folder + "/ElectricDoor.prefab";

    // Wymiary prefabu w metrach (skala 1)
    const float OpeningWidth = 1.0f;
    const float OpeningHeight = 2.1f;
    const float ReferenceWallHeight = 2.8f; // tyle "metrów" ma ściana z RoomLayoutGenerator
    const float PocketDepth = 0.5f;          // ile ściany potrzeba z każdej strony na schowanie skrzydła

    [MenuItem("Tools/Doors/Rebuild Electric Door Prefab")]
    static void RebuildPrefab() => BuildPrefab();

    [MenuItem("Tools/Doors/Insert Electric Door In Selected Wall")]
    static void InsertIntoWall()
    {
        var wall = Selection.activeTransform;
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);
        if (prefab == null) prefab = BuildPrefab();

        Vector3 s = wall.localScale;
        bool lengthAlongZ = s.z >= s.x;
        float length = lengthAlongZ ? s.z : s.x;
        float height = s.y;
        Vector3 lengthDir = lengthAlongZ ? wall.forward : wall.right;
        Vector3 thicknessDir = lengthAlongZ ? wall.right : wall.forward;

        float f = height / ReferenceWallHeight;
        float gap = OpeningWidth * f;
        float doorHeight = (OpeningHeight + 0.08f) * f; // z nadprożem ramy
        float side = (length - gap) * 0.5f;
        if (side < PocketDepth * f)
        {
            Debug.LogError($"[Doors] Ściana '{wall.name}' jest za krótka na drzwi (potrzeba min. {(gap + 2 * PocketDepth * f):0.#} jednostek).");
            return;
        }

        Vector3 center = wall.position;
        float floorY = center.y - height * 0.5f;
        string undoName = "Insert Electric Door";

        Transform MakePiece(string suffix, Vector3 pos, float len, float h)
        {
            var piece = Object.Instantiate(wall.gameObject, wall.parent).transform;
            piece.name = wall.name + suffix;
            piece.position = pos;
            piece.rotation = wall.rotation;
            piece.localScale = lengthAlongZ ? new Vector3(s.x, h, len) : new Vector3(len, h, s.z);
            Undo.RegisterCreatedObjectUndo(piece.gameObject, undoName);
            return piece;
        }

        float offset = (gap + side) * 0.5f;
        MakePiece("_A", center - lengthDir * offset, side, height);
        MakePiece("_B", center + lengthDir * offset, side, height);
        float lintelH = height - doorHeight;
        if (lintelH > 0.001f)
            MakePiece("_Lintel", new Vector3(center.x, floorY + doorHeight + lintelH * 0.5f, center.z), gap, lintelH);

        // Przód drzwi (strona panelu) = lokalne -Z, ma patrzeć w stronę kamery
        var cam = Camera.main;
        Vector3 toCam = cam != null ? cam.transform.position - center : -thicknessDir;
        Vector3 forward = Vector3.Dot(toCam, thicknessDir) > 0f ? -thicknessDir : thicknessDir;
        forward = Vector3.ProjectOnPlane(forward, Vector3.up).normalized;

        var door = (GameObject)PrefabUtility.InstantiatePrefab(prefab, wall.parent);
        door.name = "ElectricDoor";
        door.transform.SetPositionAndRotation(new Vector3(center.x, floorY, center.z), Quaternion.LookRotation(forward, Vector3.up));
        door.transform.localScale = Vector3.one * f;
        Undo.RegisterCreatedObjectUndo(door, undoName);

        Undo.DestroyObjectImmediate(wall.gameObject);
        Selection.activeGameObject = door;
        EditorSceneManager.MarkSceneDirty(door.scene);
        Debug.Log($"[Doors] Wstawiono drzwi (skala {f:0.###}).");
    }

    [MenuItem("Tools/Doors/Insert Electric Door In Selected Wall", true)]
    static bool InsertValidate() => Selection.activeTransform != null && Selection.activeTransform.GetComponent<MeshRenderer>() != null;

    // ---------------- budowa prefabu ----------------

    static GameObject BuildPrefab()
    {
        Directory.CreateDirectory(Folder + "/Materials");
        AssetDatabase.Refresh();

        var frameMat  = Mat("Door_Frame",  new Color(0.22f, 0.23f, 0.25f), 0.6f, 0.5f);
        var leafMat   = Mat("Door_Leaf",   new Color(0.68f, 0.70f, 0.72f), 0.5f, 0.6f);
        var stripeMat = Mat("Door_Stripe", new Color(1f, 0.72f, 0.1f), 0f, 0.4f);
        var panelMat  = Mat("Door_Panel",  new Color(0.1f, 0.1f, 0.11f), 0.3f, 0.5f);
        var buttonMat = Mat("Door_Button", new Color(0.85f, 0.85f, 0.85f), 0f, 0.3f);
        var redMat    = Mat("Door_Light_Closed", new Color(1f, 0.1f, 0.08f), 0f, 0.5f, true);
        var greenMat  = Mat("Door_Light_Open",   new Color(0.1f, 1f, 0.25f), 0f, 0.5f, true);

        const float W = OpeningWidth, H = OpeningHeight;
        var root = new GameObject("ElectricDoor");
        var audio = root.AddComponent<AudioSource>();
        audio.playOnAwake = false;
        audio.spatialBlend = 1f;

        // Rama (głębsza niż ściana, żeby zakryć łączenia)
        var frame = Empty("Frame", root.transform, Vector3.zero);
        Cube("Jamb_L", frame, new Vector3(-(W / 2 + 0.04f), (H + 0.08f) / 2, 0), new Vector3(0.08f, H + 0.08f, 0.14f), frameMat);
        Cube("Jamb_R", frame, new Vector3(W / 2 + 0.04f, (H + 0.08f) / 2, 0), new Vector3(0.08f, H + 0.08f, 0.14f), frameMat);
        Cube("Header", frame, new Vector3(0, H + 0.04f, 0), new Vector3(W + 0.16f, 0.08f, 0.14f), frameMat);
        var lightFront = Cube("Light_Front", frame, new Vector3(0, H + 0.04f, -0.072f), new Vector3(0.6f, 0.03f, 0.01f), redMat, false);
        var lightBack  = Cube("Light_Back",  frame, new Vector3(0, H + 0.04f, 0.072f), new Vector3(0.6f, 0.03f, 0.01f), redMat, false);

        // Skrzydła
        Transform Leaf(string name, float x)
        {
            var leaf = Empty(name, root.transform, new Vector3(x, 0, 0));
            var rb = leaf.gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            Cube("Body", leaf, new Vector3(0, H / 2, 0), new Vector3(W / 2, H, 0.04f), leafMat);
            Cube("Stripe", leaf, new Vector3(0, 1.0f, 0), new Vector3(W / 2, 0.12f, 0.05f), stripeMat, false);
            return leaf;
        }
        var leftLeaf = Leaf("Leaf_L", -W / 4);
        var rightLeaf = Leaf("Leaf_R", W / 4);

        // Panel (przód = -Z, ściana ma 0.1 grubości)
        var panel = Empty("Panel", root.transform, new Vector3(W / 2 + 0.35f, 1.2f, -0.065f));
        Cube("Base", panel, Vector3.zero, new Vector3(0.18f, 0.28f, 0.03f), panelMat);
        Cube("Button", panel, new Vector3(0, -0.03f, -0.02f), new Vector3(0.1f, 0.1f, 0.02f), buttonMat, false);
        var panelLight = Cube("Indicator", panel, new Vector3(0, 0.1f, -0.02f), new Vector3(0.12f, 0.03f, 0.015f), redMat, false);

        var door = root.AddComponent<ElectricDoor>();
        var so = new SerializedObject(door);
        so.FindProperty("leftLeaf").objectReferenceValue = leftLeaf;
        so.FindProperty("rightLeaf").objectReferenceValue = rightLeaf;
        so.FindProperty("openDistance").floatValue = W / 2;
        so.FindProperty("openMaterial").objectReferenceValue = greenMat;
        so.FindProperty("closedMaterial").objectReferenceValue = redMat;
        var ind = so.FindProperty("indicators");
        ind.arraySize = 3;
        ind.GetArrayElementAtIndex(0).objectReferenceValue = lightFront.GetComponent<Renderer>();
        ind.GetArrayElementAtIndex(1).objectReferenceValue = lightBack.GetComponent<Renderer>();
        ind.GetArrayElementAtIndex(2).objectReferenceValue = panelLight.GetComponent<Renderer>();
        so.ApplyModifiedPropertiesWithoutUndo();

        var panelComp = panel.gameObject.AddComponent<DoorPanel>();
        var pso = new SerializedObject(panelComp);
        pso.FindProperty("door").objectReferenceValue = door;
        pso.ApplyModifiedPropertiesWithoutUndo();

        var prefab = PrefabUtility.SaveAsPrefabAsset(root, PrefabPath);
        Object.DestroyImmediate(root);
        Debug.Log($"[Doors] Zbudowano prefab: {PrefabPath}");
        return prefab;
    }

    static Transform Empty(string name, Transform parent, Vector3 localPos)
    {
        var t = new GameObject(name).transform;
        t.SetParent(parent, false);
        t.localPosition = localPos;
        return t;
    }

    static Transform Cube(string name, Transform parent, Vector3 localPos, Vector3 size, Material mat, bool collider = true)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        if (!collider) Object.DestroyImmediate(go.GetComponent<Collider>());
        go.transform.SetParent(parent, false);
        go.transform.localPosition = localPos;
        go.transform.localScale = size;
        go.GetComponent<Renderer>().sharedMaterial = mat;
        return go.transform;
    }

    static Material Mat(string name, Color color, float metallic, float smoothness, bool emissive = false)
    {
        string path = $"{Folder}/Materials/{name}.mat";
        var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (mat != null) return mat;

        var shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null) shader = Shader.Find("Standard");
        mat = new Material(shader) { color = color };
        mat.SetFloat("_Metallic", metallic);
        mat.SetFloat("_Smoothness", smoothness);
        mat.SetFloat("_Glossiness", smoothness);
        if (emissive)
        {
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", color * 2f);
            mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
        }
        AssetDatabase.CreateAsset(mat, path);
        return mat;
    }
}
