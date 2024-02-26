using System.Collections;
using UnityEngine;
using UnityEngine.UI; // UI ������Ʈ ����� ���� �߰�

public class UpdateCameraShake : MonoBehaviour
{
    public RectTransform panel; // ��鸱 �г��� RectTransform
    public float shakeIntensity = 0.5f; // ��鸲�� ����
    public float shakeTime = 0.5f; // ��鸲 ���� �ð�

    private Vector3 originalPosition; // �г��� ���� ��ġ
    private float timer;

    void Start()
    {
        if (panel != null)
            originalPosition = panel.localPosition; // ���� �� �г��� ���� ��ġ ����
    }

    public void ShakePanel()
    {
        if (timer <= 0)
        {
            StartCoroutine(DoShake());
        }
    }

    IEnumerator DoShake()
    {
        timer = shakeTime;
        while (timer > 0)
        {
            panel.localPosition = originalPosition + (Vector3)Random.insideUnitCircle * shakeIntensity;
            timer -= Time.deltaTime;
            yield return null;
        }
        panel.localPosition = originalPosition; // ��鸲�� ������ �г��� ���� ��ġ��
    }

    // UI �����̴����� ȣ���� �� �ִ� �޼���
    public void SetShakeIntensity(float intensity)
    {
        shakeIntensity = intensity;
    }
}
