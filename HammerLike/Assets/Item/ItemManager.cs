using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Player player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Insert))
        {
            GiveItemToPlayer(1);
        }
    }

    void GiveItemToPlayer(int itemId)
    {
        // AddItem �޼ҵ带 �� ���� ȣ��
        bool isAdded = player.inventory.AddItem(itemId);
        if (isAdded)
        {
            Debug.Log("�������� ���������� �߰��Ǿ����ϴ�.");
        }
        else
        {
            Debug.LogError("�������� �߰����� ���߽��ϴ�.");
        }
    }

}
