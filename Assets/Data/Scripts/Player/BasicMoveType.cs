using System;
using System.Collections.Generic;
using UnityEngine;
using Transform = UnityEngine.Transform;

public class BasicMoveType : MoveTypeBase
{
    private Func<bool> GetIsDodging;

    public BasicMoveType(CharacterController cc,
                         Transform transform,
                         float runSpeed,
                         float detectDistance,
                         Func<bool> GetIsDodging,
                         Func<int, float, List<GameObject>> monsterFinder)
        : base(cc, transform, detectDistance, runSpeed, monsterFinder)
    {
        this.GetIsDodging = GetIsDodging;
    }

    public override void OnAction()
    {
        if (auto)
            AutoMoveToTarget();
        else
            OnMove();
    }

    public override void OnMove()
    {
        if (GetIsDodging())
            return;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        bool isMoving = moveX != 0 || moveZ != 0;

        if (isMoving)
        {
            moveDirection = new Vector3(moveX, 0, moveZ).normalized;
            cc.Move(moveDirection * runSpeed * Time.fixedDeltaTime);

            if (moveX < 0f)
                transform.localScale = new Vector3(1, 1, 1);
            else
                transform.localScale = new Vector3(-1, 1, 1);
        }

        var characters = GameObject.FindWithTag("Player").GetComponent<TeamContainer>().GetCharacters();
        foreach (var character in characters)
        {
            character.GetComponent<Animator>().SetBool("Run", isMoving);
            character.GetComponent<Animator>().SetBool("RunFinish", !isMoving);
        }
        //animator.SetBool("Run", isMoving);
        //animator.SetBool("RunFinish", !isMoving);
    }
}