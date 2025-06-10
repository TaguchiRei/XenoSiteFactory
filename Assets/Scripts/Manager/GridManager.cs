using System.Collections.Generic;
using Interface;
using UnityEngine;

namespace Manager
{
    public class GridManager : MonoBehaviour, IManager
    {
        [SerializeField, Range(20, 200)] private int gridSize = 20;

        /// <summary>
        /// グリッドが占有されているかどうかを示す情報
        /// </summary>
        public bool[,,] Grid { get; private set; }

        /// <summary>
        /// グリッドに設置されている物を保存する
        /// </summary>
        public List<PutUnitData> PutUnitData { get; private set; }

        /// <summary>
        /// 初期化時に呼び出される
        /// </summary>
        public void Initialize()
        {
            Grid = new bool[gridSize, gridSize, gridSize];
            PutUnitData = new();
        }
    }

    /// <summary>
    /// 設置済みユニットのデータ
    /// </summary>
    public struct PutUnitData
    {
        public int UnitId;
        public Vector2Int Position;
        public Vector2Int Direction;
    }

    public struct UnitData
    {
        public ulong UnitShape;
        public Vector2Int[] EnterPositions;
        public Vector2Int[] ExitPositions;
    }
}