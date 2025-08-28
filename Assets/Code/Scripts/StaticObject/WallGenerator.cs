using System.Collections.Generic;
using Manager;
using UnitInfo;
using Unity.VisualScripting;
using UnityEngine;

namespace StaticObject
{
    public static class WallGenerator
    {
        private static Vector3 wallOffset = new(0.5f, 0, 0.5f);

        /// <summary>
        /// 壁を生成する
        /// </summary>
        /// <param name="data"></param>
        /// <param name="walls"></param>
        private static void GenerateWallObjects(WallData data, GameObject[] walls)
        {
            var size = data.Size;
            var width = data.Width;
            var height = data.Height;

            var wallLength = (size - width);

            //中心から一定の値ずらす
            walls[0].transform.position += Vector3.back * wallLength * 0.5f - Vector3.right * width * 0.5f - wallOffset;
            walls[1].transform.position += Vector3.forward * wallLength * 0.5f + Vector3.right * width * 0.5f - wallOffset;
            walls[2].transform.position += Vector3.right * wallLength * 0.5f - Vector3.forward * width * 0.5f - wallOffset;
            walls[3].transform.position += Vector3.left * wallLength * 0.5f + Vector3.forward * width * 0.5f - wallOffset;

            //スケールを調整
            walls[0].transform.localScale = new Vector3(size - width, height, width);
            walls[1].transform.localScale = new Vector3(size - width, height, width);
            walls[2].transform.localScale = new Vector3(width, height, size - width);
            walls[3].transform.localScale = new Vector3(width, height, size - width);
        }

        /// <summary>
        /// 壁のインデックス(x,z)を取得するメソッド
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Vector2Int[] GetWallIndex(WallData data)
        {
            var centerPos = data.Position;
            var mostOutsideIndex = data.Size / 2;//centerPosの値に足すと最も外側に近い部分の座標になる
            var mostCenterIndex = mostOutsideIndex - data.Width;//centerPosの値に足すと最も内側に近い部分の座標になる
            List<Vector2Int> wallIndex = new List<Vector2Int>();

            //左右の壁の取得を行う
            for (int z = centerPos.z - mostOutsideIndex; z < centerPos.z + mostCenterIndex; z++)
            {
                //右側の壁の取得
                for (int x = centerPos.x + mostCenterIndex; x < centerPos.x + mostOutsideIndex; x++)
                {
                    wallIndex.Add(new Vector2Int(x, z));
                }
                //左側の壁の取得
                var zAndWidth = z + data.Width;
                for (int x = centerPos.x - mostOutsideIndex; x < centerPos.x - mostCenterIndex; x++)
                {
                    wallIndex.Add(new Vector2Int(x, zAndWidth));
                }
            }
            //前後の壁の取得を行う
            for (int x = centerPos.x - mostOutsideIndex; x < centerPos.x + mostCenterIndex; x++)
            {
                //手前の壁の取得
                for (int z = centerPos.z - mostOutsideIndex; z < centerPos.z - mostCenterIndex; z++)
                {
                    wallIndex.Add(new Vector2Int(x, z));
                }
                //奥の壁の取得
                var xAndWidth = x + data.Width;
                for (int z = centerPos.z + mostCenterIndex; z < centerPos.z + mostOutsideIndex; z++)
                {
                    wallIndex.Add(new Vector2Int(xAndWidth, z));
                }
            }
            
            return wallIndex.ToArray();
        }
        /// <summary>
        /// 壁オブジェクトのインスタンス生成を担当する
        /// </summary>
        public static void GenerateWall(WallData wallData)
        {
            //壁は四方を囲んでいるため４
            GameObject[] walls = new GameObject[4];
            for (int i = 0; i < 4; i++)
            {
                walls[i] = Object.Instantiate(wallData.wallPrefab, wallData.Position + Vector3.down * 0.5f, Quaternion.identity);
                walls[i].name = "Wall" + i;
            }
            GenerateWallObjects(wallData, walls);
        }
    }
}