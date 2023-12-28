using UnityEngine;
using System.Collections.Generic;

public class RandomObjectSpawner : MonoBehaviour
{
    [System.Serializable]
    public class PrefabProbability
    {
        public GameObject prefab;
        public float probability; // �� ������ ��ü �հ谡 1�� �Ǿ�� ��
    }

    public List<PrefabProbability> prefabsWithProbability;
    public bool destroyOriginal = false;
    public bool useParentScale = true;
    public bool useParentRotation = true;
    public bool useParentPosition = true;

    private void Start()
    {
        ReplaceObject();
    }

    public void ReplaceObject()
    {
        GameObject selectedPrefab = SelectRandomPrefab();

        Quaternion rotation = useParentRotation ? transform.rotation : selectedPrefab.transform.rotation;
        Vector3 scale = useParentScale ? transform.localScale : selectedPrefab.transform.localScale;

        Vector3 position;
        if (useParentPosition)
        {
            position = transform.position;
        }
        else
        {
            position = transform.position + (selectedPrefab.transform.position);
        }

        // �� ������Ʈ�� �����ϰ� �θ� ������Ʈ�� ����
        GameObject spawnedObject = Instantiate(selectedPrefab, position, rotation, transform.parent);
        spawnedObject.transform.localScale = scale;

        // ���� ������Ʈ ó��
        if (destroyOriginal)
        {
            Destroy(gameObject);
        }
        else
        {
            // ���� ������Ʈ�� ��Ȱ��ȭ���� �ʰ� �״�� ��
        }
    }


    private GameObject SelectRandomPrefab()
    {
        float total = 0;
        foreach (var item in prefabsWithProbability)
        {
            total += item.probability; // ��� Ȯ���� ���� ���
        }

        float randomPoint = Random.value * total; // 0�� total ������ ������ ���� ����

        foreach (var item in prefabsWithProbability)
        {
            if (randomPoint < item.probability)
                return item.prefab;
            else
                randomPoint -= item.probability;
        }

        return null; // �� ���� �߻����� �ʾƾ� ��
    }

}
