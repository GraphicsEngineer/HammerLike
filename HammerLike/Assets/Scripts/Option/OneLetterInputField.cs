using UnityEngine;
using TMPro;

public class OneLetterInputField : MonoBehaviour
{
    public TMP_InputField inputField; // TMP_InputField ������Ʈ�� �Ҵ�

    void Start()
    {
        if (inputField != null)
        {
            // onValueChanged �̺�Ʈ�� �޼ҵ� �Ҵ�
            inputField.onValueChanged.AddListener(HandleInputChanged);
        }
    }

    private void HandleInputChanged(string input)
    {
        if (input.Length > 1)
        {
            // �ؽ�Ʈ�� �� ���ڸ� �ʰ��ϸ� ������ ���ڸ� ����
            inputField.text = input[input.Length - 1].ToString();
        }
    }
}
