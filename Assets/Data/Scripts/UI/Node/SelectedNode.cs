using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedNode : MonoBehaviour
{
    private Image selectedEffectImage;
    private float speed = 3f;
    public void PlayEffect(bool selected)
    {
        if (selectedEffectImage == null)
        {
            if(TryGetComponent<Image>(out selectedEffectImage) == false)
                return;
        }

        if(selected)
            this.gameObject.SetActive(true);

        StartCoroutine(PlayNodeEffect(selected));

    }
    IEnumerator PlayNodeEffect(bool selected)
    {
        float value = 0f;
        if (selected == false)
            value = 1f;

        selectedEffectImage.fillAmount = value;

        while(true)
        {
            if(selected == false)
            {
                value -= Time.deltaTime * speed;
                selectedEffectImage.fillAmount = value;

                if (selectedEffectImage.fillAmount <= 0f)
                {
                    this.gameObject.SetActive(false);
                    yield break;
                }
            }
            else
            {
                value += Time.deltaTime * speed;
                selectedEffectImage.fillAmount = value;

                if (selectedEffectImage.fillAmount >= 1f)
                    yield break;
            }

            yield return null;
        }

    }
}
