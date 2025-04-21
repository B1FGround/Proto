using System;
using UnityEngine;

public abstract class DodgeTypeBase
{
    protected CharacterController cc;
    protected readonly float dodgeDuration;
    protected readonly float dodgeSpeed;

    protected Vector3 dodgeDirection;
    protected float dodgeTimer = 0;

    public bool isDodging = false;

    protected DodgeTypeBase(CharacterController cc,
                            float dodgeDuration,
                            float dodgeSpeed)
    {
        this.cc = cc;
        this.dodgeDuration = dodgeDuration;
        this.dodgeSpeed = dodgeSpeed;
    }

    public abstract void OnAction();

    public abstract void OnDodge();
}