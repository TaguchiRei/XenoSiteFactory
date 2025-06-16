using System;
using Manager;
using UnityEngine;

namespace XenoScriptableObject
{
    [CreateAssetMenu(menuName = "ScriptableObject/AllUnitData")]
    public class AllUnitData : ScriptableObject
    { 
        public UnitDataArray[] UnitTypeArray;
    }

    [Serializable]
    public class UnitDataArray
    {
        public UnitData[] AllUnit;
    }
}
