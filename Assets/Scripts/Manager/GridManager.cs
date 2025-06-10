using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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

        private void Start()
        {
            for (int i = 1; i <= 4; i++)
            {
                for (int j = 1; j <= 4; j++)
                {
                    for (int k = 1; k <= 4; k++)
                    {
                        Debug.Log(i*j*k);
                    }
                }
            }
        }

        /// <summary>
        /// 初期化時に呼び出される
        /// </summary>
        public void Initialize()
        {
            Grid = new bool[gridSize, gridSize, gridSize];
            PutUnitData = new();
        }

        /// <summary>
        /// グリッドを設置済みユニットデータを利用して埋める
        /// </summary>
        private async Awaitable FillGrid()
        {
            await Awaitable.BackgroundThreadAsync();
            foreach (var unitData in PutUnitData)
            {

            }
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
}