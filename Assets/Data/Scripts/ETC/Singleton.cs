using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    private static bool applicationIsQuitting = false;

    public static T Instance
    {
        get
        {
            if (applicationIsQuitting || !Application.isPlaying)
                return null;

            if (instance == null)
            {
#if UNITY_2023_1_OR_NEWER
                instance = FindFirstObjectByType<T>();
#else
                instance = FindObjectOfType<T>();
#endif
                if (instance == null)
                {
                    GameObject singletonGO = new GameObject(typeof(T).Name);
                    instance = singletonGO.AddComponent<T>();
                }
            }

            return instance;
        }
    }

    [SerializeField] protected bool dontDestroyOnLoad = true;

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;

            if (dontDestroyOnLoad)
            {
                if (transform.parent != null && transform.root != null)
                    DontDestroyOnLoad(transform.root.gameObject);
                else
                    DontDestroyOnLoad(gameObject);
            }
        }
        else if (instance != this)
        {
            Debug.LogWarning($"[Singleton] 중복 인스턴스 제거됨: {gameObject.name}");
            Destroy(gameObject);
        }
    }

    protected virtual void OnApplicationQuit()
    {
        applicationIsQuitting = true;
    }
}
