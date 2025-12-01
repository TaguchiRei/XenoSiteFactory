using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "ScriptableObjects/UnitData")]
public class UnitDatas : ScriptableObject
{
    public List<UnitData> AllUnitData = new();
}

[Serializable]
public class UnitData
{
    public string Name;
    public string Info;
    public Texture2D UnitTexture;
    public GameObject UnitPrefab;
    public Vector3Int[] InputPorts;
    public Vector3Int[] OutputPorts;
    public bool[] UnitShape;
    public int UnitWidth;
    public int UniDepth;
    public int UnitHeight;
}