using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Idle : cState
{
    public Monster monster;

    public Monster_Idle(Monster _monster)
    {
        monster = _monster;
    }

    public override void EnterState()
    {
        base.EnterState();
        // ���⿡ ���Ͱ� ��� ���·� ���� ���� ������ �߰�
    }

    public override void UpdateState()
    {
        base.UpdateState();
        // ������ ��� ���¿��� ����� ������ ���⿡ �߰�
    }

    // ������ �޼���鵵 �ʿ��ϴٸ� �������̵��Ͽ� ���
}