using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DIContainer;
using GamesKeystoneFramework.KeyDebug.KeyLog;
using GamesKeystoneFramework.KeyMathBit;
using Interface;
using StaticObject;
using UnityEngine;
using XenoScriptableObject;
using Random = UnityEngine.Random;

namespace Manager
{
    public class GridManager : MonoBehaviour, IManager
    {
        /// <summary> グリッドが占有されているエリアを保存する </summary>
        public DUlong[,] DUlongGrid { get; private set; }

        /// <summary> グリッドに設置されている物を保存する </summary>
        public List<PutUnitData> PutUnitDataList { get; private set; }
        /// <summary>一辺４の長さの４*４*４のデータを扱うため </summary>
        private const int Edge = 4;
        private static readonly Vector3 _wallOffset = new(0f, -0.5f, 0f);
        private bool _gridCreated;
        private readonly DUlong _oneDUlong = new(0, 1);
        [SerializeField] private AllUnitData _allUnitData;
        [SerializeField] private GameObject _gridCollider;
        [SerializeField, Range(20, 128)] private int _gridSize = 20;
        [SerializeField, Range(Edge, 10)] private int _height;
        [SerializeField] WallData _wallData;

        /// <summary>
        /// テスト用のスクリプトなので今後削除予定です
        /// </summary>
        private void GenerateTestData()
        {
            KeyLogger.LogWarning("This is Test Only Method");
            //テスト用スクリプト
            PutUnitDataList = new List<PutUnitData>();
            for (int i = 0; i < 10; i++)
            {
                var id = Random.Range(0, 5);
                PutUnitDataList.Add(new PutUnitData()
                {
                    UnitId = id,
                    UnitType = _allUnitData.UnitTypeArray[0].AllUnit[id].UnitType,
                    Position = new Vector2Int(Random.Range(0, _gridSize - Edge), Random.Range(0, _gridSize - Edge)),
                    Direction = (UnitRotate)Random.Range(0, Edge)
                });
                KeyLogger.Log($"ID{id}  UnitPosition{PutUnitDataList[i].Position}");
            }
        }

        /// <summary>
        /// グリッドマネージャーの初期化を行う。
        /// グリッド情報生成、壁の生成、占有座標へのコライダー配置等
        /// </summary>
        private async Awaitable GridManagerInitialize(List<PutUnitData> putUnitDataList)
        {
            GenerateWall();

            Awaitable.BackgroundThreadAsync();
            DUlong[,] grid = new DUlong[_gridSize, _height];
            var wallIndices = WallGenerator.GetWallIndex(_wallData);

            foreach (var index in wallIndices)
            {
                for (int y = 0; y < _wallData.Height; y++)
                {
                    //XY平面上で表現されたグリッドにZ軸の情報を足す。index.yはZ軸情報
                    grid[index.x, y] |= _oneDUlong << index.y;
                }

                Debug.Log($"InputIndex {index.x} {index.y}");
            }

            grid = FillGridDUlongBase(grid, putUnitDataList);
            await Awaitable.MainThreadAsync();

            DUlongGrid = grid;
            GenerateCollider();
            _gridCreated = true;
        }


