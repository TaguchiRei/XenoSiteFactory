using DIContainer;
using GamesKeystoneFramework.KeyDebug.KeyLog;
using Interface;
using UnityEngine;
using XenoScriptableObject;

namespace Manager
{
    public class UnitResourceManager : MonoBehaviour,IManager
    {
        private byte[] _numbersOfUnits;
        private GameObject[] _units;
        private AllUnitData _allUnits;
        public void Register()
        {
            DiContainer.Instance.Register(this);
        }

        public void Awake()
        {
            Register();
        }

        public void Initialize()
        {
            DiContainer.Instance.TryGetScriptableObject(out _allUnits);
            foreach (var unitType in _allUnits.UnitTypeArray)
            {
                foreach (var unit in unitType.AllUnit)
                {
                    if (unit.UnitObject == null)
                    {
                        KeyLogger.LogWarning("unit object is null");
                    }
                    Instantiate(unit.UnitObject);
                }
            }
        }

        /// <summary>
        /// リソースを追加するためのメソッド
        /// </summary>
        /// <param name="id"></param>
        /// <param name="amount"></param>
        public void AddResource(int id, int amount)
        {
            
        }

        /// <summary>
        /// リソースを消費するためのメソッド
        /// </summary>
        /// <param name="id"></param>
        /// <param name="amount"></param>
        public void RemoveResource(int id, int amount)
        {
            
        }

        public GridManager.UnitData GetUnitData(GridManager.UnitType unitType, int id)
        {
            Debug.Log(_allUnits == null);
            return _allUnits.UnitTypeArray[(byte) unitType].AllUnit[id];
        }
    }
}
