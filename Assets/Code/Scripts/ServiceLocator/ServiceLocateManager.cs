using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GamesKeystoneFramework.KeyDebug.KeyLog;
using Interface;
using UnityEngine;

namespace ServiceManagement
{
    /// <summary>
    /// サービスの管理、登録を担当する。
    /// </summary>
    public class ServiceLocateManager : MonoBehaviour
    {
        [SerializeField] private int _timeOutTime;
        [SerializeField] private ScriptableObject[] _scriptableObjects;

        private readonly Dictionary<Type, object> _presentationLayers = new();
        private readonly Dictionary<Type, object> _domainLayers = new();
        private readonly Dictionary<Type, object> _applicationLayers = new();
        private readonly Dictionary<Type, object> _dataLayers = new();
        private readonly Dictionary<Type, object> _funcManagementInterfaces = new();

        public static ServiceLocateManager Instance { get; private set; }

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
            KeyLogger.Log($"プレゼンテーション層[{typeof(T).Name}] がサービスロケーターに登録されました。", this);
            _presentationLayers[typeof(T)] = instance;
        }

        /// <summary>
        /// ドメイン層のインスタンスを保存
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        public void RegisterDomain<T>(T instance) where T : class, IDomainLayer
        {
            KeyLogger.Log($"ドメイン層[{typeof(T).Name}] がサービスロケーターに登録されました。", this);
            _domainLayers[typeof(T)] = instance;
        }

        /// <summary>
        /// インフラ層のインスタンスを保存
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        public void RegisterApplication<T>(T instance) where T : class, IApplicationLayer
        {
            KeyLogger.Log($"インフラ層[{typeof(T).Name}] がサービスロケーターに登録されました。", this);
            _applicationLayers[typeof(T)] = instance;
        }

        /// <summary>
        /// データ層のインスタンスを保存
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        public void RegisterData<T>(T instance) where T : class, IDataLayer
        {
            KeyLogger.Log($"データ層[{typeof(T).Name}] がサービスロケーターに登録されました。", this);
            _dataLayers[typeof(T)] = instance;
        }

        /// <summary>
        /// 機能の管理機能を登録する
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="TInterface"></typeparam>
        public void RegisterManagementFunc<TInterface>(IManagementFunc<TInterface> instance)
        {
            if (!typeof(TInterface).IsInterface) throw new Exception("TInterface must be an interface");
            KeyLogger.Log($"管理システム[{typeof(TInterface).Name}] がサービスロケーターに登録されました。", this);
            _funcManagementInterfaces[typeof(TInterface)] = instance;
        }

