using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Johnson;

[Serializable]
public struct MonsterStat
{
    public float maxHp;
    public float curHp;

    [Space(7.5f)]
    public float walkSpd;
    public float runSpd;

    [Space(7.5f)]
    public EnvasionStat envasionStat;

    [Space(7.5f)]
    public float upperHomingSpd; //��ü ȸ�� �ӵ�
    public float legHomingSpd; //��ü ȸ�� �ӵ�

    public float detectionRange;  // �÷��̾ �ν��� ���� ����. ���ϴ� ������ ���� ����.

}

public class Monster : MonoBehaviour
{

    private Transform playerTransform;
    // Note: ��� ���� �� ���� ���������� ũ�� �Ű� �Ⱦ��� �۾���.
    // ���� ��� �۾� ������ ���� �߰������� ���� ����!!
    // �����ص� ���عٶ��ϴ�!!! �����̼�!!

    public MonsterStat stat;
    public Slider healthSlider;
    [Header("State Machine")]
    public MonsterFSM fsm;

    [Space(10f)]
    [Header("Default Comps")]
    public Transform meshTr;
    public Animator animCtrl;
    public Rigidbody rd;

    [Space(10f)]
    [Header("Action Table")]
    // Note: �ش� �κ��� ���Ϳ� �´� �׼����� ���� �ʿ�
    public MonsterMove move;
    public MonsterAtk atk;
    public MonsterAim monsterAim;

    [Space(10f)]
    [Header("Cam Controller")]
    public CamCtrl camCtrl; // Note: ���Ͱ� ī�޶� ���� ������ �ʿ䰡 ������ Ȯ�� �ʿ�

    [Space(10f)]
    [Header("Anim Bones")]
    public Transform headBoneTr;
    public Transform spineBoneTr;
    public Transform hpBoneTr;
    private HashSet<int> processedAttacks = new HashSet<int>();

    private bool isKnockedBack = false;
    private bool canTakeKnockBackDamage = true;

    private void Awake()
    {
        if (!fsm)
        { fsm = GetComponent<MonsterFSM>(); }

        if (!fsm)
        {
            gameObject.AddComponent<MonsterFSM>();
        }

        if (!rd)
        {
            rd = GetComponent<Rigidbody>();
        }
        stat.curHp = stat.maxHp;
    }

    void Start()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = stat.maxHp;
            healthSlider.value = stat.curHp;
        }
    }

    void Update()
    {
        //move.Move(stat.walkSpd);
        DetectPlayer();
        ChasePlayer();

        if (healthSlider != null)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position + new Vector3(-2.7f, 1.2f, 0));
            healthSlider.transform.position = screenPosition;
        }
    }

    private void LateUpdate()
    {
        Vector3 lookDir = monsterAim.Aiming();
        //Funcs.LookAtSpecificBone(spineBoneTr, eGizmoDir.Forward, lookDir, Vector3.zero);
    }

    private void FixedUpdate()
    {

    }

    private void OnEnable()
    {

    }
    private void OnDisable()
    {

    }

    private void OnDestroy()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WeaponCollider"))
        {
            WeaponCollider weaponCollider = other.GetComponent<WeaponCollider>();
            if (weaponCollider != null && !processedAttacks.Contains(weaponCollider.CurrentAttackId))
            {
                // ������ �� �˹� ó��
                PlayerAtk playerAttack = other.GetComponentInParent<PlayerAtk>();
                if (playerAttack != null)
                {
                    TakeDamage(playerAttack.attackDamage);
                    ApplyKnockback(playerAttack.transform.forward);
                    processedAttacks.Add(weaponCollider.CurrentAttackId);
                }

            }
        }
        if (other.gameObject.CompareTag("KnockBackable") && isKnockedBack && canTakeKnockBackDamage)
        {
            TakeDamage(10f); // KnockBackDamage
            canTakeKnockBackDamage = false;
            StartCoroutine(KnockBackDamageCooldown());
        }
    }

    private IEnumerator KnockBackDamageCooldown()
    {
        yield return new WaitForSeconds(1f); // �˹� ������ ��ٿ�
        canTakeKnockBackDamage = true;
    }

    private void TakeDamage(float damage)
    {
        if (stat.curHp > 0)  // ���Ͱ� ������� ���� �ǰ� ó��
        {
            stat.curHp -= damage;
            if (healthSlider != null)
            {
                healthSlider.value = stat.curHp;
                ShowHealthSlider();  // ü�� UI �����̴� ǥ��
            }

            if (stat.curHp <= 0)
            {
                Die();
            }
        }
    }

    private void ApplyKnockback(Vector3 direction)
    {
        float knockbackIntensity = 30f; // �˹� ����
        direction.y = 0; // Y�� ��ȭ ����
        GetComponent<Rigidbody>().AddForce(direction.normalized * knockbackIntensity, ForceMode.Impulse);
        isKnockedBack = true;
        StartCoroutine(KnockBackDuration());
    }

    private IEnumerator KnockBackDuration()
    {
        yield return new WaitForSeconds(1f); // �˹� ���� �ð�
        isKnockedBack = false;
    }

    private void Die()
    {
        // ���� ��� ó��
        // ��: gameObject.SetActive(false); �Ǵ� Destroy(gameObject);
        Destroy(gameObject);
    }
    private void ShowHealthSlider()
    {
        if (healthSlider != null)
        {
            healthSlider.gameObject.SetActive(true);
            StopCoroutine("HideHealthSlider");  // �̹� ���� ���� �ڷ�ƾ�� �ִٸ� �ߴ�
            StartCoroutine("HideHealthSlider");  // �� �ڷ�ƾ ����
        }
    }

    private IEnumerator HideHealthSlider()
    {
        yield return new WaitForSeconds(2f);
        if (healthSlider != null && stat.curHp > 0)  // ���Ͱ� ������� ���� �����̴� ��Ȱ��ȭ
        {
            healthSlider.gameObject.SetActive(false);
        }
    }

    private void DetectPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, stat.detectionRange);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))  // Player �±׸� ���� ������Ʈ�� �ν�
            {
                playerTransform = hit.transform;
                monsterAim.SetTarget(playerTransform);
                break;
            }
        }
    }


    void ChasePlayer()
    {
        if (playerTransform != null)
        {
            // �÷��̾ ���� �̵�
            Vector3 moveDirection = (playerTransform.position - transform.position).normalized;
            transform.position += moveDirection * stat.walkSpd * Time.deltaTime;

            // �÷��̾ �ٶ󺸵��� Y�� ȸ��
            Vector3 lookDirection = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
            transform.LookAt(lookDirection);

            animCtrl.SetBool("IsChasing", true);
        }
        else
        {
            animCtrl.SetBool("IsChasing", false);
        }
    }



}
