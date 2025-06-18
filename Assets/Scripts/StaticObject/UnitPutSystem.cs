using DIContainer;
using Manager;
using UnityEngine;

namespace StaticObject
{
    public static class UnitPutSystem
    {
        private static UnitResourceManager _resourceManager;
        
        public static int SelectUnitID
        {
            get;
            private set;
        }

        public static void SetSelectUnitData(int id)
        {
            SelectUnitID = id;
        }
    }
}
