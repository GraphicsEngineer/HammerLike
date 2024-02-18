using UnityEngine;

public class WeaponColliderScript : MonoBehaviour
{
    public PlayerAtk playerAtk; // PlayerAtk ?�크립트??참조

    private void Awake()
    {
        // playerAtk 컴포?�트�??�동?�로 찾아???�당???�도 ?�습?�다.
        // ?��?�?FindObjectOfType?� ?�무 비효?�적?�긴 ?�다
        //playerAtk = FindObjectOfType<PlayerAtk>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            Rigidbody enemyRb = other.GetComponent<Rigidbody>();
            if (enemyRb != null)
            {
                //Vector3 forceDirection = playerAtk.player.transform.forward;
                //enemyRb.AddForce(forceDirection.normalized * playerAtk.forceMagnitude, ForceMode.Impulse);
            }
        }
    }
}
