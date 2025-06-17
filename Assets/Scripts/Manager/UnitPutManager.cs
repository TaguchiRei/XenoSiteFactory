using System;
using DIContainer;
using Interface;
using UnityEngine;
using UnityEngine.Serialization;

namespace Manager
{
    public class UnitPutManager : MonoBehaviour, IManager
    {
        [SerializeField] private int _layerLimit = 4;
        private InGameManager _inGameManager;
        public int PutLayer
        {
            get;
            private set;
        }

        public void PutMode()
        {
            if (_inGameManager == null) DiContainer.Instance.TryGet(out _inGameManager);
            if(_inGameManager == null) return;
            _inGameManager?.PutModeChange();
        }
        
        /// <summary>
        /// レイヤーを一つ上げる
        /// </summary>
        public void UpLayer()
        {
            PutLayer++;
            if (PutLayer > _layerLimit)
            {
                PutLayer = 1;
            }
        }

        /// <summary>
        /// レイヤーを一つ下げる
        /// </summary>
        public void DownLayer()
        {
            PutLayer--;
            if (PutLayer <= 0)
            {
                PutLayer = _layerLimit;
            }
        }
        
        public void Register()
        {
            DiContainer.Instance.Register(this);
        }

        public void Initialize()
        {
            PutLayer = 1;
        }

        public void Awake()
        {
            Register();
        }
    }
}
