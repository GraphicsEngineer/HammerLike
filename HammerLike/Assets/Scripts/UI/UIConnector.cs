using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIConnector : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        // �ɼ� �г��� Ȱ��ȭ�� �� UI ��Ҹ� �����ϴ� ����
        SoundManager.Instance.ConnectUIElements();
    }
}
