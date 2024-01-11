using UnityEngine;
using UnityEngine.UI;
using Rewired;
using TMPro; // TextMesh Pro ���ӽ����̽� �߰�
using Rewired.Data;

public class KeyRebindScript : MonoBehaviour
{
    public TMP_InputField inputField; // TMP �Է� �ʵ�
    public Button changeKeyButton; // Ű ���� ��ư
    private Rewired.Player player; // Rewired �÷��̾�
    private UserDataStore_PlayerPrefs userDataStore; // ����� ������ �����

    void Start()
    {
        player = ReInput.players.GetPlayer(0); // ù ��° �÷��̾� ��������
        userDataStore = ReInput.userDataStore as UserDataStore_PlayerPrefs; // UserDataStore_PlayerPrefs �ν��Ͻ� ��������

        // ��ư Ŭ�� �̺�Ʈ�� ChangeKey �޼��� �߰�
        changeKeyButton.onClick.AddListener(() => ChangeKey(inputField.text));
    }

    public void ChangeKey(string newKey)
    {
        KeyCode keyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), newKey, true); // ���ڿ��� KeyCode�� ��ȯ

        int categoryId = 1; // ���� ���, ī�װ� ID�� 1���̶�� ����
        foreach (ControllerMap map in player.controllers.maps.GetAllMaps(ControllerType.Keyboard))
        {
            if (map.categoryId != categoryId)
            {
                Debug.Log("�߸��� ī�װ�");
                continue; // �ùٸ� ī�װ����� Ȯ��
            }

                foreach (ActionElementMap element in map.AllMaps)
            {
                if (element.actionDescriptiveName == "Horizontal" && element.axisContribution == Pole.Positive) // Horizontal�� Positive Key�� ã��
                {
                    var elementIdentifier = ReInput.controllers.Keyboard.GetElementIdentifierByKeyCode(keyCode);
                    if (elementIdentifier != null)
                    {
                        ElementAssignment assignment = new ElementAssignment(
                            ControllerType.Keyboard,
                            ControllerElementType.Button,
                            elementIdentifier.id,
                            AxisRange.Positive,
                            keyCode,
                            ModifierKeyFlags.None,
                            element.actionId,
                            Pole.Positive,
                            false,
                            element.id
                        );

                        // Ű ����
                        map.ReplaceElementMap(assignment);
                    }
                    break;
                }
            }
        }

        // ���� ���� ����
        if (userDataStore != null)
        {
            userDataStore.Save();
        }
        else
        {
            Debug.LogError("UserDataStore_PlayerPrefs not found!");
        }
    }
}
