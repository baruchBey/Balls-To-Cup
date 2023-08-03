using UnityEngine;

[DisallowMultipleComponent,RequireComponent(typeof(PrettyHierarchy.PrettyObject)),DefaultExecutionOrder(-11)]
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    private static T instance;

    /// <summary>
    /// 
    /// </summary>
    public static T Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<T>();
                DontDestroyOnLoad(instance);
            }

            return instance;
        }
        private set
        {
            if (instance)
            {
                return;
            }

            instance = value;
            DontDestroyOnLoad(instance);
        }
    }
   
    /// <summary>
    /// 
    /// </summary>
    public virtual void Awake()
    {
        Instance = this as T;
    }
    private void Reset()
    {
        name = typeof(T).ToString().Split('.')[^1];
        
    }
}