using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class TrailToMesh : MonoBehaviour
{
    private TrailRenderer trailRenderer;
    private Mesh mesh;
    private List<Vector3> vertices;
    private List<int> triangles;
    private float trailLifeTime;
    private List<float> segmentLifetimes;

    void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        trailLifeTime = trailRenderer.time;
        segmentLifetimes = new List<float>();
        vertices = new List<Vector3>();
        triangles = new List<int>();
    }

    void Update()
    {
        UpdateTrailMesh();
        RemoveOldSegments();
    }

    void UpdateTrailMesh()
    {
        int segments = trailRenderer.positionCount;
        segments = Mathf.Min(segments, 10000);

        if (segments < 2) return;

        vertices.Clear();
        triangles.Clear();
        segmentLifetimes.Add(trailLifeTime);

        Vector3 trailPosition = transform.position;
        for (int i = 0; i < segments; i++)
        {
            Vector3 position = trailRenderer.GetPosition(i) - trailPosition;
            vertices.Add(position + Vector3.left * trailRenderer.startWidth * 0.5f);
            vertices.Add(position + Vector3.right * trailRenderer.startWidth * 0.5f);

            if (i < segments - 1)
            {
                int startIndex = i * 2;
                triangles.Add(startIndex);
                triangles.Add(startIndex + 1);
                triangles.Add(startIndex + 2);

                triangles.Add(startIndex + 2);
                triangles.Add(startIndex + 1);
                triangles.Add(startIndex + 3);
            }
        }

        mesh.Clear();
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();

        MeshCollider meshCollider = GetComponent<MeshCollider>();
        if (meshCollider != null)
        {
            meshCollider.sharedMesh = mesh;
        }
    }

    void RemoveOldSegments()
    {
        if (segmentLifetimes.Count == 0) return;

        for (int i = segmentLifetimes.Count - 1; i >= 0; i--)
        {
            segmentLifetimes[i] -= Time.deltaTime;
            if (segmentLifetimes[i] <= 0)
            {
                segmentLifetimes.RemoveAt(i);
                RemoveSegmentMeshData(i);
            }
        }
    }

    void RemoveSegmentMeshData(int segmentIndex)
    {
        int vertexIndex = segmentIndex * 2;
        if (vertexIndex < vertices.Count - 2)
        {
            // ���׸�Ʈ�� �ش��ϴ� �� ���� ���ؽ��� ����
            vertices.RemoveAt(vertexIndex);
            vertices.RemoveAt(vertexIndex);
        }
        else if (vertexIndex <= vertices.Count - 2)
        {
            // ������ �� ���ؽ��� ���� (������ ���׸�Ʈ)
            vertices.RemoveAt(vertices.Count - 1);
            vertices.RemoveAt(vertices.Count - 1);
        }

        UpdateTriangleIndices(); // �ﰢ�� �ε����� ������Ʈ

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();
    }

    void UpdateTriangleIndices()
    {
        triangles.Clear();

        // ���ؽ� �迭�� ���� �ﰢ�� �迭�� �籸��
        for (int i = 0; i < vertices.Count / 2 - 1; i++)
        {
            int startIndex = i * 2;
            triangles.Add(startIndex);
            triangles.Add(startIndex + 1);
            triangles.Add(startIndex + 2);

            triangles.Add(startIndex + 2);
            triangles.Add(startIndex + 1);
            triangles.Add(startIndex + 3);
        }
    }


}
