using System;
using System.Collections.Generic;
using System.Linq;
using GamesKeystoneFramework.KeyDebug.KeyLog;
using Interface;
using UnityEngine;

namespace Service
{
    public class LayeredServiceLocator : MonoBehaviour
    {
        [SerializeField] private string _systemSceneName;
        [SerializeField] private ScriptableObject[] _scriptableObjects;

        private readonly Dictionary<Type, object> _presentationLayers = new();
        private readonly Dictionary<Type, object> _domainLayers = new();
        private readonly Dictionary<Type, object> _dataLayers = new();

        public static LayeredServiceLocator Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            KeyLogger.Log("Initialize Complete", this);
        }

        #region Register系

        /// <summary>
        /// プレゼンテーション層のインスタンスを保存
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        public void RegisterPresentation<T>(T instance) where T : class, IPresentationLayer
        {
            _presentationLayers[typeof(T)] = instance;
        }

        /// <summary>
        /// ドメイン層のインスタンスを保存
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        public void RegisterDomain<T>(T instance) where T : class, IDomainLayer
        {
            _domainLayers[typeof(T)] = instance;
        }

        /// <summary>
        /// データ層のインスタンスを保存
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        public void RegisterData<T>(T instance) where T : class, IDataLayer
        {
            _dataLayers[typeof(T)] = instance;
        }

        #endregion

        #region UnRegister系

        //UnRegister呼び出しで引数に指定したインスタンスが登録されているインスタンスと違う場合はエラーを出す。
        //登録されたインスタンス以外がUnRegisterを呼んでいる場合、そこから取得できる情報と実際の運用で変更された情報が乖離している可能性があり、それが原因のエラーが出る可能性がある。

        /// <summary>
        /// プレゼンテーション層のインスタンス登録を解除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void UnRegisterPresentation<T>(T instance) where T : class, IPresentationLayer
        {
            if (_presentationLayers.TryGetValue(typeof(T), out var registeredInstance) &&
                registeredInstance == instance)
            {
                _presentationLayers.Remove(typeof(T));
            }
            else
            {
                KeyLogger.LogError("登録されたインスタンス以外がUnRegisterを呼んでいます。", typeof(T));
            }
        }

        /// <summary>
        /// ドメイン層のインスタンス登録を解除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void UnRegisterDomain<T>(T instance) where T : class, IDomainLayer
        {
            if (_domainLayers.TryGetValue(typeof(T), out var registeredInstance) && registeredInstance == instance)
            {
                _domainLayers.Remove(typeof(T));
            }
            else
            {
                //インスタンスDispose時にUnRegisterを呼ぶ。
                //そのためUnRegisterするインスタンスと登録されているインスタンスが違う場合、
                KeyLogger.LogError("登録されたインスタンス以外がUnRegisterを呼んでいます。", typeof(T));
            }
        }

        /// <summary>
        /// データ層のインスタンス登録を解除
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        public void UnRegisterData<T>(T instance) where T : class, IDataLayer
        {
            if (_dataLayers.TryGetValue(typeof(T), out var registeredInstance) && registeredInstance == instance)
            {
                _dataLayers.Remove(typeof(T));
            }
            else
            {
                KeyLogger.LogError("登録されたインスタンス以外がUnRegisterを呼んでいます。", typeof(T));
            }
        }

        #endregion

        #region TryGet系

        /// <summary>
        /// プレゼンテーション層のインスタンスを取得
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool TryGetPresentationLayer<T>(out T instance) where T : class, IPresentationLayer
        {
            if (_presentationLayers.TryGetValue(typeof(T), out object result))
            {
                instance = (T)result;
                return true;
            }

            instance = default;
            return false;
        }

        /// <summary>
        /// ドメイン層のインスタンスを取得
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool TryGetDomainLayer<T>(out T instance) where T : class, IDomainLayer
        {
            if (_domainLayers.TryGetValue(typeof(T), out object result))
            {
                instance = (T)result;
                return true;
            }

            instance = default;
            return false;
        }

