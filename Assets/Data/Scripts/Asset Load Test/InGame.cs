using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class InGame : MonoBehaviour
{
    public Image img;
    public TMP_Text resultText;

    public string targetSpriteKey;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Addressables.LoadAssetAsync<Sprite>(targetSpriteKey).Completed += ((result) =>
        {
            if(result.Status == AsyncOperationStatus.Succeeded)
            {
                img.sprite = result.Result;
                resultText.text = "Load Success";
            }
            else
            {
                resultText.text = "Load Failed";
                img.sprite = null;
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
