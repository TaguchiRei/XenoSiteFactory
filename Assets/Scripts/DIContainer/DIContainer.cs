using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DIContainer
{
    public class DiContainer : MonoBehaviour
    {
        [SerializeField] private Scene _managerScene;

        private Dictionary<Type, object> _container;

        public static DiContainer Instance;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            _container = new();
        }

        public void Register<T>(T instance)
        {
            _container.Add(typeof(T), instance);
        }

        public void Unregister<T>(T instance)
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
                Debug.Log($"type {typeof(T)} not found");
                instance = default;
                return false;
            }
        }

        private void Start()
        {
            SceneManager.LoadScene(_managerScene.buildIndex);
        }
    }
}
