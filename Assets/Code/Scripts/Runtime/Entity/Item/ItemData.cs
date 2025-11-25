using UnityEngine;

/// <summary>
/// 素材の基本的な情報
/// </summary>
public class ItemData
{
    public int ID { get; private set; }
    public int Rank { get; private set; }
    public ItemType Type { get; private set; }
}

/// <summary>
/// 素材の種類
/// </summary>
public enum ItemType : byte
{
    Wood = 1,
    Iron = 2,
    Crystal = 3
}