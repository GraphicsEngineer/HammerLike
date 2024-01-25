using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject invenUI; // SetActive�� ������ų ��� ������Ʈ
    public List<Item> items = new List<Item>(16); // �κ��丮 �����۵�
    public List<ItemSlot> itemSlots;
    private int selectedSlotIndex = -1; // ���� ���õ� ���� �ε���
    private Item selectedItem; // ���� ���õ� ������

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

        if(invenUI.activeSelf)
        {
            HandleInput();
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
                //Debug.Log("�������� �κ��丮 ���� " + i + "�� �߰��Ǿ����ϴ�.");
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
            //Debug.Log("���� " + i + " ������Ʈ: " + (sprite != null ? sprite.name : "�� ����"));
        }
    }

    public void SwapItems(int index1, int index2)
    {
        if (index1 < 0 || index1 >= items.Count || index2 < 0 || index2 >= items.Count)
            return;

        Item temp = items[index1];
        items[index1] = items[index2];
        items[index2] = temp;
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveSelection(4); // �Ʒ��� �̵�
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveSelection(-4); // ���� �̵�
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveSelection(-1); // �������� �̵�
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveSelection(1); // ���������� �̵�
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            ConfirmSelection(); // ���� Ȯ��
        }
        else if (Input.GetKeyDown(KeyCode.Home))
        {
            SwapSlots(0, 1);
        }
    }

    private void MoveSelection(int offset)
    {
        if (selectedSlotIndex == -1) return;

        int newSlotIndex = Mathf.Clamp(selectedSlotIndex + offset, 0, items.Count - 1);
        if (newSlotIndex != selectedSlotIndex)
        {
            // ������ ������ ��ȯ
            Item tempItem = items[newSlotIndex];
            items[newSlotIndex] = items[selectedSlotIndex];
            items[selectedSlotIndex] = tempItem;

            // ���� UI ������Ʈ
            itemSlots[newSlotIndex].SetItem(items[newSlotIndex]);
            itemSlots[selectedSlotIndex].SetItem(items[selectedSlotIndex]);

            UpdateInventoryUI(); // UI ������Ʈ
            selectedSlotIndex = newSlotIndex; // ���õ� ���� ������Ʈ
        }
    }


    private void SwapFirstTwoSlots()
    {
        if (items.Count >= 2)
        {
            // ù ��°�� �� ��° ������ ��ȯ
            Item temp = items[0];
            items[0] = items[1];
            items[1] = temp;

            // ���� UI ������Ʈ
            itemSlots[0].SetItem(items[0]);
            itemSlots[1].SetItem(items[1]);

            UpdateInventoryUI(); // UI ������Ʈ
        }
    }

    private void SwapSlots(int index1, int index2)
    {
        // ��ȿ�� �ε��� Ȯ��
        if (index1 >= 0 && index1 < items.Count && index2 >= 0 && index2 < items.Count)
        {
            // ������ ��ȯ
            Item temp = items[index1];
            items[index1] = items[index2];
            items[index2] = temp;

            // ���� UI ������Ʈ
            itemSlots[index1].SetItem(items[index1]);
            itemSlots[index2].SetItem(items[index2]);

            UpdateInventoryUI(); // UI ������Ʈ
        }
    }

    private void ConfirmSelection()
    {
        // ���� ������ ���Կ� ������ ��ġ
        selectedItem = null;
        selectedSlotIndex = -1;
    }

    // �������� �����ϴ� �޼��� (��: ������ Ŭ�� �� ȣ��)
    public void SelectItem(int slotIndex)
    {
        selectedSlotIndex = slotIndex;
        selectedItem = itemSlots[slotIndex].currentItem;
    }
}
