using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{

    public Image itemImage; // ������ ������ �̹���
    public Item currentItem; // ���� ������ ������
    private Inventory inventory; // �θ� �κ��丮 ����
    private Vector2 originalPosition; // �巡�� ���� ��ġ
    private int slotIndex;
    public void SetItemSprite(Sprite sprite)
    {
        itemImage.sprite = sprite;
        itemImage.enabled = (sprite != null);
    }
    // Start is called before the first frame update
    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            inventory = playerObject.GetComponent<Inventory>();
            slotIndex = inventory.itemSlots.IndexOf(this);
            if (inventory == null)
            {
                Debug.LogError("Player ������Ʈ�� Inventory ������Ʈ�� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogError("Player �±׸� ���� ������Ʈ�� ã�� �� �����ϴ�.");
        }

        originalPosition = itemImage.rectTransform.anchoredPosition;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        inventory.SelectItem(slotIndex);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // �巡�� ���� ��, ������ �̹����� ��ġ�� ����
        originalPosition = itemImage.rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // �巡�� �߿��� ������ �̹����� ���콺 Ŀ���� ���� �̵�
        itemImage.rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // �巡�� ���� ��, ������ �̹����� ���� ��ġ�� ����
        //itemImage.rectTransform.anchoredPosition = originalPosition;
    }

    public void OnDrop(PointerEventData eventData)
    {
        ItemSlot draggedSlot = eventData.pointerDrag.GetComponent<ItemSlot>();
        GameObject dropTargetObject = eventData.pointerCurrentRaycast.gameObject;
        ItemSlot dropTargetSlot = dropTargetObject != null ? dropTargetObject.GetComponent<ItemSlot>() : null;

        Debug.Log("�巡���� ����: " + (draggedSlot != null ? draggedSlot.name : "null"));
        Debug.Log("��� ��� ��ü: " + (dropTargetObject != null ? dropTargetObject.name : "null"));
        Debug.Log("��� ��� ����: " + (dropTargetSlot != null ? dropTargetSlot.name : "null"));
        if (draggedSlot != null && dropTargetSlot != null && draggedSlot != dropTargetSlot)
        {
            // �巡���� ���԰� ��ӵ� ������ ���� �ٸ� ���

            // �巡���� ���԰� ��ӵ� ������ �������� ��ȯ
            Item tempItem = dropTargetSlot.currentItem;
            dropTargetSlot.SetItem(draggedSlot.currentItem);
            draggedSlot.SetItem(tempItem);

            // �κ��丮 UI ������Ʈ
            if (inventory != null)
            {
                inventory.UpdateInventoryUI();
            }
            else
            {
                Debug.LogError("Inventory ������ null�Դϴ�.");
            }
        }
        else
        {
            Debug.LogError("�巡���� ������ null�̰ų� ��� ��� ���԰� �����մϴ�.");
        }
    }



    public void SetItem(Item item)
    {
        currentItem = item;
        itemImage.sprite = item != null ? item.itemImage : null;
        itemImage.enabled = (item != null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
