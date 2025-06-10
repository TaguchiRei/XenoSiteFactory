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
                Vector2Int basePos = putUnitData.Position;
                Vector2Int baseDir = putUnitData.Direction;
                UnitData unit = _allUnitData.AllUnit[(int)putUnitData.unitType][putUnitData.UnitId];
                
                for (int z = 1; z <= 4; z++)
                {
                    for (int y = 1; y <= 4; y++)
                    {
                        for (int x = 1; x <= 4; x++)
                        {
                            int bitPosition = (x-1) + (y-1) * 4 + (z-1) * 16;
                            if ((unit.UnitShape & ((ulong)1 << bitPosition)) != 0)
                            {
                                unitShape[x, y, z] = true;
                            }
                        }
                    }
                }
            }
        }
        public bool[,] RotateRight90(bool[,] matrix)
        {
            bool[,] result = new bool[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    result[j, 3 - i] = matrix[i, j];
                }
            }
            return result;
        }

        // 180度回転
        public bool[,] RotateRight180(bool[,] matrix)
        {
            bool[,] result = new bool[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    result[3 - i, 3 - j] = matrix[i, j];
                }
            }
            return result;
        }

        // 270度右回転 (90度左回転と同じ)
        public bool[,] RotateRight270(bool[,] matrix)
        {
            bool[,] result = new bool[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    result[3 - j, i] = matrix[i, j];
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
        public Vector2Int Direction;
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