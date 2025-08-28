using GamesKeystoneFramework.KeyDebug.KeyLog;
using Service;
using Manager;
using StaticObject;
using UnitInfo;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class UnitAttacheDetaching : MonoBehaviour
    {
        private PlayerOperationManager _playerOperationManager;
        private UnitResourceManager _unitResourceManager;
        private InGameUIManager _inGameUIManager;
        private InGameManager _inGameManager;
        private GridManagerL _gridManagerL;

        private void Start()
        {
            ServiceLocatorL.Instance.TryGetClass(out _playerOperationManager);
            ServiceLocatorL.Instance.TryGetClass(out _unitResourceManager);
            ServiceLocatorL.Instance.TryGetClass(out _inGameUIManager);
            ServiceLocatorL.Instance.TryGetClass(out _inGameManager);
            ServiceLocatorL.Instance.TryGetClass(out _gridManagerL);
            _playerOperationManager.OnInteractAction += OnInteract;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private void OnInteract(InputAction.CallbackContext context)
        {
            if (!context.started || !_inGameManager.PutMode) return;
            if (!_inGameUIManager.CheckOnInputArea())
            {
                KeyLogger.Log("Not On Input Area");
                return;
            }
            //オブジェクトを設置する処理を書く
            var data = _unitResourceManager.GetUnitData(UnitPutSupport.SelectedUnitType,
                UnitPutSupport.SelectedUnitID);
            if (_gridManagerL.CheckCanPutUnit(data.UnitShape, UnitPutSupport.SelectedPosition))
            {
                ulong shape = UnitPutSupport.SelectedUnitRotate switch
                {
                    UnitRotate.Right90 => BitShapeSupporter.RotateRightUlongBase90(data.UnitShape),
                    UnitRotate.Right180 => BitShapeSupporter.RotateRightUlongBase180(data.UnitShape),
                    UnitRotate.Right270 => BitShapeSupporter.RotateRightUlongBase270(data.UnitShape),
                    _ => data.UnitShape
                };
                UnitPutSupport.CreatePrefab(data.UnitObject,UnitPutSupport.SelectedPosition,UnitPutSupport.SelectedUnitRotate);
                _gridManagerL.PutUnitOnGrid(shape, UnitPutSupport.SelectedPosition);
            }
        }
    }
}