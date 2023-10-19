using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class RandomTileUV : MonoBehaviour
{
    [Tooltip("���͸��� �ε��� (��κ��� ��� 0)")]
    public int materialIndex = 0;

    [Tooltip("Ÿ�ϸ��� ���� Ÿ�� ��")]
    public int tilesX = 4;

    [Tooltip("Ÿ�ϸ��� ���� Ÿ�� ��")]
    public int tilesY = 4;

    private void Start()
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend.materials.Length > materialIndex)
        {
            Material mat = rend.materials[materialIndex];

            // ������ Ÿ�� ����
            int randomX = Random.Range(0, tilesX);
            int randomY = Random.Range(0, tilesY);

            // UV ������ �� ������ ����
            Vector2 scale = new Vector2(1.0f / tilesX, 1.0f / tilesY);
            Vector2 offset = new Vector2(randomX * scale.x, randomY * scale.y);

            mat.mainTextureScale = scale;
            mat.mainTextureOffset = offset;
        }
        else
        {
            Debug.LogWarning("������ ���͸��� �ε����� ��ȿ���� �ʽ��ϴ�.");
        }
    }
}
