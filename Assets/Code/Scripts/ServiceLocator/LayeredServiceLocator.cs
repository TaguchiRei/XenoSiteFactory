using System;
using System.Collections.Generic;
using GamesKeystoneFramework.KeyDebug.KeyLog;
using Interface;
using UnityEngine;

namespace Service
{
    public class LayeredServiceLocator : MonoBehaviour
    {
        [SerializeField] private string _managerSceneName;
        [SerializeField] private ScriptableObject[] _scriptableObjects;

        private readonly Dictionary<Type, object> _presentationLayers = new();
        private readonly Dictionary<Type, object> _domainLayers = new();
        private readonly Dictionary<Type, object> _dataLayers = new();

        public static LayeredServiceLocator Instance;

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

        /// <summary>
        /// プレゼンテーション層のインスタンスを保存
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        public void RegisterPresentation<T>(T instance) where T : IPresentationLayer
        {
            _presentationLayers[typeof(T)] = instance;
        }

        /// <summary>
        /// ドメイン層のインスタンスを保存
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        public void RegisterDomain<T>(T instance) where T : IDomainLayer
        {
            _domainLayers[typeof(T)] = instance;
        }

        /// <summary>
        /// データ層のインスタンスを保存
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        public void RegisterData<T>(T instance) where T : IDataLayer
        {
            _dataLayers[typeof(T)] = instance;
        }

        /// <summary>
        /// プレゼンテーション層のインスタンス登録を解除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void UnRegisterPresentation<T>(T instance) where T : IPresentationLayer
        {
            _presentationLayers.Remove(typeof(T));
        }

        /// <summary>
        /// ドメイン層のインスタンス登録を解除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void UnRegisterDomain<T>(T instance) where T : IDomainLayer
        {
            _domainLayers.Remove(typeof(T));
        }

        public void UnRegisterData<T>(T instance) where T : IDataLayer
        {
            _dataLayers.Remove(typeof(T));
        }

        /// <summary>
        /// プレゼンテーション層のインスタンスを取得
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool TryGetPresentationLayer<T>(out T instance)
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
        public bool TryGetDomainLayer<T>(out T instance)
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
        public bool TryGetDataLayer<T>(out T instance)
        {
            if (_dataLayers.TryGetValue(typeof(T), out object result))
            {
                instance = (T)result;
                return true;
            }

            instance = default;
            return false;
        }

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
    }
}