using UnityEngine;
using System;
using System.Collections.Generic;

using UnitInfo;

namespace XenositeFramework.Editor
{
[CreateAssetMenu(menuName = "XenositeFramework/Editor/PlayerDataTestData")]    public class PlayerDataTestData : ScriptableSaveData
    {
        public string PlayerName;
        public int Days;
        public int Unit;
        public int Xenosite;
        public int Items;
        public int Money;
        public List<PutUnitData> _putUnitDataList;
    }
}
