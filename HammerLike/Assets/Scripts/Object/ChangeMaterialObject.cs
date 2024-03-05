using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialObject : MonoBehaviour
{
    public Material[] hitMaterials; // �ǰ� �� ������ ��Ƽ���� �迭
    private Material[] originalMaterials; // ���� ��Ƽ������ ������ �迭
    public MeshRenderer meshRenderer; // MeshRenderer ������Ʈ

    private void Awake()
    {
        // MeshRenderer ������Ʈ�� ã�� �Ҵ�
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        // ������ ��Ƽ������ �����Ͽ� ����
        originalMaterials = meshRenderer.materials;
    }

    // �ǰ� �� ȣ��� �޼���
    public void OnHit()
    {
        // �ǰ� ��Ƽ����� �����ϴ� �ڷ�ƾ ȣ��
        StartCoroutine(ChangeMaterialsTemporarily());
    }

    // ��Ƽ������ ��� �����ϰ� ������� �����ϴ� �ڷ�ƾ
    IEnumerator ChangeMaterialsTemporarily()
    {
        meshRenderer.materials = hitMaterials;
        // 0.3�� ���
        yield return new WaitForSeconds(0.3f);
        // ���� ��Ƽ����� ����
        meshRenderer.materials = originalMaterials;
    }
}
