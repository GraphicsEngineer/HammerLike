using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class DeformOnCollision : MonoBehaviour
{
    private Mesh mesh;
    private Vector3[] originalVertices;
    private Vector3[] currentVertices;

    private void Start()
    {
        Debug.Log("Start method called");
        mesh = GetComponent<MeshFilter>().mesh;
        originalVertices = mesh.vertices;
        currentVertices = mesh.vertices;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �浹 ����Ʈ�� ���� ����� ���ؽ��� ã�� ����
        foreach (ContactPoint contact in collision.contacts)
        {
            for (int i = 0; i < originalVertices.Length; i++)
            {
                float distance = Vector3.Distance(transform.TransformPoint(currentVertices[i]), contact.point);
                if (distance < 0.1f)  // �� ���� ������ �ݰ��� ����
                {
                    Vector3 direction = currentVertices[i] - transform.InverseTransformPoint(contact.point);
                    currentVertices[i] -= direction * 0.5f;  // 0.1f�� ������ ����
                }
            }
        }

        mesh.vertices = currentVertices;
        mesh.RecalculateNormals();  // ��� ����
    }
}
