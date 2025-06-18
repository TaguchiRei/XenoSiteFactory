using DIContainer;
using Manager;
using UnityEngine;

namespace StaticObject
{
    public static class UnitPutSystem
    {
        private static UnitResourceManager _resourceManager;
        
        public static GridManager.UnitData SelectUnitData
        {
            get;
            private set;
        }

        public static GameObject SelectUnitObject
        {
            get;
            private set;
        }

        public static void SetSelectNumber(GridManager.UnitType type,int id)
        {
            if (_resourceManager == null)
            {
                DiContainer.Instance.TryGet(out _resourceManager);
            }
            SelectUnitData = _resourceManager.GetUnitData(type, id);
        }
    }
}
