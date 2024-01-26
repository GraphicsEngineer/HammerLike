using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailCollisionDetector : MonoBehaviour
{
    public TrailRenderer trailRenderer;
    public LayerMask collisionLayer; // �浹�� ������ ���̾�
    public float checkInterval = 0.1f; // �浹 �˻� ����
    private float nextCheckTime;

    void Update()
    {
        if (Time.time >= nextCheckTime)
        {
            CheckCollision();
            nextCheckTime = Time.time + checkInterval;
        }
    }

    void CheckCollision()
    {
        Vector3[] positions = new Vector3[trailRenderer.positionCount];
        trailRenderer.GetPositions(positions);

        for (int i = 0; i < positions.Length - 1; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(positions[i], positions[i + 1] - positions[i], out hit, Vector3.Distance(positions[i], positions[i + 1]), collisionLayer))
            {
                Debug.Log("Trail collided with " + hit.collider.gameObject.name);
                // ���⼭ �浹�� ���� ó���� �����ϼ���.
            }
        }
    }
}
