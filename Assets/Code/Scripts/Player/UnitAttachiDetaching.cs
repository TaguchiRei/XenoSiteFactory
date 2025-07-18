using System;
using GamesKeystoneFramework.KeyDebug.KeyLog;
using Service;
using Manager;
using StaticObject;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Player
{
    public class UnitAttacheDetaching : MonoBehaviour
    {
        private PlayerOperationManager _playerOperationManager;
        private UnitResourceManager _unitResourceManager;
        private InGameUIManager _inGameUIManager;
        private InGameManager _inGameManager;
        private GridManager _gridManager;

        private void Start()
        {
            ServiceLocator.Instance.TryGetClass(out _playerOperationManager);
            ServiceLocator.Instance.TryGetClass(out _unitResourceManager);
            ServiceLocator.Instance.TryGetClass(out _inGameUIManager);
            ServiceLocator.Instance.TryGetClass(out _inGameManager);
            ServiceLocator.Instance.TryGetClass(out _gridManager);
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
            if (_gridManager.CheckCanPutUnit(data.UnitShape, UnitPutSupport.SelectedPosition))
            {
                ulong shape = UnitPutSupport.SelectedUnitRotate switch
                {
                    GridManager.UnitRotate.Right90 => BitShapeSupporter.RotateRightUlongBase90(data.UnitShape),
                    GridManager.UnitRotate.Right180 => BitShapeSupporter.RotateRightUlongBase180(data.UnitShape),
                    GridManager.UnitRotate.Right270 => BitShapeSupporter.RotateRightUlongBase270(data.UnitShape),
                    _ => data.UnitShape
                };
                UnitPutSupport.CreatePrefab(data.UnitObject,UnitPutSupport.SelectedPosition,UnitPutSupport.SelectedUnitRotate);
                _gridManager.PutUnitOnGrid(shape, UnitPutSupport.SelectedPosition);
            }
        }
    }
}