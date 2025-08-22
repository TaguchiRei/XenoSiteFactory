using System.Collections.Generic;
using Interface;
using Service;
using UnitInfo;

namespace GridSystem
{
    public class PlacedObjectData : IDataLayer
    {
        private List<PutUnitData> _putUnitDataList;

        private PlacedObjectData()
        {
            _putUnitDataList = new List<PutUnitData>();
        }

        public void SetUnit(PutUnitData putUnitData)
        {
            _putUnitDataList.Add(putUnitData);
        }

        public List<PutUnitData> GetAllUnitData()
        {
            return new(_putUnitDataList);
        }

        public void Dispose()
        {
            LayeredServiceLocator.Instance.UnRegisterData(this);
        }

        public void RegisterData()
        {
            LayeredServiceLocator.Instance.RegisterData(this);
        }
    }
}
