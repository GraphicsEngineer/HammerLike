using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Renderer))]
public class RandomTileUV : MonoBehaviour
{
    [System.Serializable]
    public class RandomTileMaterialData
    {
        public Material material;
        public int tilesX = 4;
        public int tilesY = 4;
    }

    public RandomTileMaterialData[] materialsData;

    private void Start()
    {
        if (materialsData.Length == 0)
        {
            Debug.LogWarning("���� �����Ͱ� �������� �ʾҽ��ϴ�.");
            return;
        }

        RandomTileMaterialData selectedData = materialsData[Random.Range(0, materialsData.Length)];

        Renderer rend = GetComponent<Renderer>();
        if (rend.sharedMaterials.Contains(selectedData.material))
        {
            Material mat = selectedData.material;

            // ������ Ÿ�� ����
            int randomX = Random.Range(0, selectedData.tilesX);
            int randomY = Random.Range(0, selectedData.tilesY);

            // UV ������ �� ������ ����
            Vector2 scale = new Vector2(1.0f / selectedData.tilesX, 1.0f / selectedData.tilesY);
            Vector2 offset = new Vector2(randomX * scale.x, randomY * scale.y);

            mat.mainTextureScale = scale;
            mat.mainTextureOffset = offset;

            rend.material = mat;  // ���õ� ������ �������� ���� ����
        }
        else
        {
            Debug.LogWarning("������ ���͸����� ��ȿ���� �ʽ��ϴ�.");
        }
    }
}