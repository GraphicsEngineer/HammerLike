using UnityEngine;

public class TestUI : MonoBehaviour
{
    public GameObject targetObject; // SetActive�� ������ų ��� ������Ʈ

    // Update is called once per frame
    void Update()
    {
        // ESC Ű�� ���ȴ��� Ȯ��
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // targetObject�� Ȱ�� ���¸� ����
            targetObject.SetActive(!targetObject.activeSelf);
        }
    }
}
