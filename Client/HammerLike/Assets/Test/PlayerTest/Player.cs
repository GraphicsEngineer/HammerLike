using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using static UnityEngine.GraphicsBuffer;

public class Player : HPObject
{
    enum MainState
    {
        Idle,
        Damaged,
        Move,
        Dash
    }

    enum ActionState
    {
        Idle,
        Attack,
        Charge
    }

    [Header("Player")]
    public float defaultMoveSpeed;
    public float minMoveSpeed;
    public float maxMoveSpeed;

    [Header("objects")]
    public Transform upperbody;
    public Transform lowerbody;

    MainState mainState;
    ActionState actionState;

    float extraMoveSpeed
    {
        get
        {
            // ������, ����, ������� �߰��Ǹ� ���� �ʿ�
            return 0;
        }
    }

    float moveSpeed
    {
        get
        {
            return Mathf.Clamp(defaultMoveSpeed + extraMoveSpeed, minMoveSpeed, maxMoveSpeed);
        }
    }

    // Implement HPObject's abstract function
    protected override float extraHp
    {
        get { return 0; }
    }

    public override void OnDamaged(Vector3 direction, float damage, float force, DamageType damageType, float specialDamage)
    {
        throw new System.NotImplementedException();
    }

    Queue<MainState> mainStateQueue = new Queue<MainState>();
    Queue<ActionState> actionStateQueue = new Queue<ActionState>();

    void ChangeMainState(MainState state)
    {
        mainStateQueue.Enqueue(state);
    }

    void ChangeActionState(ActionState state)
    {
        actionStateQueue.Enqueue(state);
    }

    void ChangeMainStateLow(MainState state, bool isCallByStart = false)
    {
        if (!isCallByStart)
        {
            switch (state)
            {
                case MainState.Idle:
                    {
                        MainStateIdleExit();

                        break;
                    }
                case MainState.Damaged:
                    {
                        MainStateDamagedExit();

                        break;
                    }
                case MainState.Move:
                    {
                        MainStateMoveExit();

                        break;
                    }
                case MainState.Dash:
                    {
                        MainStateDashExit();

                        break;
                    }
                default:
                    {
                        Debug.Assert(false, "Player�� Main State Exit�� ���ǵ��� ����");
                        break;
                    }
            }
        }

        mainState = state;

        switch (state)
        {
            case MainState.Idle:
                {
                    MainStateIdleEnter();

                    break;
                }
            case MainState.Damaged:
                {
                    MainStateDamagedEnter();

                    break;
                }
            case MainState.Move:
                {
                    MainStateMoveEnter();

                    break;
                }
            case MainState.Dash:
                {
                    MainStateDashEnter();

                    break;
                }
            default:
                {
                    Debug.Assert(false, "Player�� Main State Enter�� ���ǵ��� ����");
                    break;
                }
        }
    }

