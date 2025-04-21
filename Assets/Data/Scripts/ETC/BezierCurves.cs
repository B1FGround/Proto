using System.Collections;
using UnityEngine;

public class BezierCurves : MonoBehaviour
{
    public Transform target;
    public Vector3 pos_1, pos_2, pos_3;
    private float duration = 1f;

    private IEnumerator COR_BezierCurves(float duration = 1.0f)
    {
        float time = 0f;

        while (true)
        {
            if (time > 1f)
            {
                time = 0f;
            }

            Vector3 p4 = Vector3.Lerp(pos_1, pos_2, time);
            Vector3 p5 = Vector3.Lerp(pos_2, pos_3, time);
            target.position = Vector3.Lerp(p4, p5, time);

            time += Time.deltaTime / duration;

            yield return null;
        }
    }

    public void StartBezier()
    {
        StartCoroutine(COR_BezierCurves(duration));
    }
}