using UnityEngine;

/// <summary>
/// ユニットの処理に使うデータ
/// </summary>
public class UnitData
{
    public Vector3Int Position { get; private set; }
    public int UnitID { get; private set; }
    public bool[,,] UnitShape { get; private set; }

    private bool[] _existingHeight;

    private int _unitWidth;
    private int _unitDepth;

    public UnitData(int unitID, bool[,,] unitShape)
    {
        UnitID = unitID;
        UnitShape = unitShape;
        _existingHeight = new bool[UnitShape.GetLength(1)];

        _unitWidth = UnitShape.GetLength(0);
        _unitDepth = UnitShape.GetLength(2);

        for (int height = 0; height < _existingHeight.Length; height++)
        {
            if (_existingHeight[height]) continue;
            
            bool exist = false;
            for (int x = 0; x < _unitWidth; x++)
            {
                for (int z = 0; z < _unitDepth; z++)
                {
                    if (unitShape[x, height, z])
                    {
                        exist = true;
                        break;
                    }
                }

                if (exist) break;
            }

            if (exist)
            {
                _existingHeight[height] = true;
            }
        }
    }

    public void SetPosition(Vector3Int position)
    {
        Position = position;
    }

    /// <summary>
    /// 指定の座標とオブジェクトの最短マンハッタン距離を調べる
    /// </summary>
    /// <param name="position"></param>
    /// <returns>指定した高さに何もなければ-1が返る</returns>
    public int GetDistanceToPosition(Vector3Int position)
    {
        //その高さに何もなかったら-1を返して戻る
        if (!_existingHeight[position.y])
        {
            return -1;
        }

        int nearestDistance = int.MaxValue;

        for (int x = 0; x < UnitShape.GetLength(0); x++)
        {
            for (int z = 0; z < UnitShape.GetLength(2); z++)
            {
                if (!UnitShape[x, position.y, z])
                {
                    continue;
                }

                int manhattanDistance = Mathf.Abs(Position.x + x - position.x) + Mathf.Abs(Position.z + z - position.z);

                if (manhattanDistance < nearestDistance)
                {
                    nearestDistance = manhattanDistance;
                }
            }
        }

        return nearestDistance;
    }
}