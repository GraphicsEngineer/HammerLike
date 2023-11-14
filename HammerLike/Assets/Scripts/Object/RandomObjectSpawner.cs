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


    private void Start()
    {
        ReplaceObject();
    }

    // ������ ��ư�� ����� �޼���
    public void ReplaceObject()
    {
        GameObject selectedPrefab = SelectRandomPrefab();
        GameObject spawnedObject = Instantiate(selectedPrefab, transform.position, transform.rotation);

        // ���� ������Ʈ ó��
        if (destroyOriginal)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private GameObject SelectRandomPrefab()
    {
        float total = 0;
        foreach (var item in prefabsWithProbability)
        {
            total += item.probability;
        }

        float randomPoint = Random.value * total;

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
