using System;

using UnityEngine;

public class BasicDodgeType : DodgeTypeBase
{
    private Func<Vector3> GetMoveDirection;

    public BasicDodgeType(CharacterController cc,
                          float dodgeDuration,
                          float dodgeSpeed,
                          Func<Vector3> GetMoveDirection) : base(cc, dodgeDuration, dodgeSpeed)
    {
        this.GetMoveDirection = GetMoveDirection;
    }

    public override void OnAction()
    {
        OnDodge();
    }

    public override void OnDodge()
    {
        if (isDodging)
        {
            DodgeMove();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            dodgeDirection = GetMoveDirection();
            Roll();
        }
    }

    private void Roll()
    {
        var characters = GameObject.FindWithTag("Player").GetComponent<TeamContainer>().GetCharacters();
        foreach (var character in characters)
            character.GetComponent<CharacterAttack>().animator.SetTrigger("Dodge");

        isDodging = true;
        dodgeTimer = dodgeDuration;
    }

    private void DodgeMove()
    {
        if (dodgeTimer > 0)
        {
            cc.Move(dodgeDirection * dodgeSpeed * Time.fixedDeltaTime);
            dodgeTimer -= Time.deltaTime;
        }
    }
}