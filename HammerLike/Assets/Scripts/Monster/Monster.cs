using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Johnson;
using UnityEngine.AI;

public enum MonsterType
{
    Melee,
    Ranged,
    Special
}

[Serializable]
public struct MonsterStat
{
    public float maxHp;
    public float curHp;
    public float attackPoint;
    public float attackRange; // ���� �����Ÿ�
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

[System.Serializable]
public class DropItem
{
    public int itemID; // ������ ID
    public float dropChance; // ��� Ȯ��
}


public class Monster : MonoBehaviour
{

    private Transform playerTransform;
    // Note: ��� ���� �� ���� ���������� ũ�� �Ű� �Ⱦ��� �۾���.
    // ���� ��� �۾� ������ ���� �߰������� ���� ����!!
    // �����ص� ���عٶ��ϴ�!!! �����̼�!!

    public MonsterStat stat;
    public MonsterType monsterType;
    public Slider healthSlider;
    [Header("State Machine")]
    public MonsterFSM fsm;

    [Header("Ranged Attack Settings")]
    public GameObject ProjectilePrefab; // ���Ÿ� ������ ���� ����ü ������
    public float ProjectileSpeed; // ����ü �ӵ�
    public Transform ProjectileSpawnPoint; // �߻�ü ���� ��ġ
    private GameObject currentProjectile;

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

    [Header("Drop Items")]
    public List<DropItem> dropItems = new List<DropItem>(); // ��� ������ ���

    [Space(10f)]
    [Header("Anim Bones")]
    public Transform headBoneTr;
    public Transform spineBoneTr;
    public Transform hpBoneTr;
    private HashSet<int> processedAttacks = new HashSet<int>();

    private bool isKnockedBack = false;
    private bool canTakeKnockBackDamage = true;

    public Transform target;
    NavMeshAgent nmAgent;
    public LineRenderer lineRenderer; // LineRenderer ����

    public Collider attackCollider;
    public MeshRenderer attackMeshRenderer;
    private Player player;

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
        nmAgent = GetComponent<NavMeshAgent>();
        animCtrl.SetBool("IsChasing", true);
        if (healthSlider != null)
        {
            healthSlider.maxValue = stat.maxHp;
            healthSlider.value = stat.curHp;
        }

