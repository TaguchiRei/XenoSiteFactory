using Interface;
using Service;
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
        
        private byte[,,] _grid;

        private Vector2Int[] _checkPositions;
        
        GridDistanceData()
        {
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
        }

        /// <summary>
        /// 設置ユニットの距離データを保存する
        /// </summary>
        /// <param name="shape"></param>
        /// <param name="position"></param>
        public void SetGridData(ulong shape, Vector3Int position)
        {
            
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
