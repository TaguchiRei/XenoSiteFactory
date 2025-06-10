using System;
using Manager;
using UnityEngine;
using UnityEngine.Serialization;

namespace XenoScriptableObject
{
    [CreateAssetMenu(menuName = "ScriptableObject/AllUnitData")]
    public class AllUnitData : ScriptableObject
    {
        [FormerlySerializedAs("AllUnitArray")] public UnitDataArray[] UnitTypeArray;
    }

    [Serializable]
    public class UnitDataArray
    {
        public UnitData[] AllUnit;
    }
}
