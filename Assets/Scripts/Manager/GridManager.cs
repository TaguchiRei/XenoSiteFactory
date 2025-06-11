using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GamesKeystoneFramework.KeyDebug.KeyLog;
using GamesKeystoneFramework.KeyMathBit;
using Interface;
using Unity.VisualScripting;
using UnityEngine;
using XenoScriptableObject;
using Random = UnityEngine.Random;

namespace Manager
{
    public class GridManager : MonoBehaviour, IManager
    {
        [SerializeField, Range(20, 200)] private int _gridSize = 20;
        [SerializeField, Range(4, 10)] private int _height;
        [SerializeField] private AllUnitData _allUnitData;

        /// <summary>
        /// グリッドが占有されているかどうかを示す情報
        /// </summary>
        public bool[,,] Grid { get; private set; }

        public DUlong[,] DUlongGrid { get; private set; }

        /// <summary>
        /// グリッドに設置されている物を保存する
        /// </summary>
        public List<PutUnitData> PutUnitDataList { get; private set; }

        private bool _gridCreated;

        private readonly DUlong _oneDUlong = new DUlong(0, 1);


        private void Start()
        {
            //テスト用スクリプト
            PutUnitDataList = new List<PutUnitData>();
            for (int i = 0; i < 10; i++)
            {
                var id = Random.Range(0, 5);
                PutUnitDataList.Add(new PutUnitData()
                {
                    UnitId = Random.Range(0, 5),
                    UnitType = _allUnitData.UnitTypeArray[0].AllUnit[id].UnitType,
                    Position = new Vector2Int(Random.Range(0, _gridSize - 4), Random.Range(0, _gridSize - 4)),
                    Direction = (UnitRotate)Random.Range(0, 4)
                });
                KeyLogger.Log($"ID{id}  UnitPosition{PutUnitDataList[i].Position}");
            }

            Initialize();
        }

        private async UniTask MakeWait()
        {
            var result1 = await FillGridDUlongBase();
            var result2 = await FillGridBoolBase();
            DUlongGrid = result1;
            Grid = result2;
            _gridCreated = true;
        }

        /// <summary>
        /// 初期化時に呼び出される
        /// </summary>
        public void Initialize()
        {
            Grid = new bool[_gridSize, _height, _gridSize];
            _ = MakeWait();
        }

        /// <summary>
        /// グリッドを設置済みユニットデータを利用して埋める
        /// </summary>
        private async Awaitable<bool[,,]> FillGridBoolBase()
        {
            await Awaitable.BackgroundThreadAsync();
            bool[,,] grid = new bool[_gridSize, _height, _gridSize];

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

                //グリッドに反映
                for (int y = 0; y < 4; y++)
                {
                    for (int z = 0; z < 4; z++)
                    {
                        for (int x = 0; x < 4; x++)
                        {
                            var bitPos = x + z * 4 + y * 16;
                            grid[putUnitData.Position.x + x, y, putUnitData.Position.y + z] =
                                (rotateShape & ((ulong)1 << bitPos)) != 0;
                        }
                    }
                }
            }

            await Awaitable.MainThreadAsync();
            return grid;
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
                            if ((rotateShape & ((ulong)1 << bitPosition)) != 0)
                            {
                                grid[putUnitData.Position.x + x, y] |= _oneDUlong << bitPosition;
                            }
                        }
                    }
                }
            }

            await Awaitable.MainThreadAsync();
            return grid;
        }

        /// <summary>
        /// ulongの値をboolの配列に変換
        /// </summary>
        /// <param name="shape">形を示すulongを入れる</param>
        /// <returns></returns>
        private bool[,,] GetUnitShape(ulong shape)
        {
            var unitShape = new bool[4, 4, 4];
            for (int y = 1; y <= 4; y++)
            {
                for (int z = 1; z <= 4; z++)
                {
                    for (int x = 1; x <= 4; x++)
                    {
                        int bitPosition = (x - 1) + (z - 1) * 4 + (y - 1) * 16;
                        if ((shape & ((ulong)1 << bitPosition)) != 0)
                        {
                            unitShape[x - 1, y - 1, z - 1] = true;
                        }
                    }
                }
            }

            return unitShape;
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

        /// <summary>
        /// boolの配列を90度右回転させる。
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        private bool[,,] RotateRightBoolBase90(bool[,,] matrix)
        {
            bool[,,] result = new bool[4, 4, 4];
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    for (int z = 0; z < 4; z++)
                    {
                        result[z, y, 3 - x] = matrix[x, y, z];
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///　boolの配列を180度回転させる
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        private bool[,,] RotateRightBoolBase180(bool[,,] matrix)
        {
            bool[,,] result = new bool[4, 4, 4];
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    for (int z = 0; z < 4; z++)
                    {
                        result[3 - x, y, 3 - z] = matrix[x, y, z];
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// boolの配列を270度回転させる
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        private bool[,,] RotateRightBoolBase270(bool[,,] matrix)
        {
            bool[,,] result = new bool[4, 4, 4];
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    for (int z = 0; z < 4; z++)
                    {
                        result[3 - z, y, x] = matrix[x, y, z];
                    }
                }
            }

            return result;
        }

        private void OnDrawGizmos()
        {
            if (Grid == null || !_gridCreated) return;
            Gizmos.color = Color.red;
            for (int z = 0; z < _gridSize; z++)
            {
                for (int y = 0; y < _height; y++)
                {
                    for (int x = 0; x < _gridSize; x++)
                    {
                        if (Grid[x, y, z])
                        {
                            Gizmos.DrawWireCube(new Vector3(x, y, z), Vector3.one);
                        }
                    }
                }
            }

            Gizmos.color = Color.green;
            for (int z = 0; z < _gridSize; z++)
            {
                for (int y = 0; y < _height; y++)
                {
                    for (int x = 0; x < _gridSize; x++)
                    {
                        if ((DUlongGrid[x, y] & (_oneDUlong << z)) == _oneDUlong)
                        {
                            Gizmos.DrawWireCube(new Vector3(x, y, z), Vector3.one);
                        }
                    }
                }
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