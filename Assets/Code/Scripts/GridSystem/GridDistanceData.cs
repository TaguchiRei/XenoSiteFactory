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

        private readonly int _manhattanDistance;
        private readonly List<Vector3Int> _checkPositionOffset;
        private readonly byte[,,] _grid;

        GridDistanceData(int manhattanDistance)
        {
            _manhattanDistance = manhattanDistance;
            _checkPositionOffset = new();
            _grid = new byte[GRID_WIDTH, GRID_HEIGHT, GRID_WIDTH];
            int centerX = GRID_WIDTH / 2;
            int centerZ = GRID_WIDTH / 2;
            for (int z = 0; z < GRID_WIDTH; z++)
            {
                for (int y = 0; y < GRID_HEIGHT; y++)
                {
                    for (int x = 0; x < GRID_WIDTH; x++)
                    {
                        int dx = Mathf.Abs(x - centerX);
                        int dz = Mathf.Abs(z - centerZ);
                        _grid[x, y, z] = (byte)(dx + dz);
                    }
                }
            }

            for (int x = 0; x < _manhattanDistance; x++)
            {
                for (int y = 0; y < _manhattanDistance; y++)
                {
                    if (x + y > _manhattanDistance) continue;
                    _checkPositionOffset.Add(new(x, 0, y));
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
                        foreach (var checkPositionOffset in _checkPositionOffset)
                        {
                            Vector3Int checkPosition = newPos + checkPositionOffset;
                            Vector3Int checkVector = newPos - checkPosition;
                            byte manhattan = (byte)(Mathf.Abs(checkVector.x) + Mathf.Abs(checkVector.z));
                            if (_grid[checkPosition.x, checkPosition.y, checkPosition.z] > manhattan)
                            {
                                _grid[checkPosition.x, checkPosition.y, checkPosition.z] = manhattan;
                            }
                        }
                    }
                }
            }
        }

        public void RemoveGridData(ulong shape, Vector3Int setPosition)
        {
            
        }

        /// <summary>
        /// 指定した座標から一定のマンハッタン距離の座標の距離情報を中心からのものにリセットする
        /// </summary>
        private void ResetToDefaultDistanceData(Vector3Int position)
        {
            foreach (var offset in _checkPositionOffset)
            {
                
            }
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