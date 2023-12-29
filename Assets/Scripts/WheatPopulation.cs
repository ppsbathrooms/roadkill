using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class WheatPopulation : MonoBehaviour
{
    [SerializeField]

    private GameObject parentObject;
    private Vector2[] currentPoints;
    [SerializeField]
    private GameObject[] prefab;
    [SerializeField]
    private float wheatDensity;

    public void createWheatinShape(Vector2[] pointsToCreate, GameObject parent, float _wheatDensity, GameObject[] _prefab)
    {
        currentPoints = pointsToCreate;
        parentObject = parent;
        prefab = _prefab;
        wheatDensity = _wheatDensity;

        if (parentObject == null)
        {
            parentObject = new GameObject("wheatField");
        }

        GenerateWheat();
    }

    void GenerateWheat()
    {
        if (currentPoints != null && currentPoints.Length > 2)
        {
            ClearWheat();

            Bounds bounds = CalculateBounds(currentPoints);

            int wheatCreated = 0;
            int maxAttempts = 1000;

            while (wheatCreated < wheatDensity && wheatCreated < maxAttempts)
            {
                Vector3 randomPoint = new Vector3(
                    Random.Range(bounds.min.x, bounds.max.x),
                    0f,
                    Random.Range(bounds.min.y, bounds.max.y)
                );

                if (IsPointInsidePolygon(randomPoint, currentPoints))
                {
                    GameObject wheat = Instantiate(prefab[Random.Range(0, prefab.Length)]);
                    wheat.transform.position = randomPoint;
                    wheat.transform.rotation = Quaternion.Euler(-90, Random.Range(0, 180), 0);
                    wheat.transform.parent = parentObject.transform;
                    wheat.transform.localScale = new Vector3(Random.Range(80, 120), Random.Range(80, 120), Random.Range(80, 120));
                    wheatCreated++;
                }
            }
        }
    }

    void ClearWheat()
    {
        if (parentObject != null)
        {
            foreach (Transform child in parentObject.transform)
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }

    Bounds CalculateBounds(Vector2[] points)
    {
        Bounds bounds = new Bounds(points[0], Vector3.zero);
        for (int i = 1; i < points.Length; i++)
        {
            bounds.Encapsulate(points[i]);
        }
        return bounds;
    }

    bool IsPointInsidePolygon(Vector3 point, Vector2[] polygonPoints)
    {
        int j = polygonPoints.Length - 1;
        bool inside = false;

        for (int i = 0; i < polygonPoints.Length; j = i++)
        {
            if (((polygonPoints[i].y <= point.z && point.z < polygonPoints[j].y) ||
                 (polygonPoints[j].y <= point.z && point.z < polygonPoints[i].y)) &&
                (point.x < (polygonPoints[j].x - polygonPoints[i].x) * (point.z - polygonPoints[i].y) / (polygonPoints[j].y - polygonPoints[i].y) + polygonPoints[i].x))
            {
                inside = !inside;
            }
        }
        return inside;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(WheatPopulation))]
    public class WheatPopulationEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            WheatPopulation wheatPopulation = (WheatPopulation)target;

            if (GUILayout.Button("Regenerate Wheat"))
            {
                wheatPopulation.ClearWheat();
                wheatPopulation.GenerateWheat();
            }
        }
    }
#endif
}
