using System.Collections.Generic;
using GamesKeystoneFramework.Attributes;
using GamesKeystoneFramework.KeyDebug.KeyLog;
using GamesKeystoneFramework.KeyMathBit;
using StaticObject;
using UnityEngine;
using XenoScriptableObject;
using Random = UnityEngine.Random;
using Service;

namespace Manager
{
    /// <summary>
    /// グリッドの変化を管理するクラス
    /// </summary>
    public partial class GridManager : ManagerBase<GridManager>
    {
        /// <summary> グリッドが占有されているエリアを保存する </summary>
        public DUlong[,] DUlongGrid { get; private set; }

        /// <summary> グリッドに設置されている物を保存する </summary>
        public List<PutUnitData> PutUnitDataList { get; private set; }

        public int PutLayer { get; private set; }

        private bool _gridCreated;
        private InGameManager _inGameManager;
        private AllUnitData _allUnitData;
        private readonly DUlong _oneDUlong = new(0, 1);
        [SerializeField] private GameObject _gridCollider;
        [SerializeField] private int _layerLimit = 4;
        [SerializeField, Range(20, 128)] private int _gridSize = 20;
        [SerializeField, KeyReadOnly] private int _height = 4;
        [SerializeField] WallData _wallData;

        private int _edge;

        /// <summary>
        /// テスト用のスクリプトなので今後削除予定です
        /// </summary>
        private void GenerateTestData()
        {
            KeyLogger.LogWarning("This is Test Only Method");
            PutUnitDataList = new List<PutUnitData>();
            for (int i = 0; i < 10; i++)
            {
                var id = Random.Range(0, 5);
                PutUnitDataList.Add(new PutUnitData()
                {
                    UnitId = id,
                    UnitType = _allUnitData.UnitTypeArray[0].AllUnit[id].UnitType,
                    Position = new Vector3Int(
                        Random.Range(0, _gridSize - _edge),
                        0,
                        Random.Range(0, _gridSize - _edge)),
                    Rotation = (UnitRotate)Random.Range(0, _edge)
                });
                KeyLogger.Log(
                    $"ID{id}  UnitPosition{PutUnitDataList[i].Position} UnitRotation{PutUnitDataList[i].Rotation}");
            }
        }

        /// <summary>
        /// グリッドマネージャーの初期化を行う。
        /// グリッド情報生成、壁の生成、占有座標へのコライダー配置等
        /// </summary>
        private void GridManagerInitialize(List<PutUnitData> putUnitDataList)
        {
            WallGenerator.GenerateWall(_wallData);
            DUlong[,] grid = new DUlong[_gridSize, _height];
            var wallIndex = WallGenerator.GetWallIndex(_wallData);
            foreach (var index in wallIndex)
            {
                for (int y = 0; y < _wallData.Height; y++)
                {
                    //XY平面上で表現されたグリッドにZ軸の情報を足す。index.yはZ軸情報
                    grid[index.x, y] |= _oneDUlong << index.y;
                }
            }

            grid = FillGridDUlongBase(grid, putUnitDataList);
            GenerateAllUnitInstance(putUnitDataList);
            DUlongGrid = grid;
            _gridCreated = true;
        }


        /// <summary>
        /// グリッドをPutUnitDataをもとに復元して返すメソッド。
        /// </summary>
        /// <returns></returns>
        private DUlong[,] FillGridDUlongBase(DUlong[,] grid, List<PutUnitData> unitDataList)
        {
            foreach (var putUnitData in unitDataList)
            {
                var unit = _allUnitData.UnitTypeArray[(int)putUnitData.UnitType].AllUnit[putUnitData.UnitId];

                ulong rotateShape = 0;

                switch (putUnitData.Rotation)
                {
                    case UnitRotate.Default:
                        rotateShape = unit.UnitShape;
                        break;
                    case UnitRotate.Right90:
                        rotateShape = BitShapeSupporter.RotateRightUlongBase90(unit.UnitShape);
                        break;
                    case UnitRotate.Right180:
                        rotateShape = BitShapeSupporter.RotateRightUlongBase180(unit.UnitShape);
                        break;
                    case UnitRotate.Right270:
                        rotateShape = BitShapeSupporter.RotateRightUlongBase270(unit.UnitShape);
                        break;
                }

                for (int z = 0; z < _edge; z++)
                {
                    for (int y = 0; y < _edge; y++)
                    {
                        for (int x = 0; x < _edge; x++)
                        {
                            int bitPosition = BitShapeSupporter.CalculationBitPosition(x, y, z);
                            if ((rotateShape & ((ulong)1 << bitPosition)) != 0)
                            {
                                int gridZ = putUnitData.Position.z + z;
                                grid[putUnitData.Position.x + x, y] |= _oneDUlong << gridZ;
                            }
                        }
                    }
                }
            }

            return grid;
        }

