using System;
using System.Threading;
using UnityEngine;

public abstract class Singleton<T> where T : new()
{
    private static T _instance;
    private static readonly object _lock = new object();

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                var objLock = _lock;
                Monitor.Enter(objLock);
                try
                {
                    if (_instance == null)
                    {
                        _instance = Activator.CreateInstance<T>();
                    }
                }
                finally
                {
                    Monitor.Exit(objLock);
                }
            }

            return _instance;
        }
    }

    public virtual void Init()
    {
    }

}

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance;
    private static object _lock = new object();

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                var objLock = _lock;

                Monitor.Enter(objLock);
                try
                {
                    _instance = FindObjectOfType<T>() ?? new GameObject(typeof (T).Name, typeof (T)).GetComponent<T>();
                }
                finally
                {
                    Monitor.Exit(objLock);
                }
                
            }
            return _instance;
        }

    }

    public virtual void SetDontDestroyOnLoad(Transform parent)
    {
        transform.SetParent(parent);
    }
}