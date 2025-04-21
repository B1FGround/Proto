using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (!Application.isPlaying && instance == null)
            {
                instance = (T)FindFirstObjectByType(typeof(T));
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(T).Name, typeof(T));
                    instance = singletonObject.GetComponent<T>();
                }
            }
            return instance;
        }
    }

    [SerializeField] protected bool dontDestroyOnLoad = true;

    protected virtual void Awake()
    {
        if (instance == null)
            instance = this as T;

        if (dontDestroyOnLoad)
        {
            if (transform.parent != null && transform.root != null)
                DontDestroyOnLoad(transform.root.gameObject);
            else
                DontDestroyOnLoad(gameObject);
        }
    }
}