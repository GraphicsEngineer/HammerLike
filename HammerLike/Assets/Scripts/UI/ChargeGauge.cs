using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeGauge : MonoBehaviour
{
    public PlayerAtk playerAtk; // �÷��̾��� Attack ��ũ��Ʈ
    public Image gaugeImage; // ������ �̹���
    public GameObject particleObject; // ��ƼŬ �̹��� ������Ʈ

    private RectTransform gaugeRectTransform;

    private void Awake()
    {
        gaugeRectTransform = gaugeImage.GetComponent<RectTransform>();
        particleObject.SetActive(false); // �ʱ⿡�� ��ƼŬ �̹��� ����
    }

    private void Update()
    {
        UpdateGauge();
    }

    private void UpdateGauge()
    {
        float chargeRatio = Mathf.Clamp(playerAtk.curCharging / 3.0f, 0f, 1f); // 0 ~ 1 ������ ��
        gaugeRectTransform.anchorMax = new Vector2(gaugeRectTransform.anchorMax.x, chargeRatio);

        // ��ƼŬ �̹��� Ȱ��ȭ/��Ȱ��ȭ
        if (chargeRatio >= 1f)
        {
            particleObject.SetActive(true);
        }
        else
        {
            particleObject.SetActive(false);
        }
    }
}
