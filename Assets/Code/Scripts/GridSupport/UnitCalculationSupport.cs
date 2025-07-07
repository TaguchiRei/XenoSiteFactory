using System;
using StaticObject;
using UnityEngine;

namespace GridSupport
{
    public static class UnitCalculationSupport
    {
        /// <summary>
        /// ３重のforループの計算をするメソッド。
        /// xz平面上に高さyがあるという計算なので内部のループ順がYZXとなっている。
        /// 辺の長さ(edge)をもとにループする
        /// </summary>
        /// <param name="action">引数はｘｙｚの順</param>
        public static void CalculateUnits(Action<int,int,int> action)
        {
            var edge = BitShapeSupporter.GetEdge();
            for (int y = 0; y < edge; y++)
            {
                for (int z = 0; z < edge; z++)
                {
                    for (int x = 0; x < edge; x++)
                    {
                        action(x, y, z);
                    }
                }
            }
        }

        /// <summary>
        /// forの三十ループの計算をする
        /// xz平面上に高さyがあるという計算なので内部のループ順が
        /// YZXとなっている。
        /// 入力した値に基づきループする
        /// </summary>
        /// <param name="i">x軸と対応</param>
        /// <param name="j">y軸と対応</param>
        /// <param name="k">z軸と対応</param>
        /// <param name="action"></param>
        public static void CalculateUnits(int i, int j, int k,Action<int, int ,int> action)
        {
            for (int y = 0; y < j; y++)
            {
                for (int z = 0; z < k; z++)
                {
                    for (int x = 0; x < i; x++)
                    {
                        action(x, y, z);
                    }
                }
            }
        }
    }
}
