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
    public float attackRange; // �Ʃ���? ???����?����
    [Space(7.5f)]
    public float walkSpd;
    public float runSpd;

    [Space(7.5f)]
    public EnvasionStat envasionStat;

    [Space(7.5f)]
    public float upperHomingSpd; //???�� ?��?? ��???
    public float legHomingSpd; //???�� ?��?? ��???

    public float detectionRange;  // ??����??��?��? ??��??? ��??�� ����?��. ����??��? �ƨ�?����? ?��?? �Ƣ���?.

}

[System.Serializable]
public class DropItem
{
    public int itemID; // ����???? ID
    public float dropChance; // ??�ҩ� ?�硤?
}


public class Monster : MonoBehaviour
{

    private Transform playerTransform;
    // Note: ��?��? ����?? ?? �ҡע�? ??��????��?? ??��? ��?�Ʃ� ��?������? ??����??.
    // ?��?? ��?��? ??���� ����������? ������? ?���Ƣ�???����? ?������ ����?��!!
    // ��?��??��?? ��??����?�ҩ���?��?!!! �������ҩ�?����!!

    public MonsterStat stat;
    public MonsterType monsterType;
    public Slider healthSlider;
    [Header("State Machine")]
    public MonsterFSM fsm;

    [Header("Ranged Attack Settings")]
    public GameObject ProjectilePrefab; // ������?���� �Ʃ���??? ?��?? ?????�� ??���稡?
    public float ProjectileSpeed; // ?????�� ��???
    public Transform ProjectileSpawnPoint; // ����???�� ??���� ?��?��
    private GameObject currentProjectile;

    [Space(10f)]
    [Header("Default Comps")]
    public Transform meshTr;
    public Animator animCtrl;
    public Rigidbody rd;

    [Space(10f)]
    [Header("Action Table")]
    // Note: ?����? ��?����?�� ��?����??���� ��?��? ������??����? ��??�� ??��?
    public MonsterMove move;
    public MonsterAtk atk;
    public MonsterAim monsterAim;

    [Space(10f)]
    [Header("Cam Controller")]
    public CamCtrl camCtrl; // Note: ��?����??�Ƣ� ??������?��? ?��?? ??��??? ??��?�Ƣ� ?????? ?��?? ??��?

    [Header("Drop Items")]
    public List<DropItem> dropItems = new List<DropItem>(); // ??�ҩ� ����???? ��?��?

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
    public LineRenderer lineRenderer; // LineRenderer ???��

    public Collider attackCollider;
    public MeshRenderer attackMeshRenderer;
    private Player player;

    private LineRenderer leftLineRenderer;
    private LineRenderer frontLineRenderer;
    private LineRenderer rightLineRenderer;
    private Vector3 frontKnockbackDirection;
    private Vector3 leftKnockbackDirection;
    private Vector3 rightKnockbackDirection;
    public int debugData = 0;
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
        //animCtrl.SetTrigger("tIdle");
        if (healthSlider != null)
        {
            healthSlider.maxValue = stat.maxHp;
            healthSlider.value = stat.curHp;
        }

