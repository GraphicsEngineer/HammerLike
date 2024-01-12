using UnityEngine;
using TMPro;

public class OneLetterInputField : MonoBehaviour
{
    public TMP_InputField inputField;

    void Start()
    {
        if (inputField != null)
        {
            inputField.onValueChanged.AddListener(HandleInputChanged);
        }
    }

    private void HandleInputChanged(string input)
    {
        // �Ϲ� ���� �Է¿� ���� ó��: ������ ���ڸ� ����
        if (input.Length > 1 && !IsSpecialKeyInput(input))
        {
            inputField.text = input[input.Length - 1].ToString();
        }
    }

    void Update()
    {
        CheckForSpecialKeyInput();
    }

    private void CheckForSpecialKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.CapsLock)) inputField.text = "CapsLock";
        else if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) inputField.text = "Shift";
        else if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)) inputField.text = "Ctrl";
        else if (Input.GetKeyDown(KeyCode.Tab)) inputField.text = "Tab";
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) inputField.text = "Left";
        else if (Input.GetKeyDown(KeyCode.RightArrow)) inputField.text = "Right";
        else if (Input.GetKeyDown(KeyCode.UpArrow)) inputField.text = "Up";
        else if (Input.GetKeyDown(KeyCode.DownArrow)) inputField.text = "Down";
    }

    private bool IsSpecialKeyInput(string input)
    {
        // Ư�� Ű �̸� ���
        string[] specialKeys = { "CapsLock", "Shift", "Ctrl", "Tab", "Left", "Right", "Up", "Down" };
        foreach (string key in specialKeys)
        {
            if (input == key) return true;
        }
        return false;
    }
}
