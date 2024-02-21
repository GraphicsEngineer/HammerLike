using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTrail : MonoBehaviour
{
    private TrailRenderer trailRenderer;
    private int currentAttackId=0;
    private int attackId=0;

    void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
    }

    public void EnableTrail()
    {
        currentAttackId = attackId; // ���� ���� ID ����
        trailRenderer.enabled = true;
        trailRenderer.emitting = true;
        StartCoroutine(DisableTrailAfterDelay(attackId)); // �ڷ�ƾ���� ���� �� ��Ȱ��ȭ
    }

    private IEnumerator DisableTrailAfterDelay(int attackId)
    {
        yield return new WaitForSeconds(0.5f); // 0.5�� ���

        // ���� ���� ID�� ����� ���� ID�� ����, trailRenderer.emitting�� true�� ��쿡�� ��Ȱ��ȭ
        if (this.currentAttackId == attackId && trailRenderer.emitting)
        {
            trailRenderer.emitting = false;
        }
    }

    public void DisableTrail()
    {
        trailRenderer.emitting = false;
        attackId++;
    }
}
