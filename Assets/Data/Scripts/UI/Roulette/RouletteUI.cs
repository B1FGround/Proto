using Coffee.UIExtensions;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RouletteUI : UIView
{
    public RectTransform wheel;
    public int totalSections = 8;

    private float[] probabilities = new float[] { 1.5f, 3f, 5f, 7f, 10f, 20f, 25f, 28.5f };
    private int[] sectionIndices = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };

    [SerializeField] Button closeButton;
    [SerializeField] UIParticle[] particle;


    private void Start()
    {
        closeButton.onClick.AddListener(Close);
    }
    public void SpinToIndex(int targetIndex)
    {
        StartCoroutine(Spin(targetIndex));
    }

    IEnumerator Spin(int targetIndex)
    {
        wheel.rotation = Quaternion.identity;
        float anglePerSection = 360f / totalSections;
        int spinCount = Random.Range(5, 8);

        float targetAngle = 360f * spinCount + (anglePerSection * targetIndex);

        float duration = 4f;
        float elapsed = 0f;
        AnimationCurve spinCurve = AnimationCurve.EaseInOut(0, 0, duration, 1);

        float startAngle = wheel.eulerAngles.z;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            float angle = Mathf.Lerp(0f, targetAngle, 1 - Mathf.Pow(1 - t, 3));

            wheel.rotation = Quaternion.Euler(0, 0, startAngle - angle);
            yield return null;
        }

        // 정확히 맞춤
        wheel.rotation = Quaternion.Euler(0, 0, startAngle - targetAngle);
        foreach(var p in particle)
            p.Play();
    }
    public void SpinWithRandomIndex()
    {
        // 확률의 누적합 계산
        foreach (var p in particle)
            p.Stop();
        float totalProbability = 0f;
        foreach (var prob in probabilities)
        {
            totalProbability += prob;
        }

        float randomValue = Random.Range(0f, totalProbability);
        float cumulativeProbability = 0f;

        for (int i = 0; i < probabilities.Length; i++)
        {
            cumulativeProbability += probabilities[i];
            if (randomValue <= cumulativeProbability)
            {
                Debug.Log($"Probabilty {probabilities[i]}, Index {sectionIndices[i]}");
                StartCoroutine(Spin(sectionIndices[i]));
                return;

            }
        }
    }

    public override void Open()
    {
        wheel.rotation = Quaternion.identity;
    }
}