        /// <summary>
        /// データ層のインスタンスを取得
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool TryGetDataLayer<T>(out T instance) where T : class, IDataLayer
        {
            if (_dataLayers.TryGetValue(typeof(T), out object result))
            {
                instance = (T)result;
                return true;
            }

            instance = default;
            return false;
        }

        #endregion

        #region TryGetAllFunc系

        /// <summary>
        /// 指定したインターフェースを実装しているプレゼンテーション層のクラスをインターフェースとしてすべて取得
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T">インターフェースを指定</typeparam>
        /// <returns></returns>
        public bool TryGetAllFuncPresentationLayer<T>(out List<T> list) where T : IPresentationLayer
        {
            if (!typeof(T).IsInterface)
            {
                KeyLogger.Log("TryGetAllFuncPresentationLayer can use interfaceOnly");
                list = null;
                return false;
            }

            List<T> result = new List<T>();
            foreach (var kvp in _presentationLayers)
            {
                var type = kvp.Key;
                if (kvp.Value is T obj)
                {
                    result.Add(obj);
                }
            }

            if (result.Any())
            {
                list = result;
                return true;
            }
            else
            {
                list = null;
                return false;
            }
        }

        /// <summary>
        /// 指定したインターフェースを実装しているドメイン層のクラスをすべてインターフェースとして取得
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T">インターフェースを指定</typeparam>
        /// <returns></returns>
        public bool TryGetAllFuncDomainLayer<T>(out List<T> list) where T : IDomainLayer
        {
            if (!typeof(T).IsInterface)
            {
                KeyLogger.Log("TryGetAllFuncDomainLayer can use interfaceOnly");
                list = null;
                return false;
            }

            List<T> result = new List<T>();
            foreach (var kvp in _domainLayers)
            {
                var type = kvp.Key;
                if (kvp.Value is T obj)
                {
                    result.Add(obj);
                }
            }

            if (result.Any())
            {
                list = result;
                return true;
            }
            else
            {
                list = null;
                return false;
            }
        }

        /// <summary>
        /// 指定した型のインターフェースを実装しているクラスをすべてインターフェースとして取得
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T">インターフェースを指定</typeparam>
        /// <returns></returns>
        public bool TryGetAllFuncDataLayer<T>(out List<T> list) where T : IDataLayer
        {
            if (!typeof(T).IsInterface)
            {
                KeyLogger.Log("TryGetAllFuncDataLayer can use interfaceOnly");
                list = null;
                return false;
            }

            List<T> result = new List<T>();
            foreach (var kvp in _dataLayers)
            {
                var type = kvp.Key;
                if (kvp.Value is T obj)
                {
                    result.Add(obj);
                }
            }

            if (result.Any())
            {
                list = result;
                return true;
            }
            else
            {
                list = null;
                return false;
            }
        }

        #endregion

        #region ScriptableObject系

        /// <summary>
        /// スクリプタブルオブジェクトを取得する。
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool TryGetScriptableObject<T>(out T instance) where T : ScriptableObject
        {
            foreach (var scriptableObject in _scriptableObjects)
            {
                T obj = scriptableObject as T;
                if (obj != null)
                {
                    instance = obj;
                    return true;
                }
            }

            instance = null;
            return false;
        }

        /// <summary>
        /// 同一の型のスクリプタブルオブジェクトをすべて取得する
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool TryGetAllScriptableObjectsOfType<T>(out List<T> list) where T : ScriptableObject
        {
            List<T> scriptableObjectList = new();
            foreach (var scriptableObject in _scriptableObjects)
            {
                T obj = scriptableObject as T;
                if (obj != null)
                {
                    scriptableObjectList.Add(obj);
                }
            }

            list = scriptableObjectList;
            if (scriptableObjectList.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}