using System.Collections.Generic;
using UnityEngine;

namespace Code.Scripts.Runtime.Entity.Grid
{
    public class MapEntity
    {
        private int _height;
        private int _width;
        private int _depth;

        private List<UnitEntity> _placedUnitData;

        public MapEntity(int height, int width, int depth)
        {
            _height = height;
            _width = width;
            _depth = depth;
            _placedUnitData = new();
        }

        public MapEntity(int height, int width, int depth, List<UnitEntity> placedUnitData)
        {
            _height = height;
            _width = width;
            _depth = depth;
            _placedUnitData = placedUnitData;
        }

        public bool CheckCanPut(UnitEntity unitEntity, Vector3Int position)
        {
            if (position.x + unitEntity.UnitWidth > _width ||
                position.y + unitEntity.UnitHeight > _height ||
                position.z + unitEntity.UnitDepth > _depth ||
                position.x < 0 || position.y < 0 || position.z < 0) return false;

            foreach (var placedUnit in _placedUnitData)
            {
                if (!IsBoundingBoxOverlap(position, unitEntity, placedUnit))
                {
                    continue;
                }

                for (int x = 0; x < unitEntity.UnitWidth; x++)
                {
                    for (int y = 0; y < unitEntity.UnitHeight; y++)
                    {
                        for (int z = 0; z < unitEntity.UnitDepth; z++)
                        {
                            if (!unitEntity.UnitShape[x, y, z]) continue;

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
        /// <param name="pos"></param>
        /// <param name="unit"></param>
        /// <param name="placedUnit"></param>
        /// <returns></returns>
        private bool IsBoundingBoxOverlap(Vector3Int pos, UnitEntity unit, UnitEntity placedUnit)
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