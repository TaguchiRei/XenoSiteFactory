using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DIContainer;
using GamesKeystoneFramework.KeyDebug.KeyLog;
using GamesKeystoneFramework.KeyMathBit;
using Interface;
using UnityEngine;
using XenoScriptableObject;
using Random = UnityEngine.Random;

namespace Manager
{
    public class GridManager : MonoBehaviour, IManager
    {
        [SerializeField, Range(20, 128)] private int _gridSize = 20;
        [SerializeField, Range(4, 10)] private int _height;
        [SerializeField] private AllUnitData _allUnitData;
        [SerializeField] private GameObject _gridCollider;
        
        /// <summary>
        /// グリッドが占有されているエリアを保存する
        /// </summary>
        public DUlong[,] DUlongGrid { get; private set; }

        /// <summary>
        /// グリッドに設置されている物を保存する
        /// </summary>
        public List<PutUnitData> PutUnitDataList { get; private set; }

        private bool _gridCreated;

        private readonly DUlong _oneDUlong = new(0, 1);


        private void Start()
        {
            //テスト用スクリプト
            PutUnitDataList = new List<PutUnitData>();
            for (int i = 0; i < 10; i++)
            {
                var id = 0;
                PutUnitDataList.Add(new PutUnitData()
                {
                    UnitId = 0,
                    UnitType = _allUnitData.UnitTypeArray[0].AllUnit[id].UnitType,
                    Position = new Vector2Int(Random.Range(0, _gridSize - 4), Random.Range(0, _gridSize - 4)),
                    Direction = 0
                });
                KeyLogger.Log($"ID{id}  UnitPosition{PutUnitDataList[i].Position}");
            }

            Initialize();
        }

        private async UniTask GenerateCollider()
        {
            DUlongGrid = await FillGridDUlongBase();
            _gridCreated = true;
            
            for (int z = 0; z < _gridSize; z++)
            {
                for (int y = 0; y < _height; y++)
                {
                    for (int x = 0; x < _gridSize; x++)
                    {
                        if ((DUlongGrid[x, y] & (_oneDUlong << z)) != new DUlong(0, 0))
                        {
                            Instantiate(_gridCollider, new Vector3(x, y, z), Quaternion.identity);
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
            _ = GenerateCollider();
        }
        

        private async Awaitable<DUlong[,]> FillGridDUlongBase()
        {
            await Awaitable.BackgroundThreadAsync();
            DUlong[,] grid = new DUlong[_gridSize, _height];

            foreach (var putUnitData in PutUnitDataList)
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

                for (int y = 0; y < 4; y++)
                {
                    for (int z = 0; z < 4; z++)
                    {
                        for (int x = 0; x < 4; x++)
                        {
                            int bitPosition = x + z * 4 + y * 16;
                            int gridZ = putUnitData.Position.y + z;
                            if ((rotateShape & ((ulong)1 << bitPosition)) != 0)
                            {
                                grid[putUnitData.Position.x + x, y] |= _oneDUlong << gridZ;
                            }
                        }
                    }
                }
            }
            await Awaitable.MainThreadAsync();
            return grid;
        }
        

        /// <summary>
        /// ulong型で保存されるユニットの形状をｙ軸ベースで90度回転させる
        /// </summary>
        /// <param name="shape"></param>
        /// <returns></returns>
        private ulong RotateRightUlongBase90(ulong shape)
        {
            ulong returnShape = 0;
            for (int y = 0; y < 4; y++)
            {
                for (int z = 0; z < 4; z++)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        int baseBit = x + z * 4 + y * 16;
                        //回転させない場合はx + z * 4 + y * 16 でビットの位置が決まる
                        //回転後のbitの位置は座標にしてx = z 、y = y、z = 3 - xで求められる。
                        if (((shape >> baseBit) & 1UL) != 0)
                        {
                            int bitPos = z + (3 - x) * 4 + (y * 16);
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
            for (int y = 0; y < 4; y++)
            {
                for (int z = 0; z < 4; z++)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        int baseBit = x + z * 4 + y * 16;
                        if (((shape >> baseBit) & 1UL) != 0)
                        {
                            int bitPos = (3 - z) + (3 - x) * 4 + (y * 16);
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
            for (int y = 0; y < 4; y++)
            {
                for (int z = 0; z < 4; z++)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        int baseBit = x + z * 4 + y * 16;
                        if (((shape >> baseBit) & 1UL) != 0)
                        {
                            int bitPos = (3 - z) + (x * 4) + (y * 16);
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

        void IManager.Register()
        {
            DiContainer.Instance.Register(this);
        }

        void OnEnable()
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