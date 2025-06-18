using System;
using UnityEngine;

namespace Manager
{
    public partial class GridManager
    {
        #region struct系列

        /// <summary>
        /// 設置済みユニットのデータ
        /// </summary>
        [Serializable]
        public struct PutUnitData
        {
            /// <summary>
            /// スクリプタブルオブジェクト内の配列のインデックス番号と対応
            /// </summary>
            public int UnitId;

            public UnitType UnitType;
            public Vector2Int Position; // X, Z座標を表す
            public UnitRotate Direction;
        }

        [Serializable]
        public struct UnitData
        {
            public ulong UnitShape;
            public UnitType UnitType;
            public Vector2Int[] EnterPositions;
            public Vector2Int[] ExitPositions;
            public GameObject UnitObject;
        }

        [Serializable]
        public struct WallData
        {
            /// <summary>
            /// 壁の最も原点に近い部分の座標
            /// </summary>
            public GameObject wallPrefab;

            [Tooltip("壁に囲まれた空間の中心座標")] public Vector3Int Position;
            [Tooltip("壁の高さ")] public int Height;
            [Tooltip("壁の厚み 必ず奇数にしてください")] public int Width;
            [Tooltip("壁の外側の長さ")] public int Size;
        }

        #endregion

        #region enum系列

        public enum UnitType : byte
        {
            Manufacture = 0,
            Defense = 1,
            XenoSite = 2,
        }

        public enum UnitRotate : byte
        {
            Default = 0,
            Right90 = 1,
            Right180 = 2,
            Right270 = 3,
        }

        #endregion
    }
}