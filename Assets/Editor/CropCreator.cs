using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class CropCreator : EditorWindow
{
    private Vector2[] corners;
    private int selectedCorner = -1;
    private bool isDragging = false;
    private Vector2 prevMousePos;
    private float handleSize = 1f;
    private GameObject[] selectedPrefabs;
    private float wheatDensity = 10f;
    private int pointCount = 4;

    private Vector2 scrollPos;

    [MenuItem("Tools/Crop Creator")]
    private static void Init()
    {
        CropCreator window = GetWindow<CropCreator>();
        window.titleContent = new GUIContent("Crop Creator");
        SceneView.duringSceneGui += window.DuringSceneGUI;
        Tools.hidden = false;
    }

    private void DuringSceneGUI(SceneView sceneView)
    {
        Event guiEvent = Event.current;

        if (guiEvent.type == EventType.MouseDrag && isDragging && selectedCorner >= 0 && selectedCorner < corners.Length)
        {
            Vector2 currentMousePos = Event.current.mousePosition;
            Ray ray = HandleUtility.GUIPointToWorldRay(currentMousePos);

            Plane plane = new Plane(Vector3.up, new Vector3(corners[selectedCorner].x, 0, corners[selectedCorner].y));
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                Vector3 newPosition = ray.GetPoint(distance);
                RecordObject("Move Corner");
                corners[selectedCorner] = new Vector2(newPosition.x, newPosition.z);
                UpdateWindow();
            }
            sceneView.Repaint();
            HandleUtility.Repaint();
        }

        for (int i = 0; i < corners.Length; i++)
        {
            Handles.color = (i == selectedCorner) ? Color.red : Color.green;
            Vector3 worldPos = new Vector3(corners[i].x, 0, corners[i].y);
            Vector3 newPos = Handles.FreeMoveHandle(worldPos, Quaternion.identity, handleSize, Vector3.one, Handles.DotHandleCap);

            if (corners[i] != new Vector2(newPos.x, newPos.z))
            {
                RecordObject("Move Corner");
                corners[i] = new Vector2(newPos.x, newPos.z);
                UpdateWindow();
            }
        }

        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0)
        {
            for (int i = 0; i < corners.Length; i++)
            {
                Vector3 screenPos = HandleUtility.WorldToGUIPoint(new Vector3(corners[i].x, 0, corners[i].y));
                float dist = Vector2.Distance(Event.current.mousePosition, screenPos);

                if (dist < handleSize * 10f)
                {
                    selectedCorner = i;
                    isDragging = true;
                    guiEvent.Use();
                    prevMousePos = Event.current.mousePosition;
                    break;
                }
            }
        }

        if (guiEvent.type == EventType.MouseUp && guiEvent.button == 0)
        {
            isDragging = false;
            selectedCorner = -1;
        }

        Handles.color = Color.green;
        for (int i = 0; i < corners.Length - 1; i++)
        {
            Handles.DrawAAPolyLine(new Vector3(corners[i].x, 0, corners[i].y), new Vector3(corners[i + 1].x, 0, corners[i + 1].y));
        }
        Handles.DrawAAPolyLine(new Vector3(corners[corners.Length - 1].x, 0, corners[corners.Length - 1].y), new Vector3(corners[0].x, 0, corners[0].y));
    }

    private void CreateWheatPopulationGameObject(Vector2[] points, float wheatDensity, GameObject[] prefab)
    {
        if (points == null || points.Length == 0)
        {
            Debug.LogWarning("No points drawn!");
            return;
        }

        GameObject wheatPopulationObject = new GameObject("wheatField");
        WheatPopulation wheatPopulationScript = wheatPopulationObject.AddComponent<WheatPopulation>();

        wheatPopulationScript.createWheatinShape(points, wheatPopulationObject, wheatDensity, prefab);
    }

    private void OnGUI()
    {
        GUILayout.Label("Coordinates:");
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(200));
        for (int i = 0; i < corners.Length; i++)
        {
            EditorGUI.BeginChangeCheck();
            Vector2 newPoint = EditorGUILayout.Vector2Field($"Point {i + 1}", corners[i]);
            if (EditorGUI.EndChangeCheck())
            {
                RecordObject("Change Corner");
                corners[i] = newPoint;
                UpdateWindow();
            }
        }
        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space();

        GUILayout.Label("Point Count");
        pointCount = EditorGUILayout.IntField(pointCount);

        if (GUILayout.Button("Apply Point Count"))
        {
            corners = new Vector2[pointCount];
            float size = 100f;
            for (int i = 0; i < pointCount; i++)
            {
                float angle = i * 2 * Mathf.PI / pointCount;
                corners[i] = new Vector2(size * Mathf.Cos(angle), size * Mathf.Sin(angle));
            }

            UpdateWindow();
        }

        GUILayout.Label("Handle Size");
        handleSize = EditorGUILayout.Slider(handleSize, 0f, 1f);


        EditorGUILayout.Space();

        GUILayout.Label("Wheat Density");
        wheatDensity = EditorGUILayout.Slider(wheatDensity, 1f, 1000f);

        EditorGUILayout.Space();

        GUILayout.Label("Wheat Prefab");

        if (selectedPrefabs == null)
        {
            selectedPrefabs = new GameObject[0];
        }

        for (int i = 0; i < selectedPrefabs.Length; i++)
        {
            selectedPrefabs[i] = (GameObject)EditorGUILayout.ObjectField("Prefab " + i, selectedPrefabs[i], typeof(GameObject), false);
        }

        if (GUILayout.Button("Add Prefab"))
        {
            ArrayUtility.Add(ref selectedPrefabs, null);
        }

        if (GUILayout.Button("Clear Prefabs"))
        {
            selectedPrefabs = new GameObject[0];
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        if (GUILayout.Button("Create Wheat Field"))
        {
            CreateWheatPopulationGameObject(corners, wheatDensity, selectedPrefabs);
        }
    }

    private void OnEnable()
    {
        corners = new Vector2[pointCount];
        float size = 100f;
        for (int i = 0; i < pointCount; i++)
        {
            float angle = i * 2 * Mathf.PI / pointCount;
            corners[i] = new Vector2(size * Mathf.Cos(angle), size * Mathf.Sin(angle));
        }

        SceneView.duringSceneGui += DuringSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= DuringSceneGUI;
    }

    private static void UpdateWindow()
    {
        CropCreator window = (CropCreator)EditorWindow.GetWindow(typeof(CropCreator));
        window.Repaint();
    }

    private void RecordObject(string actionName)
    {
        if (Selection.activeObject != null)
        {
            Undo.RecordObject(Selection.activeObject, actionName);
        }
    }
}
