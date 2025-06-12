using System;
using DIContainer;
using GamesKeystoneFramework.KeyDebug.KeyLog;
using Interface;
using Manager;
using UnityEngine;

namespace Player
{
    public class Pointer : MonoBehaviour, IPauseable
    {
        public bool IsPaused { get; set; }
        
        
        InGameManager _inGameManager;
        PlayerOperationManager _playerOperationManager;
        private Vector2 _mousePosition;
        
        private void Start()
        {
            if(DiContainer.Instance.TryGet(out _inGameManager) && DiContainer.Instance.TryGet(out _playerOperationManager))
            {
                KeyLogger.Log("GetManagerClass");
            }
            else
            {
                KeyLogger.Log("GetManagerClass");
                return;
            }
        }

        private void Update()
        {
            if(IsPaused) return;

            var ray = Camera.main.ScreenPointToRay(_mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                var hitPos = hit.collider.gameObject.transform.position;
                var hitNormal = hit.normal.normalized;
                
            }
        }

        private void GetMousePosition(Vector2 mousePosition)
        {
            _mousePosition = mousePosition;
        }


        public void Pause()
        {
            IsPaused = true;
        }

        public void Resume()
        {
            IsPaused = false;
        }
    }
}
