using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveTypeBase
{
    protected CharacterController cc;
    protected Transform transform;

    public Vector3 moveDirection = Vector3.zero;
    protected readonly float runSpeed;
    protected readonly float detectDistance;

    // 자동이동
    public bool auto = false;

    public GameObject moveTarget = null;
    private bool isAvoidingObstacle = false;
    private float avoidTime = 0f;
    private float maxAvoidTime = 0.5f; // 벽을 피하는 최대 시간

    private enum MoveState
    { Stop, Forward, Backward };

    private MoveState moveState = MoveState.Stop;
    private Func<int, float, List<GameObject>> MonsterFinder;

    private List<GameObject> characters;

    protected MoveTypeBase(CharacterController cc,
                           Transform transform,
                           float runSpeed,
                           float detectDistance,
                           Func<int, float, List<GameObject>> monsterFinder)
    {
        this.cc = cc;
        this.transform = transform;
        this.runSpeed = runSpeed;
        this.detectDistance = detectDistance;
        this.MonsterFinder = monsterFinder;
        characters = GameObject.FindWithTag("Player").GetComponent<TeamContainer>().GetCharacters();
    }

    public abstract void OnAction();

    public abstract void OnMove();

    #region AutoMove

    protected void AutoMoveToTarget()
    {
        SetMoveTarget();

        if (moveTarget != null && isAvoidingObstacle == false)
        {
            moveDirection = moveTarget.transform.position - transform.position;
            moveDirection.y = 0f;

            float distanceToTarget = moveDirection.magnitude;

            if (distanceToTarget >= 8f)
                moveState = MoveState.Forward;
            else if (distanceToTarget <= 3f)
                moveState = MoveState.Backward;
            else if (distanceToTarget > 5 && distanceToTarget < 6)
            {
                moveState = MoveState.Stop;
                moveTarget = null;
                SetMoveTarget();

                MoveAnimation(false);
            }
        }
        MoveBackFromTarget();
    }

    private void MoveBackFromTarget()
    {
        if (isAvoidingObstacle)
        {
            MoveAnimation(true);
            if (moveDirection.normalized.x > 0)
                transform.localScale = new Vector3(-1, 1, 1);
            else if (moveDirection.normalized.x < 0)
                transform.localScale = new Vector3(1, 1, 1);
            cc.Move(moveDirection.normalized * runSpeed * Time.fixedDeltaTime);
            avoidTime -= Time.deltaTime;

            if (avoidTime <= 0)
            {
                isAvoidingObstacle = false;
                SetMoveTarget();
            }

            return;
        }

        if (moveTarget != null && moveState != MoveState.Stop)
        {
            moveDirection = moveTarget.transform.position - transform.position;

            moveDirection.y = 0f;
            Vector3 dir = moveDirection.normalized;

            if (moveState == MoveState.Backward)
                dir *= -1f;

            if (dir.x > 0)
                transform.localScale = new Vector3(-1, 1, 1);
            else if (dir.x < 0)
                transform.localScale = new Vector3(1, 1, 1);

            if (Physics.Raycast(transform.position, dir, out RaycastHit hit, 1f))
            {
                if (hit.collider.CompareTag("Wall"))
                {
                    moveTarget = null;
                    // 반사 방향 계산
                    moveDirection = Vector3.Reflect(dir, hit.normal);
                    isAvoidingObstacle = true;
                    avoidTime = maxAvoidTime;
                    return;
                }
            }
            cc.Move(dir * runSpeed * Time.fixedDeltaTime);
            MoveAnimation(true);
        }
    }

    private void SetMoveTarget()
    {
        if (moveTarget == null)
        {
            var target = MonsterFinder(1, detectDistance);
            if (target != null)
                moveTarget = target.Count > 0 ? target[0] : null;
        }
    }

    private void MoveAnimation(bool isMoving)
    {
        if (characters[0] == null)
            characters = GameObject.FindWithTag("Player").GetComponent<TeamContainer>().GetCharacters();

        foreach (var character in characters)
        {
            character?.GetComponent<Animator>().SetBool("Run", isMoving);
            character?.GetComponent<Animator>().SetBool("RunFinish", !isMoving);
        }
    }

    #endregion AutoMove
}