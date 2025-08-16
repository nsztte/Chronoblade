using UnityEngine;
using UnityEditor;

public class WaypointLineGenerator : EditorWindow
{
    Transform startPoint;
    Transform endPoint;
    int waypointCount = 5;

    [MenuItem("Tools/Waypoint Line Generator")]
    public static void ShowWindow()
    {
        GetWindow<WaypointLineGenerator>("Waypoint Line Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Waypoint Generator", EditorStyles.boldLabel);

        startPoint = (Transform)EditorGUILayout.ObjectField("Start Point", startPoint, typeof(Transform), true);
        endPoint = (Transform)EditorGUILayout.ObjectField("End Point", endPoint, typeof(Transform), true);
        waypointCount = EditorGUILayout.IntField("Number of Waypoints", waypointCount);

        if (GUILayout.Button("Generate Waypoints"))
        {
            if (startPoint == null || endPoint == null || waypointCount <= 0)
            {
                Debug.LogWarning("Invalid input.");
                return;
            }

            GenerateWaypoints();
        }
    }

    private void GenerateWaypoints()
    {
        Vector3 direction = (endPoint.position - startPoint.position).normalized;
        float totalDistance = Vector3.Distance(startPoint.position, endPoint.position);
        float spacing = totalDistance / (waypointCount + 1);

        GameObject mazeParent = GameObject.Find("Maze");
        GameObject parent = new GameObject("WaypointLine");

        if (mazeParent != null)
        {
            parent.transform.parent = mazeParent.transform;
        }

        for (int i = 1; i <= waypointCount; i++)
        {
            Vector3 pos = startPoint.position + direction * spacing * i;
            GameObject point = new GameObject($"Waypoint_{i}");
            point.transform.position = pos;
            point.transform.parent = parent.transform;
        }

        Debug.Log($"Generated {waypointCount} waypoints between {startPoint.name} and {endPoint.name}.");
    }
}
