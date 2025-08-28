using System;
using GamesKeystoneFramework.KeyDebug.KeyLog;
using Service;
using StaticObject;
using UnitInfo;
using UnityEngine;
using XenoScriptableObject;

namespace Manager
{
    public class UnitResourceManager : ManagerBase<UnitResourceManager>
    {
        private byte[][] _numbersOfUnits;
        private GameObject[] _units;
        private AllUnitData _allUnits;

        public void Awake()
        {
            Register();
        }

        public override void Initialize()
        {
            ServiceLocatorL.Instance.TryGetScriptableObject(out _allUnits);
            KeyLogger.LogWarning("初期化でテスト専用のデータを割り当てています。", this);
            _numbersOfUnits = new byte[_allUnits.UnitTypeArray.Length][];
            for (int i = 0; i < _allUnits.UnitTypeArray.Length; i++)
            {
                _numbersOfUnits[i] = new byte[_allUnits.UnitTypeArray[i].AllUnit.Length];
                for (int j = 0; j < _allUnits.UnitTypeArray[i].AllUnit.Length; j++)
                {
                    _numbersOfUnits[i][j] = Byte.MaxValue;
                }
            }
        }

        /// <summary>
        /// リソースを追加するためのメソッド
        /// </summary>
        /// <param name="unitType">ユニットの種類のenum</param>
        /// <param name="id">ユニットのID</param>
        /// <param name="amount">ユニットを追加する個数</param>
        public void AddResource(UnitType unitType, int id, byte amount)
        {
            if (_numbersOfUnits[(int)unitType][id] == byte.MaxValue)
            {
                return;
            }

            _numbersOfUnits[(int)unitType][id] += amount;
        }

        /// <summary>
        /// リソースを消費するためのメソッド
        /// </summary>
        /// <param name="unitType">ユニットの種類のenum</param>
        /// <param name="id">ユニットのID</param>
        /// <param name="amount">ユニットを消費する個数</param>
        public bool RemoveResource(UnitType unitType, int id, byte amount)
        {
            if (_numbersOfUnits[(int)unitType][id] > 0)
            {
                _numbersOfUnits[(int)unitType][id] =
                    (byte)Math.Clamp(_numbersOfUnits[(int)unitType][id] - amount, 0, byte.MaxValue);
                return true;
            }

            return false;
        }

        public UnitData GetUnitData(UnitType unitType, int id)
        {
            return _allUnits.UnitTypeArray[(byte)unitType].AllUnit[id];
        }
        
        /// <summary>
        /// 次に設置するユニットを変更する
        /// </summary>
        public void ChangeSettingUnit(int id)
        {
            UnitPutSupport.SetSelectUnitData(id,UnitType.Manufacture);
        }
    }
}