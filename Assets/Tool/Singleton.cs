using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    protected static T instance;
    public static T Instance => instance;

    protected virtual void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = (T)this;
        DontDestroyOnLoad((T)this);
    }

    public bool IsInitialized { get { return instance != null; } }

    public virtual void OnDestroy()
    {
        if(instance == this)
        {
            instance = null;    
        }
    }
}
