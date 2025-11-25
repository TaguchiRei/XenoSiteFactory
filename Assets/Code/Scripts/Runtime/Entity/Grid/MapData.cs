using System.Collections.Generic;
using UnityEngine;

namespace Code.Scripts.Runtime.Entity.Grid
{
    public class MapData
    {
        private int _height;
        private int _width;
        private int _depth;

        private List<UnitData> _placedUnitData;

        public MapData(int height, int width, int depth)
        {
            _height = height;
            _width = width;
            _depth = depth;
            _placedUnitData = new();
        }

        public MapData(int height, int width, int depth, List<UnitData> placedUnitData)
        {
            _height = height;
            _width = width;
            _depth = depth;
            _placedUnitData = placedUnitData;
        }
        
        private bool CheckCanPut(UnitData unitData, Vector3Int position)
        {
            if (position.x + unitData.UnitWidth > _width ||
                position.y + unitData.UnitHeight > _height ||
                position.z + unitData.UnitDepth > _depth ||
                position.x < 0 || position.y < 0 || position.z < 0) return false;

            foreach (var placedUnit in _placedUnitData)
            {
                if (!IsBoundingBoxOverlap(position, unitData, placedUnit))
                {
                    continue;
                }

                for (int x = 0; x < unitData.UnitWidth; x++)
                {
                    for (int y = 0; y < unitData.UnitHeight; y++)
                    {
                        for (int z = 0; z < unitData.UnitDepth; z++)
                        {
                            if (!unitData.UnitShape[x, y, z]) continue;

                            if (placedUnit.HasBlockAt(new(x, y, z))) return false;
                        }
                    }
                }
            }

            return true;
        }
        
        /// <summary>
        /// AABBを取り入れて検索を効率化する
        /// </summary>
        private bool IsBoundingBoxOverlap(Vector3Int pos, UnitData unit, UnitData placedUnit)
        {
            // X軸で重なっているか
            bool overlapX = pos.x < placedUnit.Position.x + placedUnit.UnitWidth &&
                            pos.x + unit.UnitWidth > placedUnit.Position.x;

            // Y軸で重なっているか
            bool overlapY = pos.y < placedUnit.Position.y + placedUnit.UnitHeight &&
                            pos.y + unit.UnitHeight > placedUnit.Position.y;

            // Z軸で重なっているか
            bool overlapZ = pos.z < placedUnit.Position.z + placedUnit.UnitDepth &&
                            pos.z + unit.UnitDepth > placedUnit.Position.z;

            // 3軸すべてで重なっていれば衝突の可能性あり
            return overlapX && overlapY && overlapZ;
        }
    }
}