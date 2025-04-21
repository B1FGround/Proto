using System.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class EffectController : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;
    void Start()
    {
        StartCoroutine(DeadEffectEnd());
    }

    IEnumerator DeadEffectEnd()
    {
        while (true)
        {
            if (particle.isPaused || particle.isStopped)
            {
                Destroy(gameObject);
                yield break;
            }
            yield return null;
        }
    }
}
