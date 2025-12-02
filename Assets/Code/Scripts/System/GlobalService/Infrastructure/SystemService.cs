using System;
using UnityEngine;

public class SystemService : MonoBehaviour
{
    public static SystemService Instance;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    

    /// <summary>
    /// 指定の型がCoreSystem属性を持っているかを調べる
    /// </summary>
    /// <typeparam name="T">任意の型</typeparam>
    /// <returns>CoreSystemを継承たインターフェース以外はfalse</returns>
    private bool CheckIsCoreSystem<T>()
    {
        return System.Attribute.IsDefined(typeof(T), typeof(CoreSystemAttribute));
    }
}