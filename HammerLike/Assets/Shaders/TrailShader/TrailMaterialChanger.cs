using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailMaterialChanger : MonoBehaviour
{
    public TrailRenderer trailRenderer;
    public Material[] materials; // ����� ��Ƽ���� �迭
    public float[] materialRatios; // �� ��Ƽ������ ����

    private float totalLength; // Ʈ������ �� ����

    void Start()
    {
        if (materials.Length != materialRatios.Length)
        {
            Debug.LogError("Materials and ratios length must be the same");
            return;
        }

        totalLength = trailRenderer.time;
        UpdateMaterial(0); // ������ �� ù ��° ��Ƽ����� ����
    }

    void Update()
    {
        float currentLength = CalculateCurrentLength();
        UpdateMaterial(currentLength / totalLength);
    }

    // ���� Ʈ������ ���̸� ����ϴ� �Լ�
    float CalculateCurrentLength()
    {
        // ���⼭�� ������ �ð��� �̿�������, ���� ���������� �ٸ� ����� ����� �� ����
        return Time.timeSinceLevelLoad % totalLength;
    }

    // Ʈ������ ���� ������ ���� ��Ƽ������ ������Ʈ�ϴ� �Լ�
    void UpdateMaterial(float currentRatio)
    {
        float cumulativeRatio = 0;

        for (int i = 0; i < materialRatios.Length; i++)
        {
            cumulativeRatio += materialRatios[i];
            if (currentRatio <= cumulativeRatio)
            {
                trailRenderer.material = materials[i];
                break;
            }
        }
    }
}
