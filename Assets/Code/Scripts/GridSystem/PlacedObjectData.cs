using System.Collections.Generic;
using Interface;
using Service;
using ServiceManagement;
using UnitInfo;

namespace GridSystem
{
    public class PlacedObjectData : IDataLayer
    {
        private readonly List<PutUnitData> _putUnitDataList = new();

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
