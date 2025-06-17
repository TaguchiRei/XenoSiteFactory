using DIContainer;
using GamesKeystoneFramework.KeyDebug.KeyLog;
using Interface;
using Manager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Pointer : MonoBehaviour, IPauseable
    {
        public bool IsPaused { get; set; }
        [SerializeField, Range(-1f, 1f)] private float pointerOffset;
        
        
        InGameManager _inGameManager;
        PlayerOperationManager _playerOperationManager;
        UnitPutManager _unitPutManager;
        
        
        private void Start()
        {
            if(DiContainer.Instance.TryGet(out _inGameManager) &&
               DiContainer.Instance.TryGet(out _playerOperationManager) &&
               DiContainer.Instance.TryGet(out _unitPutManager))
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



        private void GetMousePosition(InputAction.CallbackContext context)
        {
            if(IsPaused) return;
            Vector2 mousePosition = context.ReadValue<Vector2>();
            var ray = Camera.main.ScreenPointToRay(mousePosition);
            var mask = LayerMask.GetMask($"Layer{_unitPutManager.PutLayer}Collider");
            if (Physics.Raycast(ray, out RaycastHit hit, 20f, mask))
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
