using UnityEngine;

public interface IUnitCalculation
{
    /// <summary>
    /// 計算結果をもとに次のアイテムを返す
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public ItemEntity CalculateItem(ItemEntity[] item);
}