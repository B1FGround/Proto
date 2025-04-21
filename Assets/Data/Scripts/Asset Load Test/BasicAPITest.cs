using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class BasicAPITest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private IEnumerator Start()
    {
        AsyncOperationHandle<GameObject> loadHandle;

        yield return Addressables.InitializeAsync();

        loadHandle = Addressables.LoadAssetAsync<GameObject>("MyCube");
        yield return loadHandle;

        // 이미 메모리에 로드되있다면 ref count 증가하지 않음
        var instantiateHandle = Addressables.InstantiateAsync("MyCube");
        yield return instantiateHandle;

        var obj = instantiateHandle.Result;

        yield return new WaitForSeconds(2f);

        Addressables.ReleaseInstance(obj);

        Addressables.Release(loadHandle);
    }

    // Update is called once per frame
    private void Update()
    {
    }
}