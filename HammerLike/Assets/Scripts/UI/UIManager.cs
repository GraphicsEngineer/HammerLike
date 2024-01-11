using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject objectToActivate; // Ȱ��ȭ�� ������Ʈ

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // ���� ������Ʈ ��Ȱ��ȭ
            gameObject.SetActive(false);

            // ������ ������Ʈ Ȱ��ȭ
            if (objectToActivate != null)
            {
                objectToActivate.SetActive(true);
            }
        }
    }


}
