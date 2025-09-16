using System;
using System.Collections.Generic;
using Interface;
using ServiceManagement;
using UnitInfo;
using UnityEngine;

namespace GridSystem
{
    [Serializable]
    public class PlacedObjectData : IDataLayer
    {
        [SerializeField] private List<PutUnitData> _putUnitDataList = new();

        public void SetUnit(PutUnitData putUnitData)
        {
            _putUnitDataList.Add(putUnitData);
        }

        public void RemoveUnit(PutUnitData putUnitData)
        {
            _putUnitDataList.Remove(putUnitData);
        }

        public List<PutUnitData> GetAllUnitData()
        {
            return new(_putUnitDataList);
        }

        public void Dispose()
        {
            ServiceLocateManager.Instance.UnRegisterData(this);
        }

        public void RegisterData()
        {
            ServiceLocateManager.Instance.RegisterData(this);
        }
    }
}
