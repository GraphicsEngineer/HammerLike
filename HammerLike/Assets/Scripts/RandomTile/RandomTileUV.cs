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

    private void Awake()
    {
        Renderer rend = GetComponent<Renderer>();
        rend.material = new Material(rend.sharedMaterial);

        // ���� ������Ʈ�� ���͸����� materialsData�� �ִ��� Ȯ��
        // rend.material�� ����Ͽ� ��ġ�ϴ� ������ ã���ϴ�.
        RandomTileMaterialData selectedData = materialsData.FirstOrDefault(md => md.material == rend.material);

        if (selectedData == null)
        {
            Debug.LogWarning("���� �����Ͱ� �������� �ʾҽ��ϴ�.");
            return;
        }

        // ���� �õ� �ʱ�ȭ
        Random.InitState(System.DateTime.Now.Millisecond + this.GetInstanceID());

        MeshFilter mf = GetComponent<MeshFilter>();
        if (mf == null)
        {
            Debug.LogWarning("MeshFilter�� �ʿ��մϴ�.");
            return;
        }

        Vector2[] uvs = mf.mesh.uv;

        for (int i = 0; i < uvs.Length; i += 4) // 4���� ������ �ϳ��� Ÿ���� ��Ÿ���ϴ�.
        {
            int randomX = Random.Range(0, selectedData.tilesX);
            int randomY = Random.Range(0, selectedData.tilesY);

            Vector2 scale = new Vector2(1.0f / selectedData.tilesX, 1.0f / selectedData.tilesY);
            Vector2 offset = new Vector2(randomX * scale.x, randomY * scale.y);

            uvs[i] = new Vector2(0 + offset.x, 0 + offset.y);
            uvs[i + 1] = new Vector2(scale.x + offset.x, 0 + offset.y);
            uvs[i + 2] = new Vector2(scale.x + offset.x, scale.y + offset.y);
            uvs[i + 3] = new Vector2(0 + offset.x, scale.y + offset.y);
        }

        mf.mesh.uv = uvs;
    }
}
