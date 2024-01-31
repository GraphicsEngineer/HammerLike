using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public GameObject invenUI; // �κ��丮 UI ������Ʈ
    public List<Item> items = new List<Item>(16); // �κ��丮 ������ ����Ʈ
    public List<ItemSlot> itemSlots; // ������ ���� ����Ʈ
    public TMP_Text goldText; // ��� ������ ǥ���� Text Mesh Pro UI ������Ʈ
    public int goldItemId; // ��� �������� ID

    private int selectedSlotIndex = -1; // ���� ���õ� ���� �ε���
    private Item selectedItem; // ���� ���õ� ������

    [Header("Quick Slot UI")]
    public Image[] quickSlotUIImages;
    public TMP_Text[] quickSlotQuantityTexts;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        // �ʿ��� �ʱ�ȭ �ڵ� �߰�
    }

    void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            items.Add(null);
        }
        UpdateQuickSlotUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
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

        if (invenUI.activeSelf)
        {
            HandleInput();
        }

        // ������ ��� Ű �Է� ����
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UseItem(0); // 0�� ���� ������ ���
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UseItem(1); // 1�� ���� ������ ���
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UseItem(2); // 2�� ���� ������ ���
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            UseItem(3); // 3�� ���� ������ ���
        }
    }


    private void UseItem(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < items.Count)
        {
            Item itemToUse = items[slotIndex];
            if (itemToUse != null && itemToUse.itemID == 1) // Item ID�� 1�� ���
            {
                Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
                if (player != null)
                {
                    player.RecoverHp(30); // HP 30 ȸ��
                    RemoveItem(slotIndex); // ������ ��� �� ����
                }
            }
        }
    }

    // ������ ���� �޼���
    private void RemoveItem(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < items.Count && items[slotIndex] != null)
        {
            // ������ ���� ����
            items[slotIndex].quantity--;

            // ������ 0���ϰ� �Ǹ� ������ null�� ����
            if (items[slotIndex].quantity <= 0)
            {
                items[slotIndex] = null;
            }

            UpdateInventoryUI();
            UpdateQuickSlotUI();
        }
    }

    public void AddItem(int itemId)
    {
        Item itemToAdd = ItemDB.Instance.GetItemByID(itemId);
        if (itemToAdd == null)
        {
            Debug.LogError("Item not found in ItemDB!");
            return;
        }

        // Check for existing or empty QuickSlot (0-3) for Used items
        if (itemToAdd.itemType == Item.ItemType.Used)
        {
            for (int i = 0; i < 4; i++)
            {
                if (items[i] != null && items[i].itemID == itemToAdd.itemID && items[i].quantity < itemToAdd.limitNumber)
                {
                    items[i].quantity++;
                    UpdateInventoryUI();
                    UpdateQuickSlotUI();
                    return;
                }
                else if (items[i] == null)
                {
                    items[i] = new Item(itemToAdd.itemName, itemToAdd.itemID, itemToAdd.itemType, itemToAdd.itemImage, itemToAdd.limitNumber, 1);
                    UpdateInventoryUI();
                    UpdateQuickSlotUI();
                    return;
                }
            }
        }

        // Check for stackable items or empty slot from slot 4
        for (int i = 4; i < items.Count; i++)
        {
            if (items[i] != null && items[i].itemID == itemToAdd.itemID && items[i].quantity < itemToAdd.limitNumber)
            {
                items[i].quantity++;
                UpdateInventoryUI();
                return;
            }
            else if (items[i] == null)
            {
                items[i] = new Item(itemToAdd.itemName, itemToAdd.itemID, itemToAdd.itemType, itemToAdd.itemImage, itemToAdd.limitNumber, 1);
                UpdateInventoryUI();
                return;
            }
        }

        Debug.Log("Inventory is full or no appropriate slot found!");
    }

    private void UpdateQuickSlotUI()
    {
        for (int i = 0; i < 4; i++) // ù 4���� ���Ը� Ȯ��
        {
            if (items[i] != null)
            {
                quickSlotUIImages[i].sprite = items[i].itemImage;
                quickSlotUIImages[i].color = new Color(1f, 1f, 1f, 1f); // Ǯ ���� ������ ���� (������)
                quickSlotUIImages[i].enabled = true;

                if (items[i].quantity > 1) // ������ 1���� Ŭ ��쿡�� ���� ǥ��
                {
                    quickSlotQuantityTexts[i].text = items[i].quantity.ToString();
                    quickSlotQuantityTexts[i].enabled = true;
                }
                else
                {
                    quickSlotQuantityTexts[i].enabled = false;
                }
            }
            else
            {
                quickSlotUIImages[i].color = new Color(1f, 1f, 1f, 0f); // ���� ���� 0���� ���� (����)
                quickSlotUIImages[i].enabled = false;
                quickSlotQuantityTexts[i].enabled = false;
            }
        }
    }


    public void UpdateInventoryUI()
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (itemSlots[i] != null)
            {
                if (i < items.Count && items[i] != null)
                {
                    itemSlots[i].SetItem(items[i]);
                }
                else
                {
                    itemSlots[i].SetItem(null);
                }
            }
        }

        UpdateGoldUI();
    }

    public void UpdateGoldUI()
    {
        int totalGold = GetTotalGoldAmount(goldItemId);
        goldText.text = totalGold.ToString() + "G";
    }

    private int GetTotalGoldAmount(int goldItemId)
    {
        int totalGold = 0;
        foreach (Item item in items)
        {
            if (item != null && item.itemID == goldItemId)
            {
                totalGold += item.quantity;
            }
        }
        return totalGold;
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
