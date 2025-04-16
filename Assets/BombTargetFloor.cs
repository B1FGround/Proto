using System.Collections;
using UnityEngine;

public class BombTargetFloor : MonoBehaviour
{
    private Vector3 maxSize;
    public GameObject innerFloor;

    public void SetSize(float duration, Vector3 position)
    {
        position.y += 0.5f;
        this.transform.position = position;
        maxSize = this.transform.localScale;
        StartCoroutine(SetInnerFloorSize(duration));
    }

    private IEnumerator SetInnerFloorSize(float duration)
    {
        float time = 0;
        while (true)
        {
            if (time >= duration)
            {
                time = 0;
                Destroy(this.gameObject);
                yield break;
            }
            innerFloor.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, time);
            time += Time.deltaTime;
            yield return null;
        }
    }
}