        // LineRenderer ��?��? ����?��
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2; // ��????����? ����?��
            lineRenderer.widthMultiplier = 0.05f; // ����?? ��?��?
        }

        leftLineRenderer = CreateLineRenderer(Color.red);
        frontLineRenderer = CreateLineRenderer(Color.green);
        rightLineRenderer = CreateLineRenderer(Color.blue);
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
        // �÷��̾� ���� ��� ���� ������ ������Ʈ
        UpdateDirectionLines();
        if (Input.GetKeyDown(KeyCode.K))
        {
            ApplyKnockback(frontKnockbackDirection);
        }

    }
    void UpdateDirectionLines()
    {
        if (player != null)
        {
            Vector3 playerForward = player.transform.forward;
            Vector3 playerPosition = player.transform.position + Vector3.up * 0.5f;

            // ���� ����
            Vector3 frontDirection = playerForward;
            // ���� �밢�� ����
            Vector3 leftDirection = Quaternion.Euler(0, -45, 0) * playerForward;
            // ���� �밢�� ����
            Vector3 rightDirection = Quaternion.Euler(0, 45, 0) * playerForward;

            // ���� ����
            frontKnockbackDirection = frontDirection;
            leftKnockbackDirection = leftDirection;
            rightKnockbackDirection = rightDirection;

            // �� ���⿡ ���� ���� ������ ����
            SetLineRenderer(leftLineRenderer, playerPosition, playerPosition + leftDirection * 5); // 5�� ������ ����
            SetLineRenderer(frontLineRenderer, playerPosition, playerPosition + frontDirection * 5);
            SetLineRenderer(rightLineRenderer, playerPosition, playerPosition + rightDirection * 5);
        }
    }

    private void SetLineRenderer(LineRenderer lineRenderer, Vector3 start, Vector3 end)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }


    Vector3 CalculateKnockbackDirectionBasedOnContext()
    {
        // ���⼭�� ���ø� ���� �ܼ�ȭ�� ������ ����մϴ�.
        // ���� ���������� ������ ����, ��ġ, �÷��̾���� ���� ���� ����Ͽ� �˹� ������ ����ؾ� �մϴ�.
        // ���� ���, ���Ͱ� �÷��̾ ���ϰ� �ִٸ�, �÷��̾�� �ݴ� �������� �˹� ������ ������ �� �ֽ��ϴ�.
        return transform.forward; // ����� ���Ͱ� �ٶ󺸴� �������� ����
    }

    void DrawDirectionLine()
    {
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, transform.position); // ����?? ��????��: ��?����???? ?��?��
            lineRenderer.SetPosition(1, transform.position + transform.forward * 5f); // ����?? ����?��: ��?����??�Ƣ� ��?��?������? ����??
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
            FaceTarget(); // ??����??��?��? ??��????����? ��?��?������? ??��? ��������??
        }

    }

    private LineRenderer CreateLineRenderer(Color lineColor)
    {
        GameObject lineRendererObject = new GameObject("LineRenderer");
        lineRendererObject.transform.SetParent(transform);
        LineRenderer lineRenderer = lineRendererObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = 2;

        return lineRenderer;
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
            if (weaponCollider != null)
            {
                // Debug �α׷� ���� ���� ID�� ����մϴ�.
                Debug.Log($"[Monster] Attack ID: {weaponCollider.CurrentAttackId}");

                if (!processedAttacks.Contains(weaponCollider.CurrentAttackId))
                {
                    // ���� ó�� �� �ؽü¿� �ش� ���� ID�� ���ٴ� ���� �α׷� ����մϴ�.
                    Debug.Log($"[Monster] Processing new attack ID: {weaponCollider.CurrentAttackId}");

                    PlayerAtk playerAttack = other.GetComponentInParent<PlayerAtk>();
                    if (playerAttack != null)
                    {
                        TakeDamage(playerAttack.attackDamage);
                        Vector3 hitPoint = other.ClosestPointOnBounds(transform.position);
                        Vector3 knockbackDirection = DetermineKnockbackDirection(hitPoint, other.transform);
                        ApplyKnockback(knockbackDirection);

                        // ���� ó�� �� �ش� ���� ID�� �ؽü¿� �߰��մϴ�.
                        processedAttacks.Add(weaponCollider.CurrentAttackId);
                        Debug.Log($"[Monster] Added attack ID to processedAttacks: {weaponCollider.CurrentAttackId}");
                    }
                }
                else
                {
                    // �ߺ��� ������ ������ �� �ش� ���� ID�� �α׷� ����մϴ�.
                    Debug.Log($"[Monster] Duplicate attack ID encountered: {weaponCollider.CurrentAttackId}");
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

    private Vector3 DetermineKnockbackDirection(Vector3 hitPoint, Transform trailMeshTransform)
    {
        // Ʈ���� �޽��� ���� ��� (���� �ڵ�, ���� ���� �ʿ�)
        float trailMeshLength = Vector3.Distance(trailMeshTransform.position, trailMeshTransform.position + trailMeshTransform.forward * 10); // �޽� ���� ����
        float hitPositionRelative = Vector3.Distance(trailMeshTransform.position, hitPoint); // �ǰ� ���������� �Ÿ�

        // �ǰ� ��ġ�� Ʈ���� �޽��� ��� 1/3 ������ �ִ��� ����
        // �밢�� �˹�
        float sectionLength = trailMeshLength / 3;
        if (hitPositionRelative <= sectionLength)
        {
            // ���� �밢�� �˹�
            return rightKnockbackDirection;
        }
        else if (hitPositionRelative > sectionLength && hitPositionRelative <= sectionLength * 2)
        {
            // ���� �˹�
            return frontKnockbackDirection;
        }
        else
        {
            // ���� �밢�� �˹�
            return leftKnockbackDirection;
        }
    }




    private void ApplyKnockback(Vector3 direction)
    {
        if (isKnockedBack) return; // �̹� �˹� ���� ��� �˹��� �������� ����

        float knockbackIntensity = 300f; // �˹� ����
        direction.y = 0; // Y�� ������ 0���� �����Ͽ� ���� �˹��� ����
        Vector3 force = direction.normalized * knockbackIntensity;

        // �˹� ���� �� Velocity �α�
        Debug.Log($"[Monster] Pre-Knockback Velocity: {rd.velocity}");

        // �˹� ����� �� �α�
        Debug.Log($"[Monster] Applying Knockback. Direction: {direction}, Force: {force}");

        // �˹� �� ����
        rd.AddForce(force, ForceMode.Impulse);
        isKnockedBack = true;

        // �˹� ���� �� ���� Velocity �α� (���� ���� ���� Velocity�� ���� �����ӿ��� Ȯ�� ����)
        Debug.Log($"[Monster] Expected Post-Knockback Velocity: {rd.velocity + force}");

        StartCoroutine(KnockBackDuration());
    }


    private IEnumerator KnockBackDamageCooldown()
    {
        yield return new WaitForSeconds(1f); // ��?��? ??��??? ?����?��?
        canTakeKnockBackDamage = true;
    }

    private void TakeDamage(float damage)
    {
        if (stat.curHp <= 0) return; // ??��? ??��??? �Ʃ���? ??��???��? ����?? ��??��

        if (stat.curHp > 0)  // ��?����??�Ƣ� ??����???? �ҡע��� ??��? ?������
        {
            stat.curHp -= damage;
            if (healthSlider != null)
            {
                healthSlider.value = stat.curHp;
                ShowHealthSlider();  // ?����? UI ������???��? ??��?
            }

            if (stat.curHp <= 0)
            {
                Die();
            }
        }
    }

    /*private void ApplyKnockback(Vector3 direction)
    {
        float knockbackIntensity = 300f; // ��?��? �ơ�??
        direction.y = 0; // Y?? ��??�� ??��?
        GetComponent<Rigidbody>().AddForce(direction.normalized * knockbackIntensity, ForceMode.Impulse);
        isKnockedBack = true;
        StartCoroutine(KnockBackDuration());
    }*/

    private IEnumerator KnockBackDuration()
    {
        yield return new WaitForSeconds(1.5f); // ��?��? ??��? ��?��?
        isKnockedBack = false;
    }

    private void Die()
    {
        // ��?����?? ??��? ?������
        // ����: gameObject.SetActive(false); ��?��? Destroy(gameObject);
        animCtrl.SetBool("IsChasing", false);
        animCtrl.SetTrigger("tDead");
        DisableAttackCollider();
        DisableAttackMeshRenderer();
        // NavMeshAgent ��??�ƨ���?��
        if (nmAgent != null && nmAgent.isActiveAndEnabled)
        {
            nmAgent.isStopped = true;
            nmAgent.enabled = false;
        }

        playerTransform = null;
        DropItems();
        //Destroy(gameObject);
    }
    private void ShowHealthSlider()
    {
        if (healthSlider != null)
        {
            healthSlider.gameObject.SetActive(true);
            StopCoroutine("HideHealthSlider");  // ??��? ?��?? ?��?? ??��?����?? ??��?��? ?����?
            StartCoroutine("HideHealthSlider");  // ?? ??��?���� ��???
        }
    }

    private IEnumerator HideHealthSlider()
    {
        yield return new WaitForSeconds(2f);
        if (healthSlider != null && stat.curHp > 0)  // ��?����??�Ƣ� ??����???? �ҡע��� ������???��? ��??�ƨ���?��
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
        if (stat.curHp <= 0) return; // ?����??? 0 ????��? �ơ�?? ?��??
        if (Vector3.Distance(transform.position, target.position) <= stat.detectionRange)
        {
            playerTransform = target; // ��??�� ��??��?? ????
            player = target.GetComponent<Player>(); // target�������� Player ??������?���碬? �Ƣ�?�碯?

            if (player != null)
            {
                monsterAim.SetTarget(target); // MonsterAim ����??�������碯��?? ?����? ����?��
            }
        }
        else
        {
            playerTransform = null;
            player = null; // Player ???��?? ?��??
            monsterAim.SetTarget(null); // MonsterAim ����??��������?? ?����??? ?��??
        }
    }


    void ChasePlayer()
    {
        if (stat.curHp <= 0 || animCtrl.GetBool("IsAttacking") || animCtrl.GetBool("IsAiming")) return; // ?����??? 0 ????��?���� �Ʃ���? ?��??��? ?����? ?��??
        float distanceToTarget = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToTarget <= stat.detectionRange)
        {
            if (distanceToTarget > stat.attackRange)
            {
                nmAgent.SetDestination(playerTransform.position);
                DisableAttackCollider();
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
            DisableAttackCollider();
        }
    }

    // ??����??��?��? ��?��?������? ??��? ��������??
    private void FaceTarget()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * stat.legHomingSpd);
    }



    void Attack()
    {
        float distanceToTarget = Vector3.Distance(transform.position, playerTransform.position);
        if (stat.curHp <= 0 || distanceToTarget > stat.attackRange) return; // ?����??? 0 ????��?���� ???����?���� ��???��? �Ʃ���? ?��??
        if (monsterType == MonsterType.Melee)
        {
            FaceTarget();
            StartAttack();
            animCtrl.SetTrigger("tAttack");
        }
        else if (monsterType == MonsterType.Ranged)
        {
            FaceTarget();
            animCtrl.SetBool("IsAiming", true);
            HandleRangedAttack();
        }
        else
        {
            // ?��??���� ��?��??? ????
        }

    }

    void DropItems()
    {
        ItemManager itemManager = FindObjectOfType<ItemManager>();
        foreach (DropItem dropItem in dropItems)
        {
            if (UnityEngine.Random.Range(0f, 100f) < dropItem.dropChance)
            {
                // ����???? ??���� ���� ??�ҩ�
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

        // ?��???? ����?����? ?��?�� NavMeshAgent��? ��??�ƨ���?��??��?��?.
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
        if (stat.curHp > 0)  // ?����??? 0 ?????? �ҡע��� tIdle ���碬���?��? ����?��
        {


            // ?��???? ??�Ʃ�??��? ?��?�� NavMeshAgent��? ?�ƨ���?��??��?��?.
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
            FireProjectile(); // ������?���� ?????�� ����?? ��������??
        }
    }

    private void FireProjectile()
    {
        if (currentProjectile != null || ProjectilePrefab == null) return;

        Vector3 spawnPosition = ProjectileSpawnPoint != null ? ProjectileSpawnPoint.position : transform.position;
        Vector3 targetDirection = (playerTransform.position - spawnPosition).normalized;
        Quaternion spawnRotation = Quaternion.LookRotation(targetDirection);

        // ?????�� ??����??���� ??����
        currentProjectile = Instantiate(ProjectilePrefab, spawnPosition, spawnRotation);

        // ?????������ Rigidbody ??������?���碬? �Ƣ�?�碯?��?���� ?���Ƣ�
        Rigidbody rb = currentProjectile.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = currentProjectile.AddComponent<Rigidbody>();
        }

        // ?����? ��????? ����?? ��???��? ����?��
        rb.useGravity = false;

        // ?????������ ��??? ??��?
        rb.velocity = targetDirection * ProjectileSpeed;

        Projectile projectileComponent = currentProjectile.GetComponent<Projectile>();
        if (projectileComponent != null)
        {
            projectileComponent.SetShooter(this);
        }
        // ?????�� ��?��? ��??��?�� ?����? ?????�� ����??�������碯�� ����??
    }


    // ?????���Ƣ� ��?��???��??? �ҡ� ??????��? ��������??
    public void ProjectileDestroyed()
    {
        currentProjectile = null;
    }
    public void EnableAttackMeshRenderer()
    {
        if (stat.curHp > 0)
            attackMeshRenderer.enabled = true;
    }
    public void DisableAttackMeshRenderer()
    {
        attackMeshRenderer.enabled = false;
    }


    public void EnableAttackCollider()
    {
        if (stat.curHp > 0)
            attackCollider.enabled = true;
    }

    // �Ʃ���?��? Collider ��??�ƨ���?��
    public void DisableAttackCollider()
    {
        attackCollider.enabled = false;
    }




}