        /// <summary>
        /// 機能を登録する。必ず語引数を指定すること
        /// </summary>
        public void RegisterFunc<TInterface>(object instance)
        {
            if (!typeof(TInterface).IsInterface)
                throw new Exception("TInterface must be an interface");

            // インスタンスの型が TInterface から派生しているか（実装しているか）をチェック
            if (!typeof(TInterface).IsAssignableFrom(instance.GetType()))
                throw new Exception($"instance of type {instance.GetType().Name} does not implement {typeof(TInterface).Name}");

            // TInterface にキャスト
            var typedInstance = (TInterface)instance;

            if (_funcManagementInterfaces.TryGetValue(typeof(TInterface), out object funcManager) &&
                funcManager is IManagementFunc<TInterface> manager)
            {
                manager.RegisterFunc(typedInstance);
                KeyLogger.Log($"機能[{instance.GetType().Name}] が登録されました。", this);
            }
            else
            {
                KeyLogger.LogError($"機能[{typeof(TInterface).Name}] を管理するクラスは登録されていません。", this);
            }
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
                KeyLogger.LogError("登録されたインスタンス以外がUnRegisterを呼んでいます。", this);
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
                KeyLogger.LogError("登録されたインスタンス以外がUnRegisterを呼んでいます。", this);
            }
        }

        public void UnRegisterApplication<T>(T instance) where T : class, IApplicationLayer
        {
            if (_applicationLayers.TryGetValue(typeof(T), out var registeredInstance) &&
                registeredInstance == instance)
            {
                _applicationLayers.Remove(typeof(T));
            }
            else
            {
                KeyLogger.LogError("登録されたインスタンス以外がUnRegisterを呼んでいます。", this);
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
                KeyLogger.LogError("登録されたインスタンス以外がUnRegisterを呼んでいます。", this);
            }
        }

        /// <summary>
        /// 機能の管理機能の登録を解除する
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="TInterface"></typeparam>
        public void UnRegisterManagementFunc<TInterface>(IManagementFunc<TInterface> instance)
        {
            if (!typeof(TInterface).IsInterface) throw new Exception("TInterface must be an interface");
            if (_funcManagementInterfaces.TryGetValue(typeof(TInterface), out var func))
            {
                if (ReferenceEquals(func, instance))
                {
                    _funcManagementInterfaces.Remove(typeof(TInterface));
                    KeyLogger.Log($"管理システム[{typeof(TInterface).Name}] がサービスロケーターから解除されました。", this);
                }
                else
                {
                    KeyLogger.Log($"機能[IManagementFunc<{typeof(TInterface).Name}>] を登録時と違うインスタンスから登録解除しようとしています。",
                        this);
                }
            }
            else
            {
                KeyLogger.LogError($"機能[{typeof(TInterface)}]を管理するクラスはまだ登録されていないか、すでに登録が解除されています", this);
            }
        }

        /// <summary>
        /// 機能の登録を解除する
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="TInterface"></typeparam>
        public void UnRegisterFunc<TInterface>(object instance)
        {
            if (!typeof(TInterface).IsInterface) throw new Exception("TInterface はインターフェースでなければいけません");

            if (!typeof(TInterface).IsAssignableFrom(instance.GetType()))
                throw new Exception($"instance of type {instance.GetType().Name} does not implement {typeof(TInterface).Name}");

            var typedInstance = (TInterface)instance;

            if (_funcManagementInterfaces.TryGetValue(typeof(TInterface), out var managerFunc) &&
                managerFunc is IManagementFunc<TInterface> manager)
            {
                manager.UnregisterFunc(typedInstance);
                KeyLogger.Log($"機能[{instance.GetType().Name}] の登録を解除しました。", this);
            }
            else
            {
                KeyLogger.LogError($"機能[{instance.GetType().Name}] を管理するクラスは登録されていません。", this);
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

            instance = null;
            return false;
        }

        /// <summary>
        /// プレゼンテーション層のインスタンスを非同期で取得
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async UniTask<(bool, T)> TryGetPresentationLayerAsync<T>() where T : class, IPresentationLayer
        {
            try
            {
                await UniTask.WaitUntil(() => _presentationLayers.ContainsKey(typeof(T)))
                    .Timeout(TimeSpan.FromSeconds(_timeOutTime));
                return (true, _presentationLayers[typeof(T)] as T);
            }
            catch (TimeoutException)
            {
                KeyLogger.LogError($"[{typeof(T).Name}]はタイムアウトにより取得できませんでした。", this);
                return (false, null);
            }
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

            instance = null;
            return false;
        }

        /// <summary>
        /// ドメイン層のインスタンスを非同期で取得
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async UniTask<(bool, T)> TryGetDomainLayerAsync<T>() where T : class, IDomainLayer
        {
            try
            {
                await UniTask.WaitUntil(() => _domainLayers.ContainsKey(typeof(T)))
                    .Timeout(TimeSpan.FromSeconds(_timeOutTime));
                return (true, _domainLayers[typeof(T)] as T);
            }
            catch (TimeoutException)
            {
                KeyLogger.LogError($"[{typeof(T).Name}]はタイムアウトにより取得できませんでした。", this);
                return (false, null);
            }
        }

        /// <summary>
        /// アプリケーション層のインスタンスを取得
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool TryGetApplicationLayer<T>(out T instance) where T : class, IApplicationLayer
        {
            if (_applicationLayers.TryGetValue(typeof(T), out object result))
            {
                instance = (T)result;
                return true;
            }

            instance = null;
            return false;
        }

        /// <summary>
        /// アプリケーション層のインスタンスを非同期で取得
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async UniTask<(bool, T)> TryGetApplicationLayerAsync<T>() where T : class, IApplicationLayer
        {
            try
            {
                await UniTask.WaitUntil(() => _applicationLayers.ContainsKey(typeof(T))).Timeout(TimeSpan.FromSeconds(_timeOutTime));
                return (true, _applicationLayers[typeof(T)] as T);
            }
            catch (TimeoutException)
            {
                KeyLogger.LogError($"[{typeof(T).Name}]はタイムアウトにより取得できませんでした。", this);
                return (false, null);
            }
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

            instance = null;
            return false;
        }

        public async UniTask<(bool, T)> TryGetDataLayerAsync<T>() where T : class, IDataLayer
        {
            try
            {
                await UniTask.WaitUntil(() => _dataLayers.ContainsKey(typeof(T))).Timeout(TimeSpan.FromSeconds(_timeOutTime));
                return (true, _dataLayers[typeof(T)] as T);
            }
            catch (TimeoutException)
            {
                KeyLogger.LogError($"[{typeof(T).Name}]はタイムアウトにより取得できませんでした。", this);
                return (false, null);
            }
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
                KeyLogger.Log("TryGetAllFuncPresentationLayer can use interfaceOnly", this);
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
                KeyLogger.Log("TryGetAllFuncDomainLayer can use interfaceOnly", this);
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
                KeyLogger.Log("TryGetAllFuncDataLayer can use interfaceOnly", this);
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