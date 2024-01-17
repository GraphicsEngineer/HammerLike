using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler, IDropHandler
{

    public Image itemImage; // ������ ������ �̹���
    private Item currentItem; // ���� ������ ������
    private Inventory inventory; // �θ� �κ��丮 ����
    private Vector2 originalPosition; // �巡�� ���� ��ġ

    public void SetItemSprite(Sprite sprite)
    {
        itemImage.sprite = sprite;
        itemImage.enabled = (sprite != null);
    }
    // Start is called before the first frame update
    void Start()
    {
        inventory = GetComponentInParent<Inventory>();
        originalPosition = itemImage.rectTransform.anchoredPosition;
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
        itemImage.rectTransform.anchoredPosition = originalPosition;
    }

    public void OnDrop(PointerEventData eventData)
    {
        // �ٸ� ���Կ� ������� ���� ó��
        ItemSlot droppedSlot = eventData.pointerDrag.GetComponent<ItemSlot>();
        if (droppedSlot != null && droppedSlot != this)
        {
            // ������ ��ȯ ó��
            Item tempItem = currentItem;
            SetItem(droppedSlot.currentItem);
            droppedSlot.SetItem(tempItem);
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
