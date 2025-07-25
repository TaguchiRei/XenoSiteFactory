using GamesKeystoneFramework.KeyMathBit;
using Interface;
using Service;
using StaticObject;
using UnityEngine;

namespace GridSystem
{
    /// <summary>
    /// グリッドの占有情報を保存するクラス
    /// </summary>
    public class GridExistData : IDataLayer
    {
        /// <summary> グリッドが占有されているエリアを保存する </summary>
        private readonly DUlong[,] _dUlongGrid = new DUlong[128, 4];

        /// <summary>
        /// グリッドに設置したユニットの情報を保存する
        /// </summary>
        /// <param name="shape"></param>
        /// <param name="position"></param>
        public void SetGridData(ulong shape, Vector3Int position)
        {
            int edge = BitShapeSupporter.GetEdge();
            DUlong oneDUlong = new DUlong(0,1);
            for (int x = 0; x < edge; x++)
            {
                for (int y = 0; y < edge; y++)
                {
                    for (int z = 0; z < edge; z++)
                    {
                        int bitPosition = BitShapeSupporter.CalculationBitPosition(x, y, z);
                        if ((shape & (1ul << bitPosition)) == 0) continue;
                        _dUlongGrid[position.x + x, position.y + y] |= oneDUlong << (position.z + z);
                    }
                }
            }
        }

        /// <summary>
        /// グリッドから外したオブジェクトの情報を保存する
        /// </summary>
        /// <param name="shape"></param>
        /// <param name="position"></param>
        public void RemoveGridData(ulong shape, Vector3Int position)
        {
            int edge = BitShapeSupporter.GetEdge();
            DUlong oneDUlong = new DUlong(0, 1);
            for (int x = 0; x < edge; x++)
            {
                for (int y = 0; y < edge; y++)
                {
                    for (int z = 0; z < edge; z++)
                    {
                        int bitPosition = BitShapeSupporter.CalculationBitPosition(x, y, z);
                        if ((shape & (1ul << bitPosition)) == 0) continue;
                        _dUlongGrid[position.x + x, position.y + y] &= ~(oneDUlong << (position.z + z));
                    }
                }
            }
        }

        /// <summary>
        /// グリッドデータをすべて取得する
        /// </summary>
        /// <returns></returns>
        public DUlong[,] GetGridData()
        {
            return _dUlongGrid;
        }

        /// <summary>
        /// サービスロケーターに登録する
        /// </summary>
        public void RegisterData()
        {
            LayeredServiceLocator.Instance.RegisterData(this);
        }

        public void Dispose()
        {
            LayeredServiceLocator.Instance.UnRegisterData(this);
        }
    }
}