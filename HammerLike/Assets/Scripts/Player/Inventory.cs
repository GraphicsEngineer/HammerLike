using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject invenUI; // SetActive�� ������ų ��� ������Ʈ
    public List<Item> items = new List<Item>(16); // �κ��丮 �����۵�
    public List<ItemSlot> itemSlots;
    void Start()
    {
        // �� ���������� �ʱ�ȭ�ϰų� �̸� ���ǵ� ���������� ä��
        for (int i = 0; i < 16; i++)
        {
            items.Add(null);
        }
    }


    // Update is called once per frame
    void Update()
    {
        // ESC Ű�� ���ȴ��� Ȯ��
        if (Input.GetKeyDown(KeyCode.I))
        {
            // targetObject�� Ȱ�� ���¸� ����
            invenUI.SetActive(!invenUI.activeSelf);
            if (invenUI.activeSelf)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }

    public void AddItem(Item item)
    {
        bool isAdded = false;
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                UpdateInventoryUI(); // �κ��丮 UI ������Ʈ
                isAdded = true;
                Debug.Log("�������� �κ��丮 ���� " + i + "�� �߰��Ǿ����ϴ�.");
                break;
            }
        }

        if (!isAdded)
        {
            Debug.Log("�κ��丮�� ���� á���ϴ�.");
        }
    }


        public bool AddItem(int itemId)
    {
        // ItemDB���� ������ ã��
        Item itemToAdd = ItemDB.Instance.GetItemByID(itemId);

        if (itemToAdd == null)
        {
            Debug.LogError("Item not found in ItemDB!");
            return false;
        }

        // �κ��丮�� �� ���� ã�� ������ �߰�
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null)
            {
                items[i] = itemToAdd;
                // UI ������Ʈ�� ��Ÿ ó��
                UpdateInventoryUI();
                return true;
            }
        }

        // �κ��丮�� ������ ���� ���
        Debug.Log("Inventory is full!");
        return false;
    }

    public void UpdateInventoryUI()
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            Sprite sprite = (i < items.Count && items[i] != null) ? items[i].itemImage : null;
            itemSlots[i].SetItemSprite(sprite);
            Debug.Log("���� " + i + " ������Ʈ: " + (sprite != null ? sprite.name : "�� ����"));
        }
    }
}
