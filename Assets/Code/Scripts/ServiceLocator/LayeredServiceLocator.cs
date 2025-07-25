using System;
using System.Collections.Generic;
using GamesKeystoneFramework.KeyDebug.KeyLog;
using Interface;
using UnityEngine;

namespace Service
{
    public class LayeredServiceLocator : MonoBehaviour
    {
        [SerializeField] private string managerSceneName;

        [SerializeField] private ScriptableObject[] _scriptableObjects;

        private Dictionary<Type, object> _presentationLayers = new Dictionary<Type, object>();
        private Dictionary<Type, object> _domainLayers = new Dictionary<Type, object>();
        private Dictionary<Type, object> _dataLayers = new Dictionary<Type, object>();

        public static LayeredServiceLocator Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                _presentationLayers = new();
                _domainLayers = new();
                _dataLayers = new();
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            KeyLogger.Log("Initialize Complete", this);
        }

        public void RegisterPresentation<T>(T instance) where T : IPresentationLayer
        {
        }
    }
}