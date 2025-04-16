using System.Collections;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    public float time = 1;

    // Use this for initialization
    private void Start()
    {
        StartCoroutine(DestroyEffect_Fuc());
    }

    private IEnumerator DestroyEffect_Fuc()
    {
        yield return new WaitForSeconds(time);
        DestroyObject(this.gameObject);
    }
}