using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MiniMapGenerator : MonoBehaviour
{
    public RectTransform miniMapArea; // �̴ϸ��� ǥ���� ����
    public GameObject mapIconPrefab; // �̴ϸʿ� ǥ�õ� ������ ������

    public Vector2 worldMapSize = new Vector2(100, 100); // ���� ���� ũ��
    public Vector3 worldMapCenter = new Vector3(50, 0, 50); // ���� ���� �߽�

    void Start()
    {
        List<RoomData> rooms = new List<RoomData>();
        // ���⼭ �� ���� �����͸� �߰��մϴ�.
        // ��: rooms.Add(new RoomData{ Position = new Vector3(10, 0, 10), Size = new Vector3(5, 0, 5), Rotation = 0 });

        GenerateMiniMap(rooms);
    }

    public void GenerateMiniMap(List<RoomData> rooms)
    {
        foreach (RoomData room in rooms)
        {
            // ���� �� �κп� ���� UI ��� ����
            GameObject mapIcon = Instantiate(mapIconPrefab, miniMapArea);
            RectTransform rectTransform = mapIcon.GetComponent<RectTransform>();

            // ��ġ, ũ��, ȸ�� ����
            rectTransform.anchoredPosition = CalculateMiniMapPosition(room.Position);
            rectTransform.sizeDelta = CalculateMiniMapSize(room.Size);
            rectTransform.localRotation = Quaternion.Euler(0, 0, room.Rotation);

            // �ʿ��� ��� �߰� ����
            mapIcon.SetActive(true);
        }
    }

    Vector2 CalculateMiniMapPosition(Vector3 worldPosition)
    {
        // �̴ϸ� �������� ��ġ ����
        // ���⼭ scale�� ���� ��ǥ�� �̴ϸ� ��ǥ ���� ������ �����մϴ�.
        float scale = miniMapArea.rect.width / (float)worldMapSize.x;
        float xPosition = (worldPosition.x - worldMapCenter.x) * scale;
        float yPosition = (worldPosition.z - worldMapCenter.z) * scale;

        return new Vector2(xPosition, yPosition);
    }

    Vector2 CalculateMiniMapSize(Vector3 worldSize)
    {
        // �̴ϸ� �������� ũ�� ����
        float scale = miniMapArea.rect.width / (float)worldMapSize.x;
        float width = worldSize.x * scale;
        float height = worldSize.z * scale;

        return new Vector2(width, height);
    }

}

public class RoomData
{
    public Vector3 Position; // ���� ��ġ
    public Vector3 Size; // ���� ũ��
    public float Rotation; // ���� ȸ��
}
