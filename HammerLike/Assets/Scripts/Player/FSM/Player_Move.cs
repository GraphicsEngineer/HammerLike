using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : cState
{
    public Player player;

    public Player_Move(Player _player)
    {
        player = _player;
    }

    public override void EnterState()
    {
        base.EnterState();
        player.animCtrl.SetLayerWeight(1, 1f);
        player.animCtrl.SetTrigger("tWalk");
    }

    public override void UpdateState()
    {
        base.UpdateState();
        player.aim.Aiming();


        if (Input.GetMouseButtonDown(0))
        {
            player.StartCharge();
        }

        // Charge ��
        if (Input.GetMouseButton(0))
        {
            player.UpdateCharge();
        }


        if (Input.GetMouseButtonUp(0))
        {
            player.PerformAttack();
            SoundManager soundManager = SoundManager.Instance;
            soundManager.PlaySFX(soundManager.audioClip[9]);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // ��ǥ ���� ����
                Vector3 targetPosition = hit.point;
                targetPosition.y = player.transform.position.y; /// TODO) ���� �÷��̾��� �����忡�� ���� �ٸ� ���Ŀ� ���Ⱑ ��� �տ�
                                                                /// �ִ��� �ľ�

                Vector3 currentDirection = player.transform.forward;
                Vector3 targetDirection = (targetPosition - player.transform.position).normalized;

                // ���� ���
                float angle = Vector3.Angle(currentDirection, targetDirection);

                // ȸ�� ���� ���� (�ð���� �Ǵ� �ݽð����)
                Vector3 cross = Vector3.Cross(currentDirection, targetDirection);
                if (cross.y > 0)  // �ð� ����
                {
                    //Debug.Log(" �ð����");
                    player.animCtrl.SetTrigger("tOutWardAttack");
                    player.atk.Attack();
                    player.atk.curCharging = 0;
                    //player.animCtrl.SetTrigger("tIdle");
                }
                else  // �� �ð���� ȸ��
                {
                    //Debug.Log("�� �ð� ����");
                    player.animCtrl.SetTrigger("tInWardAttack");
                    //player.animCtrl.SetTrigger("tAtk");
                    player.atk.Attack();
                    player.atk.curCharging = 0;
                    //player.animCtrl.SetTrigger("tIdle");
                }



            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            player.PerformAttack();
            SoundManager soundManager = SoundManager.Instance;
            soundManager.PlaySFX(soundManager.audioClip[9]);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // ��ǥ ���� ����
                Vector3 targetPosition = hit.point;
                targetPosition.y = player.transform.position.y; /// TODO) ���� �÷��̾��� �����忡�� ���� �ٸ� ���Ŀ� ���Ⱑ ��� �տ�
                                                                /// �ִ��� �ľ�

                Vector3 currentDirection = player.transform.forward;
                Vector3 targetDirection = (targetPosition - player.transform.position).normalized;

                // ���� ���
                float angle = Vector3.Angle(currentDirection, targetDirection);

                // ȸ�� ���� ���� (�ð���� �Ǵ� �ݽð����)
                Vector3 cross = Vector3.Cross(currentDirection, targetDirection);
                if (cross.y > 0)  // �ð� ����
                {
                    //Debug.Log(" �ð����");
                    player.animCtrl.SetTrigger("tOutWardAttack");
                    player.atk.Attack();
                    player.atk.curCharging = 0;
                    //player.animCtrl.SetTrigger("tIdle");
                }
                else  // �� �ð���� ȸ��
                {
                    //Debug.Log("�� �ð� ����");
                    player.animCtrl.SetTrigger("tInWardAttack");
                    //player.animCtrl.SetTrigger("tAtk");
                    player.atk.Attack();
                    player.atk.curCharging = 0;
                    //player.animCtrl.SetTrigger("tIdle");
                }



            }
        }


        // Ű �Է¿� ���� �̵� ó��
        bool isMoving = player.move.Move(player.stat.walkSpd,player.rewiredPlayer);
        if (!isMoving)
        {
            player.fsm.SetNextState("Player_Idle");
            ResetMovementAnimation();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.fsm.SetNextState("Player_Envasion");
        }

        var temp = player.aim.rayResultPoint;
        temp.y = player.transform.position.y;
        player.transform.LookAt(temp);

        // �̵� ���⿡ ���� �ִϸ��̼� ������Ʈ
        if (isMoving)
        {
            Vector3 relativeDir = Quaternion.Euler(-player.transform.rotation.eulerAngles) * player.move.lastMoveDir;
            player.animCtrl.SetFloat("MoveX", relativeDir.x);
            player.animCtrl.SetFloat("MoveZ", relativeDir.z);
        }
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
    }

    public override void LateUpdateState()
    {
        base.LateUpdateState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    private void ResetMovementAnimation()
    {
        player.animCtrl.SetFloat("MoveX", 0);
        player.animCtrl.SetFloat("MoveZ", 0);
    }
}
