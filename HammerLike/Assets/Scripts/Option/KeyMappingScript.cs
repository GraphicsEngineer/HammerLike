using UnityEngine;
using Rewired;
using UnityEngine.UI;
using System;


public class KeyMappingScript : MonoBehaviour
{
    public InputField inputField; // �Է� �ʵ� ����
    private Rewired.Player player; // Rewired �÷��̾�

    void Start()
    {
        player = ReInput.players.GetPlayer(0); // �÷��̾� ����
    }

    public void ApplyNewKey()
    {
        string newKey = inputField.text; // �Է� �ʵ忡�� �� Ű�� �����ɴϴ�.

        // Ű ���� ������Ʈ
        foreach (var map in player.controllers.maps.GetAllMaps(ControllerType.Keyboard))
        {
            /*if (map.elementMaps.Find(x => x.actionName == "Horizontal") != null)
            {
                // 'Horizontal' �׼��� ���� �� Ű ����
                map.elementMaps.Find(x => x.actionName == "Horizontal").elementIdentifierName = newKey;
            }*/
            
        }

        // ���� ���� ����
        //player.controllers.maps.LoadAllMaps();
        //player.controllers.maps.();
    }
}
