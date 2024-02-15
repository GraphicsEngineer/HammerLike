<<<<<<< HEAD
using UnityEngine;

public class WeaponColliderScript : MonoBehaviour
{
    public PlayerAtk playerAtk; // PlayerAtk ��ũ��Ʈ�� ����

    private void Awake()
    {
        // playerAtk ������Ʈ�� �ڵ����� ã�Ƽ� �Ҵ��� ���� �ֽ��ϴ�.
        // ��: playerAtk = FindObjectOfType<PlayerAtk>();
=======
﻿using UnityEngine;

public class WeaponColliderScript : MonoBehaviour
{
    public PlayerAtk playerAtk; // PlayerAtk 스크립트의 참조

    private void Awake()
    {
        // playerAtk 컴포넌트를 자동으로 찾아서 할당할 수도 있습니다.
        // 하지만 FindObjectOfType은 너무 비효율적이긴 하다
        //playerAtk = FindObjectOfType<PlayerAtk>();
>>>>>>> 490b48c1d07f9272897a1d5bb968027958be33a4
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            Rigidbody enemyRb = other.GetComponent<Rigidbody>();
            if (enemyRb != null)
            {
<<<<<<< HEAD
                // Player�� �ٶ󺸴� �������� ���� ����
=======
                // Player가 바라보는 방향으로 힘을 가함
>>>>>>> 490b48c1d07f9272897a1d5bb968027958be33a4
                Vector3 forceDirection = playerAtk.player.transform.forward;
                enemyRb.AddForce(forceDirection.normalized * playerAtk.forceMagnitude, ForceMode.Impulse);
            }
        }
    }
}
