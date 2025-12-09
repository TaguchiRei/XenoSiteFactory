using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "UnitData", menuName = "ScriptableObjects/UnitData")]
public class UnitDatas : ScriptableObject
{
    public UnitData[] Manufactures;

    public UnitData[] Weapons;

    public UnitData[] Xenosites;
}

[Serializable]
public class UnitData
{
    public string Name;
    public string Info;
    public AssetReferenceTexture2D UnitTexture;
    public AssetReferenceGameObject UnitPrefab;
    public Vector3Int[] InputPorts;
    public Vector3Int[] OutputPorts;
    public bool[] UnitShape;
    public int UnitWidth;
    public int UnitDepth;
    public int UnitHeight;
}