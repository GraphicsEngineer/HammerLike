using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    public Material[] hitMaterials; // �ǰ� �� ������ ��Ƽ���� �迭
    private Material[] originalMaterials; // ���� ��Ƽ������ ������ �迭
    public SkinnedMeshRenderer skinnedMeshRenderer; // SkinnedMeshRenderer ������Ʈ

    private void Awake()
    {
        // SkinnedMeshRenderer ������Ʈ�� ã�� �Ҵ�
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        // ������ ��Ƽ������ �����Ͽ� ����
        originalMaterials = skinnedMeshRenderer.materials;
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
        skinnedMeshRenderer.materials = hitMaterials;
        // 1�� ���
        yield return new WaitForSeconds(0.3f);
        // ���� ��Ƽ����� ����
        skinnedMeshRenderer.materials = originalMaterials;
    }

}