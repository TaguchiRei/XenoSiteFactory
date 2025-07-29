using Manager;
using UnitInfo;
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

        public static UnitType SelectedUnitType { get; private set; }
        public static UnitRotate SelectedUnitRotate { get; private set; }
        public static int SelectedUnitID { get; private set; }
        
        public static void SetSelectUnitData(int id, UnitType unitType)
        {
            SelectedUnitType = unitType;
            SelectedUnitID = id;
        }

        public static void ChangeRotation(UnitRotate unitRotate)
        {
            SelectedUnitRotate = unitRotate;
        }

        public static void CreatePrefab(GameObject prefab,Vector3 position, UnitRotate rotate)
        {
            Quaternion rotation = rotate switch
            {
                UnitRotate.Default => Quaternion.identity,
                UnitRotate.Right90 => Quaternion.AngleAxis(90, Vector3.up),
                UnitRotate.Right180 => Quaternion.AngleAxis(180, Vector3.up),
                UnitRotate.Right270 => Quaternion.AngleAxis(270, Vector3.up),
                _ => Quaternion.identity
            };
            Object.Instantiate(prefab, position, Quaternion.identity)
                .transform.GetChild(0)
                .transform.rotation = rotation;
        }
    }
}