using System;
using System.Collections.Generic;

using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class MeleeAttackType : AttackTypeBase
{
    private GameObject targetMonster = null;
    private Transform bodyBone = null;

    public MeleeAttackType(Animator animator,
                           float attackDistance,
                           int damage,
                           float attackSpeed,
                           int additionAttack,
                           Func<int, float, List<GameObject>> MonsterFinder,
                           Transform bodyBone,
                           GameObject weapon)
        : base(animator, attackDistance, damage, attackSpeed, additionAttack, MonsterFinder)
    {
        this.bodyBone = bodyBone;
        weapon?.SetActive(true);
        type = AttackType.Melee;
    }

    public override void OnAction()
    {
        Attack();
    }

    private void Attack()
    {
        // 가장 가까운 몬스터 하나 찾기
        List<GameObject> closestMonsters = MonsterFinder(1, attackDistance);
        if (closestMonsters == null)
            return;

        targetMonster = closestMonsters.Count > 0 ? closestMonsters[0] : null;

        if (targetMonster != null)
            animator.SetBool("Attack", true);
        else
            animator.SetBool("Attack", false);
    }

    public override void ThrowProjectile()
    {
        // 공격 대상 찾기 (기본 1개 + additionalAttack 개수 만큼)
        List<GameObject> targets = MonsterFinder(1 + AdditionalAttack, attackDistance);
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] != null)
            {
                targets[i].GetComponent<MonsterController>().Damaged(Damage);
                var effect = Object.Instantiate(Resources.Load("Prefabs/Effect/MeleeEffect"), Vector3.zero, Quaternion.identity, targets[i].transform) as GameObject;
            }
        }
    }
}