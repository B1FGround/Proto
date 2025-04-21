using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackTypeBase
{
    public enum AttackType
    {
        Magic,
        Melee,
        Range
    }

    public AttackType type;
    protected Animator animator;
    protected GameObject projectilePrefab = null;
    protected Func<int, float, List<GameObject>> MonsterFinder;
    protected float attackDistance;
    private int damage;
    private float attackSpeed;
    private int additionalAttack;

    public int Damage
    { get { return damage; } set { damage = value; } }

    public float AttackSpeed
    { get { return attackSpeed; } set { attackSpeed = value; animator.SetFloat("AttackSpeed", attackSpeed); } }

    public int AdditionalAttack
    { get { return additionalAttack; } set { additionalAttack = value; } }

    protected AttackTypeBase(Animator animator,
                             float attackDistance,
                             int damage,
                             float attackSpeed,
                             int additionAttack,
                             Func<int, float, List<GameObject>> MonsterFinder)
    {
        this.animator = animator;
        this.attackDistance = attackDistance;
        this.Damage = damage;
        this.AttackSpeed = attackSpeed;
        this.AdditionalAttack = additionAttack;
        this.MonsterFinder = MonsterFinder;
    }

    public abstract void OnAction();

    public abstract void ThrowProjectile();
}