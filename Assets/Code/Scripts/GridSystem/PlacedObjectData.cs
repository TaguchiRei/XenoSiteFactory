using System.Collections.Generic;
using Interface;
using Service;
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
