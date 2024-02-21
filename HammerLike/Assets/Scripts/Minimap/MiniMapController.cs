using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MiniMapController : MonoBehaviour
{
    public Transform player; // �÷��̾��� Transform
    public RectTransform miniMapRect; // �̴ϸ��� RectTransform
    public float mapScale = 5f; // �̴ϸ��� ������

    [System.Serializable]
    public class MapObject
    {
        public string objectTag; // ���� ������Ʈ�� �±�
        public Image mapIconPrefab; // �̴ϸ� ������ ������
    }

    public List<MapObject> mapObjects; // ǥ���� ������Ʈ ����Ʈ
    private Dictionary<string, List<Image>> mapIcons = new Dictionary<string, List<Image>>(); // ������ �����ܵ��� ����

    void Start()
    {
        // �� �±׿� ���� ������ ����Ʈ �ʱ�ȭ
        foreach (var mapObject in mapObjects)
        {
            mapIcons[mapObject.objectTag] = new List<Image>();
        }
    }

    public void Update()
    {
        foreach (MapObject mo in mapObjects)
        {
            GameObject[] worldObjects = GameObject.FindGameObjectsWithTag(mo.objectTag);

            // �ʿ��� ��ŭ ������ ���� �Ǵ� ����
            while (mapIcons[mo.objectTag].Count < worldObjects.Length)
            {
                Image newIcon = Instantiate(mo.mapIconPrefab, miniMapRect);
                mapIcons[mo.objectTag].Add(newIcon);
            }

            // �� ������Ʈ�� ���� ������ ó��
            for (int i = 0; i < worldObjects.Length; i++)
            {
                GameObject worldObject = worldObjects[i];
                Image mapIcon = mapIcons[mo.objectTag][i];

                // Monster ������Ʈ�� �ְ�, curHp�� 0���� ū ��쿡�� �������� ǥ��
                Monster monster = worldObject.GetComponent<Monster>();
                if (monster != null && monster.stat.curHp > 0)
                {
                    Vector3 worldPosition = worldObject.transform.position;
                    Vector3 mapPosition = (worldPosition - player.position) * mapScale;

                    // �̴ϸ� ��� ���� �ִ��� Ȯ��
                    if (Mathf.Abs(mapPosition.x) <= miniMapRect.sizeDelta.x / 2 && Mathf.Abs(mapPosition.z) <= miniMapRect.sizeDelta.y / 2)
                    {
                        // �̴ϸ� ��� ���� ������ ������ ǥ��
                        mapIcon.rectTransform.anchoredPosition = new Vector2(mapPosition.x, mapPosition.z);
                        mapIcon.enabled = true;
                    }
                    else
                    {
                        // �̴ϸ� ��踦 ����� ������ ��Ȱ��ȭ
                        mapIcon.enabled = false;
                    }
                }
                else
                {
                    // Monster ������Ʈ�� ���ų� curHp�� 0 �����̸� �������� ��Ȱ��ȭ
                    mapIcon.enabled = false;
                }
            }
        }
    }

}