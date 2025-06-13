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
        [SerializeField, Range(-1f, 1f)] private float pointerOffset;
        
        
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
                var hitNormal = hit.normal;
                var hitPos = hit.point;
                //そのまま値を使うとタイルが埋まる(望ましくないほうに小数点が切り捨て、切り上げられるられるなど)が発生したので法線方向に少しだけ修正
                hitPos += hitNormal * 0.1f;
                var pointerPos = new Vector3(Mathf.Round(hitPos.x), Mathf.Round(hitPos.y), Mathf.Round(hitPos.z));
                transform.rotation = Quaternion.FromToRotation(Vector3.up, hitNormal);
                //pointerOffsetとtransformをかけて法線方向に多少ずらす。
                transform.position = pointerPos + transform.up * pointerOffset;
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
