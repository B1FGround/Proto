using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class MiniBossBullet : MonoBehaviour
{
    public Vector3 targetPos;
    public Vector3 startPos;
    private Vector3 middlePos;

    float duration = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetPosition(Vector3 start, Vector3 target)
    {
        float posX = Random.Range(start.x, target.x);
        float posY = Random.Range(7f, 12f);
        float posZ = Random.Range(start.z, target.z);

        Vector3 middle = new Vector3(posX, posY, posZ);
        StartCoroutine(COR_BezierCurves(start, middle, target, duration));
    }
    IEnumerator COR_BezierCurves(Vector3 start, Vector3 middle, Vector3 target, float duration = 1.0f)
    {
        float time = 0f;


        var targetObj = Instantiate(Resources.Load("Prefabs/Effect/BombTarget")) as GameObject;
        targetObj.GetComponent<BombTargetFloor>().SetSize(duration, target);
        while (true)
        {
            if (time >= duration)
            {
                time = 0f;
                Instantiate(Resources.Load("Prefabs/Effect/WaveEffect"), this.transform.position, Quaternion.identity);
                Destroy(this.gameObject);
                yield break;
            }

            Vector3 p4 = Vector3.Lerp(start, middle, time);
            Vector3 p5 = Vector3.Lerp(middle, target, time);
            this.transform.position = Vector3.Lerp(p4, p5, time);

            time += Time.deltaTime;

            yield return null;
        }
    }
}
