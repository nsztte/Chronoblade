using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class FieldOfViewCone : MonoBehaviour
{
    [SerializeField] private float viewDistance = 8f;
    [SerializeField] private float fieldOfViewAngle = 90f;
    [SerializeField] private float coneHeight = 2f;
    [SerializeField] private int segments = 30;

    private Mesh mesh;

    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void LateUpdate()
    {
        GenerateCone();
    }

private void GenerateCone()
{
    Vector3[] vertices = new Vector3[segments + 2];
    int[] triangles = new int[segments * 3];

    vertices[0] = Vector3.zero; // Tip of the cone (CCTV 위치)

    float angleStep = fieldOfViewAngle / segments;
    float startAngle = -fieldOfViewAngle / 2;

    for (int i = 0; i <= segments; i++)
    {
        float angle = startAngle + i * angleStep;
        float rad = Mathf.Deg2Rad * angle;
        Vector3 dir = new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad));
        vertices[i + 1] = dir * viewDistance + Vector3.down * coneHeight; // ← 여기가 핵심
    }

    for (int i = 0; i < segments; i++)
    {
        triangles[i * 3 + 0] = 0;         // Tip
        triangles[i * 3 + 1] = i + 1;
        triangles[i * 3 + 2] = i + 2;
    }

    mesh.Clear();
    mesh.vertices = vertices;
    mesh.triangles = triangles;
    mesh.RecalculateNormals();
}

}
