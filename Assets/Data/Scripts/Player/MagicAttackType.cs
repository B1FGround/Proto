using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttackType : AttackTypeBase
{
    private GameObject targetMonster = null;
    private Transform bodyBone = null;
    private ChainLightning chainLightning;

    public MagicAttackType(Animator animator,
                           float attackDistance,
                           int damage,
                           float attackSpeed,
                           int additionAttack,
                           Func<int, float, List<GameObject>> MonsterFinder,
                           Transform bodyBone,
                           GameObject weapon,
                           GameObject chainLightningPrefab)
        : base(animator, attackDistance, damage, attackSpeed, additionAttack, MonsterFinder)
    {
        this.bodyBone = bodyBone;

        // ChainLightning 컴포넌트 찾기
        chainLightning = chainLightningPrefab.GetComponent<ChainLightning>();

        // 공격 타입 설정
        weapon?.SetActive(false);
        type = AttackType.Magic;
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

        if (targets == null || targets.Count == 0)
            return;

        // ChainLightning 효과를 적용
        chainLightning.ApplyChainLightningEffect(bodyBone, targets);
    }
}