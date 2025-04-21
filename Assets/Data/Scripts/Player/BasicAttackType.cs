using System;
using System.Collections.Generic;

using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class BasicAttackType : AttackTypeBase
{
    private GameObject targetMonster = null;
    private Transform bodyBone = null;
    private CharacterAttack CharacterAttack;

    public BasicAttackType(Animator animator,
                           float attackDistance,
                           int damage,
                           float attackSpeed,
                           int additionAttack,
                           Func<int, float, List<GameObject>> MonsterFinder,
                           Transform bodyBone,
                           GameObject weapon,
                           CharacterAttack character)
        : base(animator, attackDistance, damage, attackSpeed, additionAttack, MonsterFinder)
    {
        projectilePrefab = Resources.Load("Prefabs/Object/Sword") as GameObject;
        weapon?.SetActive(false);
        this.bodyBone = bodyBone;
        type = AttackType.Range;
        CharacterAttack = character;
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
        foreach (var target in targets)
            InstantiateProjectile(projectilePrefab, target);
    }

    public void InstantiateProjectile(GameObject prefab, GameObject target)
    {
        if (prefab == null || bodyBone == null)
        {
            Debug.LogError("projectile prefab or bodyBone is null");
            return;
        }
        var swordObj = Object.Instantiate(prefab, bodyBone.transform.position, Quaternion.identity);
        swordObj.GetComponent<Sword>().damage = Damage;
        swordObj.GetComponent<Sword>().target = target;
        swordObj.GetComponent<Sword>().pos_1 = bodyBone.transform.position;

        float randX = bodyBone.transform.position.x + Random.Range(-5f, 5f);
        float randY = bodyBone.transform.position.y + Random.Range(5f, 10f);
        float randZ = bodyBone.transform.position.z + Random.Range(-5f, 5f);

        swordObj.GetComponent<Sword>().pos_2 = new Vector3(randX, randY, randZ);
        swordObj.GetComponent<Sword>().StartBezier(0.5f);

        return;
    }
}