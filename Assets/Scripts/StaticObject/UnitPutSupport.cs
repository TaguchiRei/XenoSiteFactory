using DIContainer;
using Manager;
using UnityEngine;

namespace StaticObject
{
    public static class UnitPutSupport
    {
        public static Vector3Int SelectedPosition;

        public static bool SelectMode
        {
            get;
            set;
        }
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
