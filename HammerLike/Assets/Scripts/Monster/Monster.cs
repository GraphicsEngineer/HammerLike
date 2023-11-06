using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    }

    void Start()
    {

    }

    void Update()
    {
        //move.Move(stat.walkSpd);
        DetectPlayer();
        ChasePlayer();
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
            Vector3 moveDirection = (playerTransform.position - transform.position).normalized;
            transform.position += moveDirection * stat.walkSpd * Time.deltaTime;

            animCtrl.SetBool("IsChasing", true);
        }
        else
        {
            animCtrl.SetBool("IsChasing", false);
        }
    }


}
