using Manager;
using UnityEngine;

namespace XenoScriptableObject
{
    [CreateAssetMenu(menuName = "ScriptableObject/AllUnitData")]
    public class AllUnitData : UnityEngine.ScriptableObject
    {
        public UnitData[][] AllUnit;
    }
}
