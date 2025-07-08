using GridSupport;

namespace StaticObject
{
    public static class BitShapeSupporter
    {
        /// <summary> Edgeは64ビットを4*4*4としてとらえたときの一辺を表す </summary>
        private const int Edge = 4;

        private const int LastEdgeIndex = Edge - 1;

        /// <summary>
        /// ulong型で保存されるユニットの形状をｙ軸ベースで90度回転させる
        /// </summary>
        /// <param name="shape"></param>
        /// <returns></returns>
        public static ulong RotateRightUlongBase90(ulong shape)
        {
            ulong returnShape = 0;

            for (int x = 0; x < Edge; x++)
            {
                for (int y = 0; y < Edge; y++)
                {
                    for (int z = 0; z < Edge; z++)
                    {
                        int baseBit = CalculationBitPosition(x, y, z);
                        if (((shape >> baseBit) & 1UL) != 0)
                        {
                            int bitPos = z + (LastEdgeIndex - x) * 4 + (y * Edge * Edge);
                            returnShape |= (ulong)1 << bitPos;
                        }
                    }
                }
            }

            return returnShape;
        }


        /// <summary>
        /// ulong型で保存されるユニットの形状をy軸ベースで90度回転させる　
        /// </summary>
        /// <param name="shape"></param>
        /// <returns></returns>
        public static ulong RotateRightUlongBase180(ulong shape)
        {
            ulong returnShape = 0;

            for (int x = 0; x < Edge; x++)
            {
                for (int y = 0; y < Edge; y++)
                {
                    for (int z = 0; z < Edge; z++)
                    {
                        int baseBit = CalculationBitPosition(x, y, z);
                        if (((shape >> baseBit) & 1UL) != 0)
                        {
                            int bitPos = (LastEdgeIndex - x) + (LastEdgeIndex - z) * Edge + (y * Edge * Edge);
                            returnShape |= (ulong)1 << bitPos;
                        }
                    }
                }
            }

            return returnShape;
        }

        /// <summary>
        /// ulong型で保存されるユニットの形状をy軸ベースで90度回転させる
        /// </summary>
        /// <param name="shape"></param>
        /// <returns></returns>
        public static ulong RotateRightUlongBase270(ulong shape)
        {
            ulong returnShape = 0;

            for (int x = 0; x < Edge; x++)
            {
                for (int y = 0; y < Edge; y++)
                {
                    for (int z = 0; z < Edge; z++)
                    {
                        int baseBit = CalculationBitPosition(x, y, z);
                        if (((shape >> baseBit) & 1UL) != 0)
                        {
                            int bitPos = (LastEdgeIndex - z) + (x * Edge) + (y * Edge * Edge);
                            returnShape |= (ulong)1 << bitPos;
                        }
                    }
                }
            }

            return returnShape;
        }

        /// <summary>
        /// ビット座標を計算して返す
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static int CalculationBitPosition(int x, int y, int z)
        {
            return x + z * Edge + y * Edge * Edge;
        }

        public static int GetEdge()
        {
            return Edge;
        }
    }
}