using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    Monster monster;
    Vector3 destination;
    private MonsterAim monsterAim;
    private void Awake()
    {
        monster = GetComponent<Monster>();
        monsterAim = GetComponent<MonsterAim>();
    }

    public void Move(float moveSpeed)
    {
        if (monsterAim.CurrentTarget)  // MonsterAim Ŭ�������� ���� Ÿ���� ���������� �ϴ� ��� �߰� �ʿ�
        {
            Vector3 direction = (monsterAim.CurrentTarget.position - transform.position).normalized;
            monster.rd.velocity = direction * moveSpeed;
        }
        else
        {
            monster.rd.velocity = Vector3.zero;
        }
    }

    // AI sets the destination
    public void SetDestination(Vector3 newDestination)
    {
        destination = newDestination;
    }
}
