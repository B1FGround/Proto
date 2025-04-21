using System.Collections;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public GameObject target;
    private float runSpeed = 20f;

    //private PlayerController playerController;
    public int damage;

    public Vector3 pos_1, pos_2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        //playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    private void Update()
    {
        //if (target == null)
        //{
        //    Destroy(this.gameObject);
        //    return;
        //}
        //
        //Vector3 moveDirection = target.transform.position - transform.position;
        //
        //if (Mathf.Abs(moveDirection.magnitude) < 1f)
        //{
        //    Destroy(this.gameObject);
        //    target.GetComponent<MonsterController>().Damaged(playerController.damage);
        //    return;
        //}
        //
        //moveDirection.Normalize();
        //transform.position += moveDirection * runSpeed * Time.fixedDeltaTime * PlayerController.globalTime;
    }

    private IEnumerator COR_BezierCurves(float duration = 1.0f)
    {
        float time = 0f;

        while (true)
        {
            if (time > 1f)
            {
                time = 0f;
                target.GetComponent<MonsterController>().Damaged(damage);
                Destroy(this.gameObject);
                yield break;
            }

            if (target == null)
            {
                Destroy(this.gameObject);
                yield break;
            }

            Vector3 p4 = Vector3.Lerp(pos_1, pos_2, time);
            Vector3 p5 = Vector3.Lerp(pos_2, target.transform.position, time);
            this.transform.position = Vector3.Lerp(p4, p5, time);

            time += Time.deltaTime / duration;

            yield return null;
        }
    }

    public void StartBezier(float duration)
    {
        StartCoroutine(COR_BezierCurves(duration));
    }
}