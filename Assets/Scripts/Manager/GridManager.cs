using System;
using System.Collections.Generic;
using GamesKeystoneFramework.KeyDebug.KeyLog;
using Interface;
using UnityEngine;
using UnityEngine.Serialization;
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

        /// <summary>
        /// グリッドに設置されている物を保存する
        /// </summary>
        public List<PutUnitData> PutUnitDataList { get; private set; }

        private Awaitable _awaitable;

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
                    UnitType =  _allUnitData.UnitTypeArray[0].AllUnit[id].UnitType,
                    Position = new Vector2Int(Random.Range(0, _gridSize - 4), Random.Range(0, _gridSize - 4)),
                    Direction = (UnitRotate)Random.Range(0,4)
                });
                KeyLogger.Log($"ID{id}  UnitPosition{PutUnitDataList[i].Position}");
            }
            Initialize();
        }

        /// <summary>
        /// 初期化時に呼び出される
        /// </summary>
        public void Initialize()
        {
            Grid = new bool[_gridSize, _height, _gridSize];
            _awaitable = FillGrid();
        }

        /// <summary>
        /// グリッドを設置済みユニットデータを利用して埋める
        /// </summary>
        private async Awaitable FillGrid()
        {
            await Awaitable.BackgroundThreadAsync();

            foreach (var putUnitData in PutUnitDataList)
            {
                UnitData unit = _allUnitData.UnitTypeArray[(int)putUnitData.UnitType].AllUnit[putUnitData.UnitId];

                //boolの配列を回転
                var shape = GetUnitShape(unit.UnitShape);
                switch (putUnitData.Direction)
                {
                    case UnitRotate.Right90:
                        shape = RotateRight90(shape);
                        break;
                    case UnitRotate.Right180:
                        shape = RotateRight180(shape);
                        break;
                    case UnitRotate.Right270:
                        shape = RotateRight270(shape);
                        break;
                }

                for (int y = 0; y < 4; y++)
                {
                    for (int z = 0; z < 4; z++)
                    {
                        for (int x = 0; x < 4; x++)
                        {
                            Grid[x + putUnitData.Position.x, y, z + putUnitData.Position.y] = shape[x, y, z];
                        }
                    }
                }
            }
            await Awaitable.MainThreadAsync();
            Debug.Log("Grid Fill Success");
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
        /// boolの配列を90度右回転させる。
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        private bool[,,] RotateRight90(bool[,,] matrix)
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
        private bool[,,] RotateRight180(bool[,,] matrix)
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
        private bool[,,] RotateRight270(bool[,,] matrix)
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
            if (Grid == null || _awaitable == null || !_awaitable.IsCompleted) return;
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