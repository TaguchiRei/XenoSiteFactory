using Manager;
using UnityEngine;

namespace StaticObject
{
    public static class WallGenerator
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public static void GenerateWalls(WallData data, GameObject[] walls)
        {
            var centerPos = data.Position;
            var size = data.Size;
            var width = data.Width;
            var height = data.Height;
            
            walls[0].transform.position += Vector3.back * size * 0.5f;
            walls[1].transform.position += Vector3.forward * size * 0.5f;
            walls[2].transform.position += Vector3.right * size * 0.5f;
            walls[3].transform.position += Vector3.left * size * 0.5f;
        }
    }
}