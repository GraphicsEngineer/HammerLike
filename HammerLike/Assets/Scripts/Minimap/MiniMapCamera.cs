using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MiniMapCamera : MonoBehaviour
{
    private Camera cam;
    private Vector3 fixedRotation;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Start()
    {
        // �ʱ� ȸ�� ���� ����
        fixedRotation = transform.eulerAngles;
        AdjustProjectionMatrix();
    }

    void AdjustProjectionMatrix()
    {
        Matrix4x4 matrix = cam.projectionMatrix;

        // ���⿡ ���� �ְ� ����
        // �� ������ ���������� ������ �� �ֽ��ϴ�.
        float tiltCorrection = Mathf.Cos(30f * Mathf.Deg2Rad); // 30�� ����
        matrix.m11 *= 0.866025404f; // y�� ������ ���� (cos(30��))
        matrix.m22 *= 0.5f / tiltCorrection; // z�� ������ ���� (1/2�� ����)

        cam.projectionMatrix = matrix;
    }

    void LateUpdate()
    {
        // �� �����Ӹ��� ī�޶��� ȸ���� �ʱ� ������ ����
        transform.eulerAngles = fixedRotation;
    }
}
