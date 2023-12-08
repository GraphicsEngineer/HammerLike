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
        public GameObject worldObject; // ���� ���� ������Ʈ
        public Image mapIcon; // �̴ϸʿ� ǥ�õ� ������
    }

    public List<MapObject> mapObjects; // ǥ���� ������Ʈ ����Ʈ

    public void Update()
    {
        foreach (MapObject mo in mapObjects)
        {
            Vector3 worldPosition = mo.worldObject.transform.position;
            Vector3 mapPosition = (worldPosition - player.position) * mapScale;

            // �̴ϸ� ��� ���� �ִ��� Ȯ��
            if (Mathf.Abs(mapPosition.x) <= miniMapRect.sizeDelta.x / 2 && Mathf.Abs(mapPosition.z) <= miniMapRect.sizeDelta.y / 2)
            {
                // �̴ϸ� ��� ���� ������ ������ ǥ��
                mo.mapIcon.rectTransform.anchoredPosition = new Vector2(mapPosition.x, mapPosition.z);
                mo.mapIcon.enabled = true; // ������ Ȱ��ȭ
            }
            else
            {
                // �̴ϸ� ��踦 ����� ������ ��Ȱ��ȭ
                mo.mapIcon.enabled = false;
            }
        }
    }



}
