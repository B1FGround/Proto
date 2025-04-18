using System.Collections;
using System.Resources;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public float minimumDistance = 1f;
    public GameObject player;
    private float runSpeed = 2.5f;
    private float randZ = 0f;

    public int hp = 3;

    private float attackRate = 2f;
    private float curAttackRate = 0f;
    private float attackCount = 5;

    private GameObject leftWall;
    private GameObject rightWall;
    private GameObject upWall;
    private GameObject downWall;

    private bool moveable = true;
    private float chargingDuration = 3f;
    private Vector3 chargePosition;
    private float chargeMoveTime = 1.5f;

    public GameObject arrowRot;
    public GameObject arrowOut;
    public GameObject arrowIn;

    public UnityEngine.Events.UnityEvent OnDead;

    public enum MonsterType
    {
        Normal,
        Elite,
        Boss
    }

    public MonsterType monsterType = MonsterType.Normal;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        randZ = Random.Range(-0.5f, 0.5f);

        if (monsterType == MonsterType.Elite)
        {
            leftWall = GameObject.Find("LeftWall");
            rightWall = GameObject.Find("RightWall");
            upWall = GameObject.Find("UpWall");
            downWall = GameObject.Find("DownWall");
        }
    }

    private void FixedUpdate()
    {
        Move();

        if (monsterType == MonsterType.Elite)
            MiniBossAttack();
        if (monsterType == MonsterType.Boss)
        {
            WaitCharge();
        }
    }

    private void Move()
    {
        if (player == null)
            return;

        if (moveable == false)
            return;

        if (this.transform.position.x < player.transform.position.x)
            this.transform.localScale = new Vector3(1, 1, 1);
        else
            this.transform.localScale = new Vector3(-1, 1, 1);

        Vector3 moveDirection = player.transform.position - transform.position;
        moveDirection = new Vector3(moveDirection.x, moveDirection.y, moveDirection.z + randZ);

        if (Mathf.Abs(moveDirection.magnitude) < minimumDistance)
            return;

        moveDirection.Normalize();
        transform.position += moveDirection * runSpeed * Time.fixedDeltaTime;
    }

    public void Damaged(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            player.GetComponent<PlayerController>().GetExp();

            var pos = transform.position;
            pos.y += 0.5f;
            Instantiate(Resources.Load("DeadEffect"), pos, Quaternion.identity);
            OnDead.Invoke();
            Destroy(this.gameObject);
        }
    }

    private void MiniBossAttack()
    {
        curAttackRate += Time.fixedDeltaTime;

        if (curAttackRate >= attackRate)
        {
            curAttackRate = 0;

            for (int i = 0; i < attackCount; ++i)
            {
                float randX = Random.Range(-5f, 5f);
                float randZ = Random.Range(-5f, 5f);

                Vector3 bulletTargetPos = new Vector3(this.transform.position.x + randX, 0f, this.transform.position.z + randZ);

                bulletTargetPos.x = Mathf.Clamp(bulletTargetPos.x, leftWall.transform.position.x, rightWall.transform.position.x);
                bulletTargetPos.z = Mathf.Clamp(bulletTargetPos.z, downWall.transform.position.z, upWall.transform.position.z);

                GameObject bullet = Instantiate(Resources.Load("MiniBossBullet"), transform.position, Quaternion.identity) as GameObject;
                bullet.GetComponent<MiniBossBullet>().SetPosition(transform.position, bulletTargetPos);
            }
        }
    }

    private void WaitCharge()
    {
        if (moveable == false)
            return;

        curAttackRate += Time.fixedDeltaTime;

        if (curAttackRate >= attackRate)
        {
            StartCoroutine(Charge());

            moveable = false;
        }
    }

    private IEnumerator Charge()
    {
        float time = 0;
        arrowRot.SetActive(true);
        while (true)
        {
            time += Time.deltaTime;
            arrowRot.transform.LookAt(player.transform);
            arrowIn.GetComponent<SpriteRenderer>().material.SetFloat("_Progress", time / chargingDuration);
            chargePosition = player.transform.position;
            if (time >= chargingDuration)
            {
                arrowRot.SetActive(false);
                StartCoroutine(ChargeMove());
                yield break;
            }
            yield return null;
        }
    }

    private IEnumerator ChargeMove()
    {
        float time = 0f;

        while (true)
        {
            time += Time.deltaTime;
            if (time >= chargeMoveTime)
            {
                moveable = true;
                curAttackRate = 0;
                yield break;
            }
            transform.position = Vector3.Lerp(transform.position, chargePosition, time / chargeMoveTime);
            yield return null;
        }
    }
}