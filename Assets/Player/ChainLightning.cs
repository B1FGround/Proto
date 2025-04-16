using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChainLightning : MonoBehaviour
{
    public GameObject lightningEffectPrefab;

    //private PlayerController player;
    private CharacterAttack character;

    private void Start()
    {
        character = this.gameObject.transform.parent.gameObject.GetComponent<CharacterAttack>();
    }

    public void ApplyChainLightningEffect(Transform startPoint, List<GameObject> targets)
    {
        StartCoroutine(ExecuteChainLightning(startPoint, targets));
    }

    private IEnumerator ExecuteChainLightning(Transform startPoint, List<GameObject> targets)
    {
        Transform currentTarget = startPoint;
        if (currentTarget == null)
            yield break;

        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] != null)
            {
                // 몬스터에게 데미지 적용
                var monsterController = targets[i].GetComponent<MonsterController>();
                Vector3 targetPos = targets[i].transform.position;
                if (monsterController != null)
                {
                    monsterController.Damaged(character.AttackType.Damage);
                }

                // 전기 이펙트 생성
                if (currentTarget)
                    InstantiateLightningEffect(currentTarget.position, targetPos, currentTarget.gameObject, targets[i]);
                else
                    yield break;

                // 현재 타겟을 업데이트 (파괴된 타겟은 넘어가도록 처리)
                if (targets[i] != null && targets[i].transform != null)
                    currentTarget = targets[i].transform;
                else
                    break; // 타겟이 파괴된 경우 더 이상 연결하지 않음
            }

            // 타겟 간 간격을 두고 진행 (0.2초의 지연)
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void InstantiateLightningEffect(Vector3 start, Vector3 end, GameObject startObject, GameObject endObject)
    {
        GameObject lightningEffect = Instantiate(lightningEffectPrefab, Vector3.zero, Quaternion.identity);
        lightningEffect.GetComponent<Lightning>().start = startObject;
        lightningEffect.GetComponent<Lightning>().end = endObject;
        LineRenderer lineRenderer = lightningEffect.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        // 전기선 위치 설정
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        Destroy(lightningEffect, 0.25f); // 이펙트가 1초 후에 삭제되도록 설정
    }
}