    void ChangeActionStateLow(ActionState state, bool isCallByStart = false)
    {
        if (!isCallByStart)
        {
            switch (state)
            {
                case ActionState.Idle:
                    {
                        ActionStateIdleExit();

                        break;
                    }
                case ActionState.Attack:
                    {
                        ActionStateAttackExit();

                        break;
                    }
                case ActionState.Charge:
                    {
                        ActionStateChargeExit();

                        break;
                    }
                default:
                    {
                        Debug.Assert(false, "Player�� Action State Exit�� ���ǵ��� ����");
                        break;
                    }
            }
        }

        actionState = state;

        switch (state)
        {
            case ActionState.Idle:
                {
                    ActionStateIdleEnter();

                    break;
                }
            case ActionState.Attack:
                {
                    ActionStateAttackEnter();

                    break;
                }
            case ActionState.Charge:
                {
                    ActionStateChargeEnter();

                    break;
                }
            default:
                {
                    Debug.Assert(false, "Player�� Action State Enter�� ���ǵ��� ����");
                    break;
                }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        ChangeMainStateLow(MainState.Idle, true);
        ChangeActionStateLow(ActionState.Idle, true);
    }

    // Update is called once per frame
    void Update()
    {
        switch(mainState)
        {
            case MainState.Idle:
                {
                    MainStateIdleUpdate();
                    break;
                }
            case MainState.Damaged:
                {
                    MainStateDamagedUpdate();
                    break;
                }
            case MainState.Move:
                {
                    MainStateMoveUpdate();
                    break;
                }
            case MainState.Dash:
                {
                    MainStateDashUpdate();
                    break;
                }
            default:
                {
                    Debug.Assert(false, "Player�� Main State Update�� ���ǵ��� ����");
                    break;
                }
        }

        switch (actionState)
        {
            case ActionState.Idle:
                {
                    ActionStateIdleUpdate();
                    break;
                }
            case ActionState.Attack:
                {
                    ActionStateAttackUpdate();
                    break;
                }
            case ActionState.Charge:
                {
                    ActionStateChargeUpdate();
                    break;
                }
            default:
                {
                    Debug.Assert(false, "Player�� Action State Update�� ���ǵ��� ����");
                    break;
                }
        }

        while (mainStateQueue.Count > 0 || actionStateQueue.Count > 0)
        {
            if (mainStateQueue.Count > 0)
            {
                MainState nextState = mainStateQueue.Dequeue();
                ChangeMainStateLow(nextState);
            }
            else
            {
                ActionState nextState = actionStateQueue.Dequeue();
                ChangeActionStateLow(nextState);
            }
        }
    }

    // Main State
    void MainStateIdleEnter()
    {

    }

    void MainStateIdleUpdate()
    {
        if (Input.GetAxis("Horizontal") != 0.0f || Input.GetAxis("Vertical") != 0.0f)
        {
            ChangeMainState(MainState.Move);
        }
    }

    void MainStateIdleExit()
    {

    }

    void MainStateDamagedEnter()
    {

    }

    void MainStateDamagedUpdate()
    {

    }

    void MainStateDamagedExit()
    {

    }

    void MainStateMoveEnter()
    {

    }

    void MainStateMoveUpdate()
    {
        Vector3 moveDelta = (new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"))).normalized;
        Move(moveDelta);

        if (Input.GetAxis("Horizontal") == 0.0f && Input.GetAxis("Vertical") == 0.0f)
        {
            ChangeMainState(MainState.Idle);
        }
    }

    void MainStateMoveExit()
    {

    }

    void MainStateDashEnter()
    {

    }

    void MainStateDashUpdate()
    {

    }

    void MainStateDashExit()
    {

    }


    // Action State
    void ActionStateIdleEnter()
    {

    }

    void ActionStateIdleUpdate()
    {
        switch(mainState)
        {
            case MainState.Idle:
                {
                    // ���콺 ��ġ�� upperbody��ġ�� ������ ���� �ڵ�
                    // ī�޶� ������ �ٲ�� �ٴ��� �� �򸮸� �����ؾ��Ѵ�.
                    Vector3 dir = Camera.main.ScreenToViewportPoint(Input.mousePosition) - Camera.main.WorldToViewportPoint(upperbody.position);
                    Vector3 target = new Vector3(dir.x, 0.0f, dir.y);

                    Aim(target);

                    break;
                }
            case MainState.Move:
                {
                    // ���콺 ��ġ�� upperbody��ġ�� ������ ���� �ڵ�
                    // ī�޶� ������ �ٲ�� �ٴ��� �� �򸮸� �����ؾ��Ѵ�.
                    Vector3 dir = Camera.main.ScreenToViewportPoint(Input.mousePosition) - Camera.main.WorldToViewportPoint(upperbody.position);
                    Vector3 target = new Vector3(dir.x, 0.0f, dir.y);

                    Aim(target);

                    break;
                }
            default:
                {
                    Debug.Assert(false);
                    break;
                }
        }
    }

    void ActionStateIdleExit()
    {

    }

    void ActionStateAttackEnter()
    {

    }

    void ActionStateAttackUpdate()
    {

    }

    void ActionStateAttackExit()
    {

    }

    void ActionStateChargeEnter()
    {

    }

    void ActionStateChargeUpdate()
    {

    }

    void ActionStateChargeExit()
    {

    }


    void Move(Vector3 moveDelta)
    {
        transform.position = transform.position + moveDelta.normalized * moveSpeed * Time.deltaTime;

        if (moveDelta == Vector3.zero)
        {
            // �е忡�� ���⿡ ���� �Է��� ���� ����� ����ó�� �ʿ�
        }
        else
        {
            transform.forward = moveDelta.normalized;
        }
    }

    void Aim(Vector3 targetDirection)
    {
        float angle = Vector3.SignedAngle(targetDirection, lowerbody.forward, Vector3.up);

        Debug.Log(angle);
        if (angle > 90.0f)
        {
            upperbody.forward = Vector3.Cross(lowerbody.forward, Vector3.up);
        }
        else if (angle < -90.0f)
        {
            upperbody.forward = Vector3.Cross(Vector3.up, lowerbody.forward);
        }
        else
        {
            upperbody.forward = targetDirection.normalized;
        }
    }
}
