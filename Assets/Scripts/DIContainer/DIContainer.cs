using System;
using System.Collections.Generic;
using GamesKeystoneFramework.KeyDebug.KeyLog;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DIContainer
{
    public class DiContainer : MonoBehaviour
    {
        [SerializeField] private string managerSceneName;

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

        public bool TryGet<T>(out T instance)
        {
            if (_container.ContainsKey(typeof(T)))
            {
                instance = (T)_container[typeof(T)];
                return true;
            }
            else
            {
                KeyLogger.Log($"{typeof(T).Name} is not found");
                instance = default;
                return false;
            }
        }
    }
}