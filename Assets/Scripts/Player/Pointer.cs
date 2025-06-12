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



        private void GetMousePosition(Vector2 mousePosition)
        {
            if(IsPaused) return;
            var ray = Camera.main.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                var hitPos = new Vector3Int((int)hit.point.x, (int)hit.point.y, (int)hit.point.z);
                var hitNormal = hit.normal.normalized;
                transform.position = hitPos;
                transform.rotation = Quaternion.FromToRotation(Vector3.up, hitNormal);
            }
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
