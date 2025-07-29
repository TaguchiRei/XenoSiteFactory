using System.Collections.Generic;
using GamesKeystoneFramework.KeyMathBit;
using Interface;
using Service;
using StaticObject;
using Unity.Mathematics;
using UnityEngine;

namespace GridSystem
{
    /// <summary>
    /// 設置済みオブジェクトからの距離を保存するクラス
    /// </summary>
    public class GridDistanceData : IDataLayer
    {
        private const int GRID_WIDTH = 128;
        private const int GRID_HEIGHT = 4;

        private readonly byte[,,] _grid;
        private readonly int _manhattanDistance;
        private readonly List<Vector3Int> _checkPositionOffset;
        private readonly List<Vector3Int> _reexplorationPosOffset;

        GridDistanceData(int manhattanDistance)
        {
            _manhattanDistance = manhattanDistance;
            _checkPositionOffset = new();
            _grid = new byte[GRID_WIDTH, GRID_HEIGHT, GRID_WIDTH];

            _reexplorationPosOffset = new();
            for (int x = 0; x < _manhattanDistance * 2; x++)
            {
                for (int y = 0; y < _manhattanDistance * 2; y++)
                {
                    //ユニット設置時の探査用リストの初期化
                    if (x + y > _manhattanDistance * 2) continue;
                    _reexplorationPosOffset.Add(new(x, 0, y));
                    //ユニットを外した後の再探査用リストの初期化
                    if (x + y > _manhattanDistance) continue;
                    _checkPositionOffset.Add(new(x, 0, y));
                }
            }

            ResetGridData();
        }

        /// <summary>
        /// グリッドのデータを初期化する。
        /// 中央からのマンハッタン距離で初期化を行う
        /// </summary>
        private void ResetGridData()
        {
            int centerX = GRID_WIDTH / 2;
            int centerZ = GRID_WIDTH / 2;
            for (int z = 0; z < GRID_WIDTH; z++)
            {
                for (int y = 0; y < GRID_HEIGHT; y++)
                {
                    for (int x = 0; x < GRID_WIDTH; x++)
                    {
                        _grid[x, y, z] = (byte)(Mathf.Abs(x - centerX) + Mathf.Abs(z - centerZ));
                    }
                }
            }
        }

        /// <summary>
        /// 設置ユニットの距離データを保存する
        /// </summary>
        /// <param name="shape"></param>
        /// <param name="setPosition"></param>
        public void SetGridData(ulong shape, Vector3Int setPosition)
        {
            int edge = BitShapeSupporter.GetEdge();
            for (int x = 0; x < edge; x++)
            {
                for (int y = 0; y < edge; y++)
                {
                    for (int z = 0; z < edge; z++)
                    {
                        int bitPosition = BitShapeSupporter.CalculationBitPosition(x, y, z);
                        if ((shape & (1ul << bitPosition)) == 0) continue;
                        Vector3Int newPos = new(setPosition.x + x, setPosition.y + y, setPosition.z + z);
                        SetDistanceData(newPos);
                    }
                }
            }
        }

        /// <summary>
        /// グリッドからオブジェクトを外した際の処理
        /// </summary>
        /// <param name="shape"></param>
        /// <param name="setPosition"></param>
        public void RemoveGridData(ulong shape, Vector3Int setPosition)
        {
            //まず影響範囲内の盤面の初期化を行う
            int edge = BitShapeSupporter.GetEdge();
            for (int x = 0; x < edge; x++)
            {
                for (int y = 0; y < edge; y++)
                {
                    for (int z = 0; z < edge; z++)
                    {
                        int bitPosition = BitShapeSupporter.CalculationBitPosition(x, y, z);
                        if ((shape & (1ul << bitPosition)) == 0) continue;
                        ResetToDefaultDistanceData(new(setPosition.x + x, setPosition.y + y, setPosition.z + z));
                    }
                }
            }

            //次に距離の二倍で探索し、ゼロになっている箇所(オブジェクトが設置されている場所)から距離の再計算をする
            for (int x = 0; x < edge; x++)
            {
                for (int y = 0; y < edge; y++)
                {
                    for (int z = 0; z < edge; z++)
                    {
                        if (!IsInBounds(setPosition + new Vector3Int(x, y, z))) continue;

                        int bitPosition = BitShapeSupporter.CalculationBitPosition(x, y, z);
                        if ((shape & (1ul << bitPosition)) == 0) continue;

                        foreach (var position in GetOffsetCellPositions(setPosition + new Vector3Int(x, y, z), _reexplorationPosOffset))
                        {
                            if (_grid[position.x, position.y, position.z] == 0)
                            {
                                SetDistanceData(position);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 指定した座標を中心に距離データの書き換えを行う
        /// </summary>
        /// <param name="position"></param>
        private void SetDistanceData(Vector3Int position)
        {
            foreach (var checkPosition in GetOffsetCellPositions(position, _checkPositionOffset))
            {
                Vector3Int checkVector = position - checkPosition;
                byte manhattan = (byte)(Mathf.Abs(checkVector.x) + Mathf.Abs(checkVector.z));
                if (_grid[checkPosition.x, checkPosition.y, checkPosition.z] > manhattan)
                {
                    _grid[checkPosition.x, checkPosition.y, checkPosition.z] = manhattan;
                }
            }
        }


        /// <summary>
        /// 指定した座標から一定のマンハッタン距離の座標の距離情報を中心からのものにリセットする
        /// </summary>
        private void ResetToDefaultDistanceData(Vector3Int position)
        {
            int centerX = GRID_WIDTH / 2;
            int centerZ = GRID_WIDTH / 2;

            foreach (var resetPosition in GetOffsetCellPositions(position, _checkPositionOffset))
            {
                byte manhattan = (byte)(Mathf.Abs(resetPosition.x - centerX) + Mathf.Abs(resetPosition.z - centerZ));
                _grid[resetPosition.x, resetPosition.y, resetPosition.z] = manhattan;
            }
        }

        /// <summary>
        /// オブジェクトに探索範囲の配列を足した配列を取得できる
        /// </summary>
        private List<Vector3Int> GetOffsetCellPositions(Vector3Int position, List<Vector3Int> offsets)
        {
            List<Vector3Int> replacedCells = new();
            foreach (var offset in offsets)
            {
                if (!IsInBounds(position + offset)) continue;
                replacedCells.Add(position + offset);
            }

            return replacedCells;
        }

        /// <summary>
        /// 指定の座標が範囲内かどうかを調べる
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool IsInBounds(Vector3Int pos)
        {
            return pos.x >= 0 && pos.x < GRID_WIDTH &&
                   pos.y >= 0 && pos.y < GRID_HEIGHT &&
                   pos.z >= 0 && pos.z < GRID_WIDTH;
        }

        public void Dispose()
        {
            LayeredServiceLocator.Instance.UnRegisterData(this);
        }

        public void RegisterData()
        {
            LayeredServiceLocator.Instance.RegisterData(this);
        }
    }
}