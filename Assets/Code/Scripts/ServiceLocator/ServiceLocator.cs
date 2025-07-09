using System;
using System.Collections.Generic;
using GamesKeystoneFramework.KeyDebug.KeyLog;
using Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Service
{
    public class ServiceLocator : MonoBehaviour
    {
        [SerializeField] private string managerSceneName;

        [SerializeField] private ScriptableObject[] _scriptableObjects;

        private Dictionary<Type, object> _container;

        public static ServiceLocator Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                _container = new();
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            KeyLogger.Log("Initialize Complete", this);
        }

        private void Start()
        {
            if (!string.IsNullOrEmpty(managerSceneName)) SceneManager.LoadScene(managerSceneName, LoadSceneMode.Single);
        }

        public void Register(Type type, object instance)
        {
            KeyLogger.Log($"Registered Instance {type.Name}");
            _container[type] = instance;
        }

        public void Unregister<T>()
        {
            _container.Remove(typeof(T));
        }

        public bool TryGetClass<T>(out T instance) where T : class
        {
            if (_container.ContainsKey(typeof(T)))
            {
                instance = (T)_container[typeof(T)];
                return true;
            }

            KeyLogger.Log($"{typeof(T).Name} is not found");
            instance = null;
            return false;
        }

        public bool TryGetAllManagerClass(out List<(Type, object)> instances)
        {
            List<(Type, object)> result = new();
            foreach (var kvp in _container)
            {
                if (IsDerivedFromManagerBase(kvp.Key))
                {
                    result.Add((kvp.Key, kvp.Value));
                }
            }

            if (result.Count > 0)
            {
                instances = result;
                return true;
            }
            else
            {
                instances = null;
                return false;
            }
        }

        bool IsDerivedFromManagerBase(Type targetType)
        {
            Type genericBase = typeof(ManagerBase<>);

            while (targetType != null && targetType != typeof(object))
            {
                if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == genericBase)
                {
                    return true;
                }

                targetType = targetType.BaseType;
            }

            return false;
        }

        public bool TryGetScriptableObject<T>(out T scriptableObjects) where T : class
        {
            foreach (var scriptableObject in _scriptableObjects)
            {
                if (scriptableObject is T obj)
                {
                    scriptableObjects = obj;
                    return true;
                }
            }

            scriptableObjects = null;
            return false;
        }
    }
}