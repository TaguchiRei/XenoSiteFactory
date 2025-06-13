using Manager;
using UnityEngine;

namespace StaticObject
{
    public static class WallGenerator
    {
        public static void GenerateWalls(WallData data, GameObject[] walls)
        {
            var centerPos = data.Position;
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
    }
}