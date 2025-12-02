using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// コアなサービスに限定したサービスロケーター
/// ゲーム内で汎用にどこからでも利用される機能をインターフェースの形で登録する
/// </summary>
public class SystemService : MonoBehaviour
{
    public static SystemService Instance;

    private readonly Dictionary<Type, object> _systemServices = new();

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
    /// システムサービスを登録する
    /// </summary>
    /// <param name="instance"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool TryRegister<T>(T instance)
    {
        if (!CheckIsCoreSystem<T>())
        {
            Debug.LogError($"Cannot register a system of type {typeof(T)}");
            return false;
        }

        if (_systemServices.ContainsKey(instance.GetType()))
        {
            Debug.Log("SystemService already registered");
            return false;
        }

        _systemServices.Add(instance.GetType(), instance);
        return true;
    }

    /// <summary>
    /// コンポーネントの登録を試みる
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool TryUnregister<T>()
    {
        if (!CheckIsCoreSystem<T>())
        {
            Debug.LogError($"Cannot unregister a system of type {typeof(T)}");
            return false;
        }

        if (!_systemServices.ContainsKey(typeof(T)))
        {
            Debug.Log("SystemService not registered");
            return false;
        }

        _systemServices.Remove(typeof(T));
        return true;
    }

    /// <summary>
    /// 指定したコアシステムを取得する
    /// </summary>
    /// <param name="instance"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool TryGetSystem<T>(out T instance)
    {
        if (!_systemServices.ContainsKey(typeof(T)))
        {
            Debug.Log($"{nameof(T)} is not registered");
            instance = default;
            return false;
        }

        instance = (T)_systemServices[typeof(T)];
        return true;
    }

    /// <summary>
    /// やむを得ず初期化順が不定の時に取得しなければいけないときに使用する
    /// </summary>
    /// <param name="timeoutSeconds">タイムアウトの時間を秒数で設定</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async UniTask<(bool, T)> TryGetSystemAsync<T>(float timeoutSeconds = 5)
    {
        var type = typeof(T);

        if (_systemServices.TryGetValue(type, out var service))
            return (true, (T)service);


        float time = 0;
        while (time < timeoutSeconds)
        {
            await UniTask.Yield();
            time += Time.deltaTime;

            if (_systemServices.TryGetValue(type, out service))
                return (true, (T)service);
        }

        return (false, default);
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