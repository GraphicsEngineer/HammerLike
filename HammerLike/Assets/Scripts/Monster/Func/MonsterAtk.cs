using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAtk : MonoBehaviour
{
    public Monster monster; // Monster ������Ʈ�� ���� ����

    void Awake()
    {
        //monster = GetComponentInParent<Monster>(); // Monster ������Ʈ�� ã�� �Ҵ�
        if (monster == null)
        {
            Debug.LogError("Monster component not found in parent");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (monster == null)
        {
            Debug.Log("monster is null");
            return; // monster�� null�̸� �Լ� ����
        }

        if (other.gameObject.CompareTag("Player") && monster.attackCollider.enabled)
        {
            Player player = monster.Player; // getter�� ����Ͽ� player�� ����
            if (player != null&&!player.isEvading)
            {
                player.TakeDamage(monster.stat.attackPoint);
                if(monster.monsterType==MonsterType.Melee)
                {
                    SoundManager soundManager = SoundManager.Instance;
                    soundManager.PlaySFX(soundManager.audioClip[7]);
                }
               
            }
        }
    }
}

