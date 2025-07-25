using Service;
using GamesKeystoneFramework.KeyDebug.KeyLog;
using Interface;
using Manager;
using StaticObject;
using UnityEngine;
using UnityEngine.InputSystem;
using GridManager = Manager.GridManager;

namespace Player
{
    public class Pointer : MonoBehaviour, IPauseable
    {
        public bool IsPaused { get; set; }
        [SerializeField, Range(-2f, 2f)] private float pointerOffset;
        
        PlayerOperationManager _playerOperationManager;
        GridManager _gridManager;
        UnitResourceManager _unitResourceManager;
        
        
        
        private void Start()
        {
            if(ServiceLocatorL.Instance.TryGetClass(out _playerOperationManager) &&
               ServiceLocatorL.Instance.TryGetClass(out _gridManager) &&
               ServiceLocatorL.Instance.TryGetClass(out _unitResourceManager))
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
            var mask = LayerMask.GetMask($"Layer{_gridManager.PutLayer}Collider");
            
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, mask))
            {
                var hitNormal = hit.normal;
                var hitPos = hit.point;
                //そのまま値を使うとタイルが埋まる(望ましくないほうに小数点が切り捨て、切り上げられるられるなど)が発生したので法線方向に少しだけ修正
                hitPos += hitNormal * 0.1f;
                var pointerPos = new Vector3Int((int)Mathf.Round(hitPos.x), (int)Mathf.Round(hitPos.y), (int)Mathf.Round(hitPos.z));
                transform.rotation = Quaternion.FromToRotation(Vector3.up, hitNormal);
                //pointerOffsetとtransformをかけて法線方向に多少ずらす。
                transform.position = pointerPos + transform.up * pointerOffset;
                //座標を保存
                UnitPutSupport.SelectedPosition = pointerPos;
                PointerAppearance();
            }
        }

        private void PointerAppearance()
        {
            
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
