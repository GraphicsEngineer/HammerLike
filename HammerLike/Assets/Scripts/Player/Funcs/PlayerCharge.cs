using UnityEngine;
using UnityEngine.UI;

public class PlayerCharge : MonoBehaviour
{
    public Slider chargeSlider;
    public Player player; // player ������ public���� �����Ͽ� Inspector���� �Ҵ��� �� �ֵ��� ��

    private void Start()
    {
        // ���� �� �����̴��� ����ϴ�.
        chargeSlider.gameObject.SetActive(false);
    }

    private void Update()
    {
        // player.atk.curCharging ���� 1 �̻��� ���� �����̴��� �����ݴϴ�.
        if (player.atk.curCharging >= 1f)
        {
            // �����̴��� Ȱ��ȭ�Ǿ� ���� �ʴٸ� Ȱ��ȭ�մϴ�.
            if (!chargeSlider.gameObject.activeSelf)
            {
                chargeSlider.gameObject.SetActive(true);
            }

            // �����̴��� ���� curCharging ���� �°� ������Ʈ�մϴ�.
            // ���⼭�� 10���� ������ 0~1 ������ ���� ������ �����մϴ�.
            chargeSlider.value = (player.atk.curCharging - 1) / (10 - 1);
        }
        else if (chargeSlider.gameObject.activeSelf)
        {
            // �����̴��� ��Ȱ��ȭ�մϴ�.
            chargeSlider.gameObject.SetActive(false);
        }
    }
}
