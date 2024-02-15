using UnityEngine;

public class WeaponColliderScript : MonoBehaviour
{
    public PlayerAtk playerAtk; // PlayerAtk ��ũ��Ʈ�� ����

    private void Awake()
    {
        // playerAtk ������Ʈ�� �ڵ����� ã�Ƽ� �Ҵ��� ���� �ֽ��ϴ�.
        // ��: playerAtk = FindObjectOfType<PlayerAtk>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            Rigidbody enemyRb = other.GetComponent<Rigidbody>();
            if (enemyRb != null)
            {
                // Player�� �ٶ󺸴� �������� ���� ����
                Vector3 forceDirection = playerAtk.player.transform.forward;
                enemyRb.AddForce(forceDirection.normalized * playerAtk.forceMagnitude, ForceMode.Impulse);
            }
        }
    }
}
