using System;
using System.Collections.Generic;
using GamesKeystoneFramework.KeyDebug.KeyLog;
using UnityEngine;
using UnityEngine.SceneManagement;
using XenoScriptableObject;

namespace DIContainer
{
    public class DiContainer : MonoBehaviour
    {
        [SerializeField] private string managerSceneName;
        
        [SerializeField] private ScriptableObject[] _scriptableObjects;

        private Dictionary<Type, object> _container;

        public static DiContainer Instance;

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
            KeyLogger.Log("Initialize Complete",this);
        }

        private void Start()
        {
            if(!string.IsNullOrEmpty(managerSceneName)) SceneManager.LoadScene(managerSceneName, LoadSceneMode.Single);
        }

        public void Register<T>(T instance)
        {
            Debug.Log($"Registered Instance {typeof(T).Name}");
            _container[typeof(T)] = instance;
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