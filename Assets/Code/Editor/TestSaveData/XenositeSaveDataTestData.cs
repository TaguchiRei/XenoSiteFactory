using UnityEngine;
using System;
using System.Collections.Generic;

using GridSystem;
using PlayerSystem;

namespace XenositeFramework.Editor
{
[CreateAssetMenu(menuName = "ScriptableObject/XenositeSaveDataTestData")] public class XenositeSaveDataTestData : ScriptableSaveData
 {
 public PlayerData PlayerData;
 public PlacedObjectData PlacedObjectData;
 }
}
