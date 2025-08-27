using GamesKeystoneFramework.KeyDebug.KeyLog;
using GamesKeystoneFramework.KeyMathBit;
using Interface;
using Service;
using StaticObject;
using UnitInfo;
using UnityEngine;
using XenoScriptableObject;

namespace GridSystem
{
    public class GridManager : MonoBehaviour, IDomainLayer
    {
        private const int GRID_SIZE = 128;
        private const int GRID_HEIGHT = 4;
        private const ulong WALL_SHAPE = 268439552;

        [SerializeField] private WallData _wallData;
        [SerializeField] private int _objectInfluenceRange;

        private DUlong _oneDUlong;
        private GridExistData _gridExistData;
        private AllUnitData _allUnitData;
        private GridDistanceData _gridDistanceData;
        private PlacedObjectData _placedObjectData;

        #region テスト用スクリプト

        private void GenerateTestData()
        {
            KeyLogger.LogWarning("This is Test Only Method");
        }

        #endregion

        #region 公開メソッド

        /// <summary>
        /// グリッドのシステム全体の初期化を行う。
        /// </summary>
        /// <param name="gridExistData"></param>
        /// <param name="gridDistanceData"></param>
        /// <param name="placedObjectData"></param>
        public void GridSystemInitialize(WallData wallData)
        {
            if (GetData(out _gridExistData) &&
                GetData(out _gridDistanceData) &&
                GetData(out _placedObjectData) &&
                LayeredServiceLocator.Instance.TryGetScriptableObject(out _allUnitData))
            {
            }

            _gridDistanceData.RegisterData();
            _placedObjectData.RegisterData();
            _wallData = wallData;

            _oneDUlong = new(0, 1);

            GenerateWall();
            PutAllUnit();
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
            GenerateUnitInstance(putUnitData);

            return true;
        }

        public bool TryRemoveUnit(Vector3Int position, PutUnitData putUnitData)
        {
            var shape = _allUnitData.UnitTypeArray[(int)putUnitData.UnitType].AllUnit[putUnitData.UnitId].UnitShape;
            _gridExistData.RemoveGridData(shape, position);
            _gridDistanceData.RemoveGridData(shape, position);
            _placedObjectData.RemoveUnit(putUnitData);

            return true;
        }

        #endregion

        #region 非公開メソッド

        /// <summary>
        /// すべてのユニットを一括で設置する
        /// </summary>
        /// <returns></returns>
        private void PutAllUnit()
        {
            var edge = BitShapeSupporter.GetEdge();
            foreach (var putUnitData in _placedObjectData.GetAllUnitData())
            {
                var unit = _allUnitData
                    .UnitTypeArray[(int)putUnitData.UnitType]
                    .AllUnit[putUnitData.UnitId];
                ulong rotateShape = BitShapeSupporter.RotateRightUlongBase(unit.UnitShape, putUnitData.Rotation);

                PutUnit(rotateShape, putUnitData.Position, putUnitData);
            }
        }

        private void GenerateWall()
        {
            WallGenerator.GenerateWall(_wallData);
            var wallIndex = WallGenerator.GetWallIndex(_wallData);
            foreach (var index in wallIndex)
            {
                for (int y = 0; y < _wallData.Height; y++)
                {
                    _gridExistData.SetGridData(WALL_SHAPE, new Vector3Int(index.x, 0, index.y));
                }
            }
        }

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

        private void GenerateUnitInstance(PutUnitData putUnitData)
        {
            var unit = _allUnitData.UnitTypeArray[(int)putUnitData.UnitType].AllUnit[putUnitData.UnitId];
            UnitPutSupport.CreatePrefab(unit.UnitObject, putUnitData.Position, putUnitData.Rotation);
        }

        #endregion

        #region インターフェース実装

        public void Dispose()
        {
            LayeredServiceLocator.Instance.UnRegisterDomain(this);
        }

        public void RegisterDomain()
        {
            LayeredServiceLocator.Instance.RegisterDomain(this);
        }

        public bool GetData<T>(out T instance) where T : IDataLayer
        {
            var result = LayeredServiceLocator.Instance.TryGetDataLayer(out T instanceData);
            instance = instanceData;
            return result;
        }

        #endregion
    }
}