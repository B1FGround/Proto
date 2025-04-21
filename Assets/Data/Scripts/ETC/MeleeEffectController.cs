using System.Collections;
using UnityEngine;

public class MeleeEffectController : MonoBehaviour
{
    [SerializeField] private GameObject effect;
    private SpriteRenderer spriteRenderer = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        this.transform.localPosition = Vector3.zero;
        spriteRenderer = effect.GetComponent<SpriteRenderer>();
        effect.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float alpha = 1.0f;
        while (alpha > 0.0f)
        {
            alpha -= Time.deltaTime;
            spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, alpha);
            yield return null;
            this.transform.localPosition = Vector3.zero;
        }
        Destroy(gameObject);
    }
}