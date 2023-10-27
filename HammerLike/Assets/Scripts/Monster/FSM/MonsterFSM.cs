using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFSM : StateCtrl
{
    Monster monster;

    public override void InitState()
    {
        curState = AddState(new Monster_Idle(monster));
        // �ٸ� ���� ���µ鵵 �߰� ����
        // ��: AddState(new Monster_Attack(monster));
        // AddState(new Monster_Death(monster));
    }

    public override void Release()
    {
        // �ʿ��� ���, ����� ���ҽ��� �Ҵ�� �޸� ���� ���� �߰�
    }

    protected override void Awake()
    {
        base.Awake();
        monster = GetComponent<Monster>();
    }

    // ������ �޼���鵵 �ʿ信 ���� �������̵��Ͽ� ���
}