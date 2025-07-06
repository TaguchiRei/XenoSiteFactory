using Manager;
using UnityEngine;
using Object = UnityEngine.Object;

namespace StaticObject
{
    /// <summary>
    /// ユニットの設置を補助する静的クラス
    /// </summary>
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

        public static void CreatePrefab(GameObject prefab,Vector3 position, GridManager.UnitRotate rotate)
        {
            Quaternion rotation = rotate switch
            {
                GridManager.UnitRotate.Default => Quaternion.identity,
                GridManager.UnitRotate.Right90 => Quaternion.AngleAxis(90, Vector3.up),
                GridManager.UnitRotate.Right180 => Quaternion.AngleAxis(180, Vector3.up),
                GridManager.UnitRotate.Right270 => Quaternion.AngleAxis(270, Vector3.up),
                _ => Quaternion.identity
            };
            Object.Instantiate(prefab, position, Quaternion.identity)
                .transform.GetChild(0)
                .transform.rotation = rotation;
        }
    }
}