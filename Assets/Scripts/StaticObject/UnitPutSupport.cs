using DIContainer;
using Manager;
using UnityEngine;

namespace StaticObject
{
    public static class UnitPutSupport
    {
        public static Vector3Int SelectedPosition;

        public static bool SelectMode { get; set; }

        public static GridManager.UnitType SelectedUnitType { get; private set; }
        public static GridManager.UnitRotate SelectedUnitRotate { get; private set; }
        public static int SelectedUnitID { get; private set; }
        
        public static void SetSelectUnitData(int id, GridManager.UnitType unitType)
        {
            SelectedUnitType = unitType;
            SelectedUnitID = id;
        }

        public static void ChangeRotation(GridManager.UnitRotate unitRotate)
        {
            SelectedUnitRotate = unitRotate;
        }
    }
}