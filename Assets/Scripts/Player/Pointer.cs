using System;
using DIContainer;
using GamesKeystoneFramework.KeyDebug.KeyLog;
using Interface;
using Manager;
using UnityEngine;
using UnityEngine.UIElements;

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
            _playerOperationManager.OnMouseMoveAction += GetMousePosition;
        }

        private void Update()
        {
            if(IsPaused) return;
            KeyLogger.Log("MousePos" + _mousePosition);
            var ray = Camera.main.ScreenPointToRay(_mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                var hitPos = new Vector3Int((int)hit.point.x, (int)hit.point.y, (int)hit.point.z);
                var hitNormal = hit.normal.normalized;
                transform.position = hitPos;
                transform.rotation = Quaternion.FromToRotation(Vector3.up, hitNormal);
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
