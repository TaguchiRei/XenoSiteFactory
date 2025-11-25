namespace Code.Scripts.Runtime.Entity.Grid
{
    public class MapData
    {
        private int _height;
        private int _width;
        private int _depth;

        private UnitData[] _placedItemData;

        public MapData(int height, int width, int depth)
        {
            _height = height;
            _width = width;
            _depth = depth;
        }
    }
}