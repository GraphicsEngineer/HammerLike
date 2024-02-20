using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 5f; // ����ü�� �����ֱ�(��)
    private Monster shooter; // ����ü�� �߻��� ����

    void Start()
    {
        // ���� �ð� �Ŀ� �ڵ� �ı�
        Destroy(gameObject, lifetime);
    }

    public void SetShooter(Monster monster)
    {
        // ����ü�� �߻��� ���� ����
        shooter = monster;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // �θ� ������Ʈ�� �ִ� Player ������Ʈ�� ����
            Player playerComponent = other.GetComponentInParent<Player>();

            if (playerComponent != null&&!playerComponent.isEvading)
            {
                // �÷��̾�� �������� �ִ� ����
                playerComponent.TakeDamage(shooter.stat.attackPoint);
                 
                    SoundManager soundManager = SoundManager.Instance;
                    soundManager.PlaySFX(soundManager.audioClip[8]);
                
                // ����ü �ı�
                DestroyProjectile();
            }

        }
    }


    private void DestroyProjectile()
    {
        // ����ü�� �ı��ϱ� ���� ���Ϳ��� �˸�
        if (shooter != null)
        {
            shooter.ProjectileDestroyed();
        }

        // ����ü �ı�
        Destroy(gameObject);
    }
}
