using System.Collections.Generic;
using Manager;
using UnityEngine;

namespace StaticObject
{
    public static class WallGenerator
    {
        /// <summary>
        /// 壁を生成する
        /// </summary>
        /// <param name="data"></param>
        /// <param name="walls"></param>
        public static void GenerateWalls(WallData data, GameObject[] walls)
        {
            var size = data.Size;
            var width = data.Width;
            var height = data.Height;

            var wallLength = (size - width);
            
            //中心から一定の値ずらす
            walls[0].transform.position += Vector3.back * wallLength * 0.5f - Vector3.right * width * 0.5f;
            walls[1].transform.position += Vector3.forward * wallLength * 0.5f + Vector3.right * width * 0.5f;
            walls[2].transform.position += Vector3.right * wallLength * 0.5f - Vector3.forward * width * 0.5f;
            walls[3].transform.position += Vector3.left * wallLength * 0.5f + Vector3.forward * width * 0.5f;
            
            //スケールを調整
            walls[0].transform.localScale = new Vector3(size - width, height, width);
            walls[1].transform.localScale = new Vector3(size - width, height, width);
            walls[2].transform.localScale = new Vector3(width, height, size - width);
            walls[3].transform.localScale = new Vector3(width, height, size - width);
        }
        
        /// <summary>
        /// 壁のインデックス(x,z)を取得する非同期メソッド
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Awaitable<Vector2Int[]> GetWallIndices(WallData data)
        {
            await Awaitable.BackgroundThreadAsync();
            int halfSize = data.Size / 2;
            int wallLength = data.Size - data.Width;
            int halfLength = wallLength / 2;

            Vector3Int center = data.Position;
            List<Vector2Int> indices = new();

            // 手前の壁を保存
            for (int x = center.x - halfLength; x <= center.x + halfLength; x++)
            for (int z = center.z - halfSize; z < center.z - halfSize + data.Width; z++)
                indices.Add(new Vector2Int(x, z));

            // 前方の壁を保存
            for (int x = center.x - halfLength; x <= center.x + halfLength; x++)
            for (int z = center.z + halfSize - data.Width + 1; z <= center.z + halfSize; z++)
                indices.Add(new Vector2Int(x, z));

            // 右の壁を保存
            for (int x = center.x + halfSize - data.Width + 1; x <= center.x + halfSize; x++)
            for (int z = center.z - halfLength; z <= center.z + halfLength; z++)
                indices.Add(new Vector2Int(x, z));

            // 左の壁を保存
            for (int x = center.x - halfSize; x < center.x - halfSize + data.Width; x++)
            for (int z = center.z - halfLength; z <= center.z + halfLength; z++)
                indices.Add(new Vector2Int(x, z));
            await Awaitable.MainThreadAsync();

            return indices.ToArray();
        }
    }
}