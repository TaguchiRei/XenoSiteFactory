using System;
using DIContainer;
using Manager;
using StaticObject;
using UnityEngine;

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
        }

        private void OnInteract()
        {
            //オブジェクトを設置する処理を書く
            var data = _unitResourceManager.GetUnitData(UnitPutSupport.SelectedUnitType,UnitPutSupport.SelectedUnitID);
            if (_gridManager.CheckCanPutUnit(data.UnitShape, UnitPutSupport.SelectedPosition))
            {
                //_gridManager.
            }
        }
    }
}