        // LineRenderer �⺻ ����
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2; // �������� ����
            lineRenderer.widthMultiplier = 0.05f; // ���� �ʺ�
        }
    }

    void Update()
    {
        if (stat.curHp > 0)
        {
            DetectPlayer();

            if (playerTransform != null)
            {
                ChasePlayer();
            }
            else
            {
                animCtrl.SetBool("IsChasing", false);
                animCtrl.SetTrigger("tIdle");
            }
        }
        else
        {
            animCtrl.SetBool("IsChasing", false);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            stat.curHp = 0;
            Die();
        }


    }

    void DrawDirectionLine()
    {
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, transform.position); // ���� ������: ������ ��ġ
            lineRenderer.SetPosition(1, transform.position + transform.forward * 5f); // ���� ����: ���Ͱ� �ٶ󺸴� ����
        }
    }

    private void LateUpdate()
    {
        Vector3 lookDir = monsterAim.Aiming();
        //Funcs.LookAtSpecificBone(spineBoneTr, eGizmoDir.Forward, lookDir, Vector3.zero);
    }

    private void FixedUpdate()
    {
        if (playerTransform != null && stat.curHp > 0)
        {
            FaceTarget(); // �÷��̾ ���������� �ٶ󺸰� �ϴ� �޼���
        }

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
        if (stat.curHp <= 0) return; // �̹� ����� ��� �������� ���� ����

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
        animCtrl.SetBool("IsChasing", false);
        animCtrl.SetTrigger("tDead");

        // NavMeshAgent ��Ȱ��ȭ
        if (nmAgent != null && nmAgent.isActiveAndEnabled)
        {
            nmAgent.isStopped = true;
            nmAgent.enabled = false;
        }


        DropItems();
        //Destroy(gameObject);
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
    public Player Player
    {
        get { return player; }
    }
    private void DetectPlayer()
    {
        if (stat.curHp <= 0) return; // ü���� 0 ���ϸ� ���� ����
        if (Vector3.Distance(transform.position, target.position) <= stat.detectionRange)
        {
            playerTransform = target; // ���� ������ ����
            player = target.GetComponent<Player>(); // target���� Player ������Ʈ�� ������

            if (player != null)
            {
                monsterAim.SetTarget(target); // MonsterAim ��ũ��Ʈ���� Ÿ�� ����
            }
        }
        else
        {
            playerTransform = null;
            player = null; // Player ������ ����
            monsterAim.SetTarget(null); // MonsterAim ��ũ��Ʈ�� Ÿ�ٵ� ����
        }
    }


    void ChasePlayer()
    {
        if (stat.curHp <= 0 || animCtrl.GetBool("IsAttacking") || animCtrl.GetBool("IsAiming")) return; // ü���� 0 ���ϰų� ���� ���̸� �߰� ����
        float distanceToTarget = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToTarget <= stat.detectionRange)
        {
            if (distanceToTarget > stat.attackRange)
            {
                nmAgent.SetDestination(playerTransform.position);
                animCtrl.SetBool("IsChasing", true);
                animCtrl.SetBool("IsAttacking", false);
            }
            else
            {
                animCtrl.SetBool("IsChasing", false);
                Attack();
            }
        }
        else
        {
            animCtrl.SetBool("IsChasing", false);
            animCtrl.SetTrigger("tIdle");
        }
    }

    // �÷��̾ �ٶ󺸰� �ϴ� �޼���
    private void FaceTarget()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * stat.legHomingSpd);
    }



    void Attack()
    {
        float distanceToTarget = Vector3.Distance(transform.position, playerTransform.position);
        if (stat.curHp <= 0 || distanceToTarget > stat.attackRange) return; // ü���� 0 ���ϰų� �����Ÿ� ���̸� ���� ����
        if (monsterType==MonsterType.Melee)
        {
            FaceTarget();
            StartAttack();
            animCtrl.SetTrigger("tAttack");
        }
        else if(monsterType == MonsterType.Ranged)
        {
            FaceTarget();
            animCtrl.SetBool("IsAiming", true);
            HandleRangedAttack();
        }
        else
        {
            // ���Ŀ� Ư���� ����
        }
       
    }

    void DropItems()
    {
        ItemManager itemManager = FindObjectOfType<ItemManager>();
        foreach (DropItem dropItem in dropItems)
        {
            if (UnityEngine.Random.Range(0f, 100f) < dropItem.dropChance)
            {
                // ������ ���� �� ���
                itemManager.DropItem(dropItem.itemID, transform.position);
            }
        }
    }

    void StartAttack()
    {
        if (monsterType == MonsterType.Melee)
        {
            animCtrl.SetBool("IsAttacking", true);
        }
        else if (monsterType == MonsterType.Ranged)
        {
            animCtrl.SetBool("IsAiming", true);
        }
        
        // ������ ���߱� ���� NavMeshAgent�� ��Ȱ��ȭ�մϴ�.
        if (nmAgent != null && nmAgent.enabled)
        {
            nmAgent.isStopped = true;
        }
    }

    public void EndAttack()
    {
        if (monsterType == MonsterType.Melee)
        {
            animCtrl.SetBool("IsAttacking", false);
        }
        else if (monsterType == MonsterType.Ranged)
        {
            animCtrl.SetBool("IsAiming", false);
        }
        float distanceToTarget = Vector3.Distance(transform.position, playerTransform.position);
        if (stat.curHp > 0)  // ü���� 0 �̻��� ���� tIdle Ʈ���Ÿ� ����
        {
            

                // ������ �簳�ϱ� ���� NavMeshAgent�� Ȱ��ȭ�մϴ�.
                if (nmAgent != null && nmAgent.enabled && distanceToTarget <= stat.detectionRange)
                {
                    nmAgent.isStopped = false;
                    if (playerTransform != null && distanceToTarget > nmAgent.stoppingDistance)
                    {
                        nmAgent.SetDestination(playerTransform.position);
                        animCtrl.SetBool("IsChasing", true);
                    }
                    else if (playerTransform != null && distanceToTarget <= nmAgent.stoppingDistance)
                    {
                        Attack();
                    }
                }
                else
                {
                animCtrl.SetTrigger("tIdle");
                }
            
            
        }
        
    }

    private void HandleRangedAttack()
    {
        if (playerTransform != null && Vector3.Distance(transform.position, playerTransform.position) <= stat.attackRange)
        {
            FaceTarget();
            animCtrl.SetTrigger("tShot");
            FireProjectile(); // ���Ÿ� ����ü �߻� �޼���
        }
    }

    private void FireProjectile()
    {
        if (currentProjectile != null || ProjectilePrefab == null) return;

        Vector3 spawnPosition = ProjectileSpawnPoint != null ? ProjectileSpawnPoint.position : transform.position;
        Vector3 targetDirection = (playerTransform.position - spawnPosition).normalized;
        Quaternion spawnRotation = Quaternion.LookRotation(targetDirection);

        // ����ü �ν��Ͻ� ����
        currentProjectile = Instantiate(ProjectilePrefab, spawnPosition, spawnRotation);

        // ����ü�� Rigidbody ������Ʈ�� �������ų� �߰�
        Rigidbody rb = currentProjectile.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = currentProjectile.AddComponent<Rigidbody>();
        }

        // �߷� ������ ���� �ʵ��� ����
        rb.useGravity = false;

        // ����ü�� �ӵ� ����
        rb.velocity = targetDirection * ProjectileSpeed;

        Projectile projectileComponent = currentProjectile.GetComponent<Projectile>();
        if (projectileComponent != null)
        {
            projectileComponent.SetShooter(this);
        }
        // ����ü �ı� ������ �ش� ����ü ��ũ��Ʈ�� ����
    }


    // ����ü�� �ı��Ǿ��� �� ȣ���ϴ� �޼���
    public void ProjectileDestroyed()
    {
        currentProjectile = null;
    }
    public void EnableAttackMeshRenderer()
    {
        attackMeshRenderer.enabled = true;
    }
    public void DisableAttackMeshRenderer()
    {
        attackMeshRenderer.enabled = false;
    }


    public void EnableAttackCollider()
    {
        attackCollider.enabled = true;
    }

    // ���ݿ� Collider ��Ȱ��ȭ
    public void DisableAttackCollider()
    {
        attackCollider.enabled = false;
    }




}