        /// <summary>
        /// 指定の座標に指定のオブジェクトを配置できるかどうかを確認する
        /// </summary>
        /// <param name="shape"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool CheckCanPutUnit(ulong shape, Vector3Int position)
        {
            for (int x = 0; x < _edge; x++)
            {
                for (int y = 0; y < _edge; y++)
                {
                    for (int z = 0; z < _edge; z++)
                    {
                        int bitPosition = BitShapeSupporter.CalculationBitPosition(x, y, z);
                        if ((shape & (1ul << bitPosition)) != 0)
                        {
                            //範囲内チェックとビットがたっているかのチェック
                            if (position.x + x >= _gridSize || position.z + z >= _gridSize ||
                                position.y + y >= _height ||
                                (DUlongGrid[position.x + x, position.y + y] & (_oneDUlong << (position.z + z))) != 0)
                            {
                                return false;
                            }

                            //地面に直接ふれておらず、ビットがたっていればfalseを返す
                            if (y == 0 && position.y != 0)
                            {
                                var checkHeight = position.y - 1;
                                if ((DUlongGrid[position.x + x, checkHeight] & (_oneDUlong << (position.z + z))) == 0)
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

        /// <summary>
        /// すべてのユニットのインスタンスを生成する
        /// </summary>
        /// <param name="putUnitDataList"></param>
        private void GenerateAllUnitInstance(List<PutUnitData> putUnitDataList)
        {
            foreach (var putUnitData in putUnitDataList)
            {
                var unit = _allUnitData.UnitTypeArray[(int)putUnitData.UnitType].AllUnit[putUnitData.UnitId];
                UnitPutSupport.CreatePrefab(unit.UnitObject, putUnitData.Position, putUnitData.Rotation);
            }
        }

        /// <summary>
        /// 特定の座標にオブジェクトのデータを保存しインスタンスを生成する
        /// </summary>
        /// <param name="shape"></param>
        /// <param name="position"></param>
        public void PutUnitOnGrid(ulong shape, Vector3Int position)
        {
            for (int x = 0; x < _edge; x++)
            {
                for (int y = 0; y < _edge; y++)
                {
                    for (int z = 0; z < _edge; z++)
                    {
                        int bitPosition = BitShapeSupporter.CalculationBitPosition(x, y, z);
                        if ((shape & (1ul << bitPosition)) == 0) continue;
                        DUlongGrid[position.x + x, position.y + y] |= _oneDUlong << (position.z + z);
                        PutUnitData data = new()
                        {
                            Rotation = UnitPutSupport.SelectedUnitRotate,
                            Position = new Vector3Int(),
                            UnitId = UnitPutSupport.SelectedUnitID,
                            UnitType = UnitPutSupport.SelectedUnitType
                        };
                        PutUnitDataList.Add(data);
                    }
                }
            }
        }

        /// <summary>
        /// レイヤーを一つ上げる
        /// </summary>
        public void UpLayer()
        {
            PutLayer++;
            if (PutLayer > _layerLimit)
            {
                PutLayer = 1;
            }
        }

        /// <summary>
        /// レイヤーを一つ下げる
        /// </summary>
        public void DownLayer()
        {
            PutLayer--;
            if (PutLayer <= 0)
            {
                PutLayer = _layerLimit;
            }
        }

        public void PutMode()
        {
            ServiceLocator.Instance.TryGetClass(out _inGameManager);
            _inGameManager.PutModeChange();
        }

        private void OnDrawGizmos()
        {
            if (!_gridCreated) return;

            Gizmos.color = Color.green;
            for (int x = 0; x < _gridSize; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    for (int z = 0; z < _gridSize; z++)
                    {
                        if ((DUlongGrid[x, y] & (_oneDUlong << z)) != new DUlong(0, 0))
                        {
                            Gizmos.DrawWireCube(new Vector3(x, y, z), Vector3.one);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 初期化時に呼び出される
        /// </summary>
        public override void Initialize()
        {
            ServiceLocator.Instance.TryGetScriptableObject(out _allUnitData);
            _edge = BitShapeSupporter.GetEdge();
            PutLayer = 1;
            GenerateTestData();
            GridManagerInitialize(PutUnitDataList);
        }
    }
}