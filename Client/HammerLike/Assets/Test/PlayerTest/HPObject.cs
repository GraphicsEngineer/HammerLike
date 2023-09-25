using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HPObject : MonoBehaviour
{
    // ������Ʈ ������ �ִ� HP��
    protected float defaultMaxHp;

    // ������, ���� � ���ؼ� �����Ǵ� �ִ� HP��
    protected abstract float extraHp
    {
        get;
    }

    // �����۰� ����ȿ���� ������ �ִ� HP��
    protected float maxHp
    {
        get { return defaultMaxHp + extraHp; }
    }

    // ���� hp
    protected float hp;

    /// <summary>
    /// HPObejct�� ���ݹ���
    /// </summary>
    /// <param name="direction"> ������ ������ ���� ����([�ڽ��� ��ġ] - [������ ��ġ]) </param>
    /// <param name="damage"> ������ </param>
    /// <param name="force"> �˹� ���� </param>
    /// <param name="damageType"> ������ Ÿ�� </param>
    /// <param name="specialDamage"> Ư���� ������ ������(�����̻��� ��� ���ӽð��� ������ �� �� ����) </param>
    public abstract void OnDamaged(Vector3 direction, float damage, float force, DamageType damageType, float specialDamage);
}
