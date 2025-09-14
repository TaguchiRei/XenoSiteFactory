using System;
using UnitInfo;
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
