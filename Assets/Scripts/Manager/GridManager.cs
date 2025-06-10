using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Interface;
using UnityEngine;
using UnityEngine.Serialization;
using XenoScriptableObject;

namespace Manager
{
    public class GridManager : MonoBehaviour, IManager
    {
        [SerializeField, Range(20, 200)] private int _gridSize = 20;
        [SerializeField] private AllUnitData _allUnitData;

        /// <summary>
        /// グリッドが占有されているかどうかを示す情報
        /// </summary>
        public bool[,,] Grid { get; private set; }

        /// <summary>
        /// グリッドに設置されている物を保存する
        /// </summary>
        public List<PutUnitData> PutUnitDataList { get; private set; }

        /// <summary>
        /// 初期化時に呼び出される
        /// </summary>
        public void Initialize()
        {
            Grid = new bool[_gridSize, _gridSize, _gridSize];
            PutUnitDataList = new();
        }

        /// <summary>
        /// グリッドを設置済みユニットデータを利用して埋める
        /// </summary>
        private async Awaitable FillGrid()
        {
            await Awaitable.BackgroundThreadAsync();

            bool[,,] unitShape = new bool[4, 4, 4];
            foreach (var putUnitData in PutUnitDataList)
            {
                UnitData unit = _allUnitData.AllUnit[(int)putUnitData.unitType][putUnitData.UnitId];

                //boolの配列を回転
                var shape = GetUnitShape(unit.UnitShape);
                switch (putUnitData.Direction)
                {
                    case UnitRotate.Right90:
                        shape = RotateRight90(shape);
                        break;
                    case UnitRotate.Right180:
                        shape = RotateRight180(shape);
                        break;
                    case UnitRotate.Right270:
                        shape = RotateRight270(shape);
                        break;
                }
            }
        }

        /// <summary>
        /// ulongの値をboolの配列に変換
        /// </summary>
        /// <param name="shape">形を示すulongを入れる</param>
        /// <returns></returns>
        private bool[,,] GetUnitShape(ulong shape)
        {
            var unitShape = new bool[4, 4, 4];
            for (int z = 1; z <= 4; z++)
            {
                for (int y = 1; y <= 4; y++)
                {
                    for (int x = 1; x <= 4; x++)
                    {
                        int bitPosition = (x - 1) + (y - 1) * 4 + (z - 1) * 16;
                        if ((shape & ((ulong)1 << bitPosition)) != 0)
                        {
                            unitShape[x, y, z] = true;
                        }
                    }
                }
            }

            return unitShape;
        }

        /// <summary>
        /// boolの配列を90度右回転させる。
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public bool[,,] RotateRight90(bool[,,] matrix)
        {
            bool[,,] result = new bool[4, 4, 4];
            for (int z = 0; z < 4; z++)
            {
                for (int x = 0; x < 4; x++)
                {
                    for (int y = 0; y < 4; y++)
                    {
                        result[y, 3 - x, z] = matrix[x, y, z];
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///　boolの配列を180度回転させる
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public bool[,,] RotateRight180(bool[,,] matrix)
        {
            bool[,,] result = new bool[4, 4, 4];
            for (int z = 0; z < 4; z++)
            {
                for (int x = 0; x < 4; x++)
                {
                    for (int y = 0; y < 4; y++)
                    {
                        result[3 - x, 3 - y, z] = matrix[x, y, z];
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// boolの配列を270度回転させる
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public bool[,,] RotateRight270(bool[,,] matrix)
        {
            bool[,,] result = new bool[4, 4, 4];
            for (int z = 0; z < 4; z++)
            {
                for (int x = 0; x < 4; x++)
                {
                    for (int y = 0; y < 4; y++)
                    {
                        result[3 - y, x, z] = matrix[x, y, z];
                    }
                }
            }

            return result;
        }
    }

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

        public UnitType unitType;
        public Vector2Int Position;
        public UnitRotate Direction;
    }

    [Serializable]
    public struct UnitData
    {
        public ulong UnitShape;
        public Vector2Int[] EnterPositions;
        public Vector2Int[] ExitPositions;
    }

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
}