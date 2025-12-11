using Code.Scripts.Runtime.Entity.Grid;
using UnityEngine;

public class UnitPlacementUseCase
{
    public bool TryPutUnit(UnitDto unit, MapEntity map)
    {
        var unitShape = UnitShapeRestore(unit);

        for (int x = 0; x < unit.UnitWidth; x++)
        {
            for (int y = 0; y < unit.UnitHeight; y++)
            {
                for (int z = 0; z < unit.UnitDepth; z++)
                {
                    if (!unitShape[x, y, z]) continue;
                }
            }
        }

        return true;
    }

    public bool TryRemoveUnit()
    {
        return true;
    }


    private bool[,,] UnitShapeRestore(UnitDto unit)
    {
        bool[,,] unitShape = new bool[unit.UnitWidth, unit.UnitHeight, unit.UnitDepth];
        for (int i = 0; i < unit.UnitShape.Length; i++)
        {
            //元のインデックスはindex = x + width * (y + height * z)で加工される
            int x = i % unit.UnitWidth;
            int y = i % (unit.UnitWidth * unit.UnitHeight) / unit.UnitWidth;
            int z = i / (unit.UnitWidth * unit.UnitHeight);
            unitShape[x, y, z] = unit.UnitShape[i];
        }

        return unitShape;
    }
}