        /// <summary>
        /// コライダーの生成を担当する
        /// </summary>
        private void GenerateCollider()
        {
            for (int z = 0; z < _gridSize; z++)
            {
                for (int y = 0; y < _height; y++)
                {
                    for (int x = 0; x < _gridSize; x++)
                    {
                        if ((DUlongGrid[x, y] & (_oneDUlong << z)) != new DUlong(0, 0))
                        {
                            Instantiate(_gridCollider, new Vector3(x, y, z), Quaternion.identity).transform
                                .SetParent(transform);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// グリッドを埋めて保存することを担当する
        /// </summary>
        /// <returns></returns>
        private DUlong[,] FillGridDUlongBase(DUlong[,] grid, List<PutUnitData> unitDataList)
        {
            foreach (var putUnitData in unitDataList)
            {
                UnitData unit = _allUnitData.UnitTypeArray[(int)putUnitData.UnitType].AllUnit[putUnitData.UnitId];
                ulong rotateShape = 0;

                switch (putUnitData.Direction)
                {
                    case UnitRotate.Default:
                        rotateShape = unit.UnitShape;
                        break;
                    case UnitRotate.Right90:
                        rotateShape = RotateRightUlongBase90(unit.UnitShape);
                        break;
                    case UnitRotate.Right180:
                        rotateShape = RotateRightUlongBase180(unit.UnitShape);
                        break;
                    case UnitRotate.Right270:
                        rotateShape = RotateRightUlongBase270(unit.UnitShape);
                        break;
                }

                for (int y = 0; y < Edge; y++)
                {
                    for (int z = 0; z < Edge; z++)
                    {
                        for (int x = 0; x < Edge; x++)
                        {
                            int bitPosition = x + z * Edge + y * 16;
                            if ((rotateShape & ((ulong)1 << bitPosition)) != 0)
                            {
                                int gridZ = putUnitData.Position.y + z;
                                grid[putUnitData.Position.x + x, y] |= _oneDUlong << gridZ;
                            }
                        }
                    }
                }
            }

            return grid;
        }

        /// <summary>
        /// 壁オブジェクトのインスタンス生成を担当する
        /// </summary>
        private void GenerateWall()
        {
            GameObject[] walls = new GameObject[Edge];
            for (int i = 0; i < Edge; i++)
            {
                walls[i] = Instantiate(_wallData.wallPrefab, _wallData.Position + _wallOffset, Quaternion.identity);
                walls[i].name = "Wall" + i;
            }

            WallGenerator.GenerateWalls(_wallData, walls);
        }

        /// <summary>
        /// ulong型で保存されるユニットの形状をｙ軸ベースで90度回転させる
        /// </summary>
        /// <param name="shape"></param>
        /// <returns></returns>
        private ulong RotateRightUlongBase90(ulong shape)
        {
            ulong returnShape = 0;
            for (int y = 0; y < Edge; y++)
            {
                for (int z = 0; z < Edge; z++)
                {
                    for (int x = 0; x < Edge; x++)
                    {
                        int baseBit = x + z * Edge + y * 16;
                        //回転させない場合はx + z * Edge + y * 16 でビットの位置が決まる
                        //回転後のbitの位置は座標にしてx = z 、y = y、z = 3 - xで求められる。
                        if (((shape >> baseBit) & 1UL) != 0)
                        {
                            int bitPos = z + (3 - x) * Edge + (y * 16);
                            returnShape |= (ulong)1 << bitPos;
                        }
                    }
                }
            }

            return returnShape;
        }

        /// <summary>
        /// ulong型で保存されるユニットの形状をy軸ベースで90度回転させる　
        /// </summary>
        /// <param name="shape"></param>
        /// <returns></returns>
        private ulong RotateRightUlongBase180(ulong shape)
        {
            ulong returnShape = 0;
            for (int y = 0; y < Edge; y++)
            {
                for (int z = 0; z < Edge; z++)
                {
                    for (int x = 0; x < Edge; x++)
                    {
                        int baseBit = x + z * Edge + y * 16;
                        if (((shape >> baseBit) & 1UL) != 0)
                        {
                            int bitPos = (3 - z) + (3 - x) * Edge + (y * 16);
                            returnShape |= (ulong)1 << bitPos;
                        }
                    }
                }
            }

            return returnShape;
        }

        private ulong RotateRightUlongBase270(ulong shape)
        {
            ulong returnShape = 0;
            for (int y = 0; y < Edge; y++)
            {
                for (int z = 0; z < Edge; z++)
                {
                    for (int x = 0; x < Edge; x++)
                    {
                        int baseBit = x + z * Edge + y * 16;
                        if (((shape >> baseBit) & 1UL) != 0)
                        {
                            int bitPos = (3 - z) + (x * Edge) + (y * 16);
                            returnShape |= (ulong)1 << bitPos;
                        }
                    }
                }
            }

            return returnShape;
        }

        private void OnDrawGizmos()
        {
            if (!_gridCreated) return;

            Gizmos.color = Color.green;
            for (int z = 0; z < _gridSize; z++)
            {
                for (int y = 0; y < _height; y++)
                {
                    for (int x = 0; x < _gridSize; x++)
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
        public void Initialize()
        {
            Debug.Log("GridManager initialized");
            GenerateTestData();
            _ = GridManagerInitialize(PutUnitDataList);
        }

        void IManager.Register()
        {
            DiContainer.Instance.Register(this);
        }

        private void Awake()
        {
            DiContainer.Instance.Register(this);
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

        public UnitType UnitType;
        public Vector2Int Position; // X, Z座標を表す
        public UnitRotate Direction;
    }

    [Serializable]
    public struct UnitData
    {
        public ulong UnitShape;
        public UnitType UnitType;
        public Vector2Int[] EnterPositions;
        public Vector2Int[] ExitPositions;
    }

    [Serializable]
    public struct WallData
    {
        /// <summary>
        /// 壁の最も原点に近い部分の座標
        /// </summary>
        public GameObject wallPrefab;

        [Tooltip("壁に囲まれた空間の中心座標")] public Vector3Int Position;
        [Tooltip("壁の高さ")] public int Height;
        [Tooltip("壁の厚み 必ず奇数にしてください")] public int Width;
        [Tooltip("壁の外側の長さ")] public int Size;
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