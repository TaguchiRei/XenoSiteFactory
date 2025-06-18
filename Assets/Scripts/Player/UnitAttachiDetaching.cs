using DIContainer;
using GamesKeystoneFramework.KeyDebug.KeyLog;
using Manager;
using StaticObject;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class UnitAttacheDetaching : MonoBehaviour
    {
        private PlayerOperationManager _playerOperationManager;
        private UnitResourceManager _unitResourceManager;
        private GridManager _gridManager;

        private void Start()
        {
            DiContainer.Instance.TryGet(out _playerOperationManager);
            DiContainer.Instance.TryGet(out _unitResourceManager);
            DiContainer.Instance.TryGet(out _gridManager);
            _playerOperationManager.OnInteractAction += OnInteract;
        }

        private void OnInteract(InputAction.CallbackContext context)
        {
            KeyLogger.Log("OnInteract");
            if (!context.started) return;
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
                _gridManager.PutUnitOnGrid(shape, UnitPutSupport.SelectedPosition);
            }
        }
    }
}