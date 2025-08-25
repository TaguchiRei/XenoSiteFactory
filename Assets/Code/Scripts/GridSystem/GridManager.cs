using GamesKeystoneFramework.KeyDebug.KeyLog;
using GamesKeystoneFramework.KeyMathBit;
using Interface;
using Service;
using StaticObject;
using UnitInfo;
using UnityEngine;

namespace GridSystem
{
    public class GridManager : MonoBehaviour, IDomainLayer
    {
        private const int GRID_SIZE = 128;
        private const int GRID_HEIGHT = 4;

        private readonly DUlong _oneDUlong;

        private readonly GridExistData _gridExistData;
        private readonly GridDistanceData _gridDistanceData;
        private readonly PlacedObjectData _placedObjectData;

        #region テスト用スクリプト

        private void GenerateTestData()
        {
            KeyLogger.LogWarning("This is Test Only Method");
        }

        #endregion

        public GridManager()
        {
            _gridExistData = new();
            _gridDistanceData = new(10);
            _placedObjectData = new();
            _gridDistanceData.RegisterData();
            _gridDistanceData.RegisterData();
            _placedObjectData.RegisterData();

            _oneDUlong = new(0, 1);
        }

        /// <summary>
        /// 指定した形状のオブジェクトをpositionに設置する。
        /// </summary>
        /// <param name="shape">設置するオブジェクトの形状</param>
        /// <param name="position"></param>
        /// <param name="putUnitData"></param>
        /// <returns>設置できたかを返す。</returns>
        public bool PutUnit(ulong shape, Vector3Int position, PutUnitData putUnitData)
        {
            if (CheckCanPut(shape, position)) return false;

            _gridExistData.SetGridData(shape, position);
            _gridDistanceData.SetGridData(shape, position);
            _placedObjectData.SetUnit(putUnitData);

            return true;
        }


        #region 非公開メソッド

        private bool CheckCanPut(ulong shape, Vector3Int position)
        {
            int edge = BitShapeSupporter.GetEdge();
            var dUlongGrid = _gridExistData.GetGridData();
            for (int x = 0; x < edge; x++)
            {
                for (int y = 0; y < edge; y++)
                {
                    for (int z = 0; z < edge; z++)
                    {
                        int bitPosition = BitShapeSupporter.CalculationBitPosition(x, y, z);
                        if ((shape & (1ul << bitPosition)) != 0)
                        {
                            //範囲内チェックとビットがたっているかのチェック
                            if (position.x + x >= GRID_SIZE || position.z + z >= GRID_SIZE ||
                                position.y + y >= GRID_HEIGHT ||
                                (dUlongGrid[position.x + x, position.y + y] & (_oneDUlong << (position.z + z))) != 0)
                            {
                                return false;
                            }

                            //地面に直接ふれておらず、ビットがたっていればfalseを返す
                            if (y == 0 && position.y != 0)
                            {
                                var checkHeight = position.y - 1;
                                if ((dUlongGrid[position.x + x, checkHeight] & (_oneDUlong << (position.z + z))) == 0)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }

        #endregion


        public void Dispose()
        {
            LayeredServiceLocator.Instance.UnRegisterDomain(this);
        }

        public void RegisterDomain()
        {
            LayeredServiceLocator.Instance.RegisterDomain(this);
        }
    }
}