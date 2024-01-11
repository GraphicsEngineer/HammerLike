using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro ���ӽ����̽�
using System.IO; // ���� ������� ���� ���ӽ����̽�

public class SaveData : MonoBehaviour
{
    public TMP_InputField inputField; // ����� �Է��� ���� TMP_InputField
    public TextMeshProUGUI buttonText; // ��ư�� TextMeshPro �ؽ�Ʈ

    private string csvFilePath = "Assets/SavedData.csv"; // CSV ���� ���

    public void SaveGameSettings()
    {
        // ����ڰ� �Է��� �ؽ�Ʈ�� ��ư�� �ؽ�Ʈ�� �����մϴ�.
        string userInput = inputField.text;
        buttonText.text = string.IsNullOrEmpty(userInput) ? "���̺� ����1" : userInput;

        // ����� Ű �̸��� PlayerPrefs���� �����ɴϴ�.
        string horizontalAltNegativeButton = PlayerPrefs.GetString("HorizontalAltNegativeButton", "default_value");
        string horizontalAltPositiveButton = PlayerPrefs.GetString("HorizontalAltPositiveButton", "default_value");
        string verticalAltNegativeButton = PlayerPrefs.GetString("VerticalAltNegativeButton", "default_value");
        string verticalAltPositiveButton = PlayerPrefs.GetString("VerticalAltPositiveButton", "default_value");

        // CSV ���Ϸ� �����մϴ�.
        SaveToCSV(userInput, horizontalAltNegativeButton, horizontalAltPositiveButton, verticalAltNegativeButton, verticalAltPositiveButton);
    }


    private void SaveToCSV(string buttonName, string horizontalNeg, string horizontalPos, string verticalNeg, string verticalPos)
    {
        string csvContent = $"{buttonName},{horizontalNeg},{horizontalPos},{verticalNeg},{verticalPos}\n";

        // ������ �������� ������ ���� ����, �����ϸ� ���� ���뿡 �߰�
        if (!File.Exists(csvFilePath))
        {
            File.WriteAllText(csvFilePath, "Button Name,Horizontal Alt Negative Button,Horizontal Alt Positive Button,Vertical Alt Negative Button,Vertical Alt Positive Button\n");
        }

        File.AppendAllText(csvFilePath, csvContent);
    }
}
