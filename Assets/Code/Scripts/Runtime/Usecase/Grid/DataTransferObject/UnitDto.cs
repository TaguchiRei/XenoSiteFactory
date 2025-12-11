using UnityEngine;

public class UnitDto
{
    public string Name { get; private set; }
    public string Info { get; private set; }
    public string UnitTextureKey { get; private set; }
    public string UnitPrefabKey { get; private set; }
    public Vector3Int[] InputPorts { get; private set; }
    public Vector3Int[] OutputPorts { get; private set; }
    public bool[] UnitShape { get; private set; }
    public int UnitWidth { get; private set; }
    public int UnitDepth { get; private set; }
    public int UnitHeight { get; private set; }

    public UnitDto(
        string name,
        string info,
        string unitTextureKey,
        string unitPrefabKey,
        Vector3Int[] inputPorts,
        Vector3Int[] outputPorts,
        bool[] unitShape,
        int unitWidth,
        int unitDepth,
        int unitHeight)
    {
        Name = name;
        Info = info;
        UnitTextureKey = unitTextureKey;
        UnitPrefabKey = unitPrefabKey;
        InputPorts = inputPorts;
        OutputPorts = outputPorts;
        UnitShape = unitShape;
        UnitWidth = unitWidth;
        UnitDepth = unitDepth;
        UnitHeight = unitHeight;
    }
}