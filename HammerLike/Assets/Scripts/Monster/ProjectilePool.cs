using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool Instance;

    public GameObject projectilePrefab; // ����ü ������
    private Queue<GameObject> projectiles = new Queue<GameObject>(); // ����ü Ǯ

    private void Awake()
    {
        // �̱��� �������� �ν��Ͻ��� �ϳ��� �����ϵ��� ����
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Ǯ���� ����ü�� �������� �޼ҵ�
    public GameObject GetProjectile()
    {
        if (projectiles.Count == 0)
        {
            AddProjectiles(1); // ����� ����ü�� ������ ���� ����
        }

        return projectiles.Dequeue(); // ť���� ����ü�� �����ϰ� ��ȯ
    }

    // ����ü�� Ǯ�� ��ȯ�ϴ� �޼ҵ�
    public void ReturnProjectile(GameObject projectile)
    {
        projectile.SetActive(false); // ����ü�� ��Ȱ��ȭ
        projectiles.Enqueue(projectile); // Ǯ�� �ٽ� �߰�
    }

    // Ǯ�� ����ü�� �߰��ϴ� �޼ҵ�
    private void AddProjectiles(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var newProjectile = Instantiate(projectilePrefab); // �� ����ü ����
            newProjectile.SetActive(false); // ó������ ��Ȱ��ȭ
            projectiles.Enqueue(newProjectile); // Ǯ�� �߰�
            newProjectile.transform.SetParent(transform); // ������: ���� �������� �� ��ü �Ʒ��� ����ü�� ����
        }
    }
}
