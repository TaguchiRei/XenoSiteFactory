using UnityEngine;

/// <summary>
/// ユニットの処理に使うデータ
/// </summary>
public class UnitEntity
{
    public int UnitID { get; private set; }
    public Vector3Int Position { get; private set; }
    public UnitRotate Rotation { get; private set; }
    public Vector3Int[] InputPorts { get; private set; }
    public Vector3Int[] OutputPorts { get; private set; }
    public bool[,,] UnitShape { get; private set; }
    public int UnitWidth { get; private set; }
    public int UnitDepth { get; private set; }

    public int UnitHeight { get; private set; }

    private bool[] _existingHeight;


    public UnitEntity(int unitID, Vector3Int position, UnitRotate rotation, Vector3Int[] inputPorts, Vector3Int[] outputPorts, bool[,,] unitShape)
    {
        UnitID = unitID;
        UnitShape = unitShape;
        Rotation = rotation;
        InputPorts = inputPorts;
        OutputPorts = outputPorts;

        UnitWidth = unitShape.GetLength(0);
        UnitHeight = unitShape.GetLength(1);
        UnitDepth = unitShape.GetLength(2);

        _existingHeight = new bool[UnitHeight];

        //ユニットが存在しない高さがある場合その高度の探査をスキップするのであらかじめ調べておく
        for (int y = 0; y < UnitHeight; y++)
        {
            bool isExist = false;
            for (int x = 0; x < UnitWidth; x++)
            {
                for (int z = 0; z < UnitDepth; z++)
                {
                    if (UnitShape[x, y, z])
                    {
                        isExist = true;
                        break;
                    }
                }

                if (isExist) break;
            }

            if (isExist) _existingHeight[y] = true;
        }

        SetPosition(position);
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
    public int GetNearestDistanceAt(Vector3Int position)
    {
        int localY = position.y - Position.y;
        //positionで指定する高さがオブジェクトと重なる高さでなければ-1を返す
        if (position.y < Position.y || position.y >= Position.y + UnitHeight) return -1;
        //positionで指定した高さにユニットが存在しない場合-1を返す
        if (!_existingHeight[localY]) return -1;

        int nearestDistance = int.MaxValue;

        for (int x = 0; x < UnitWidth; x++)
        {
            for (int z = 0; z < UnitDepth; z++)
            {
                if (!UnitShape[x, localY, z]) continue;

                //マンハッタン距離で距離を調べる。(キャラクターなどは前後左右のみ移動し斜め移動はしない予定)
                var manhattan = Mathf.Abs((Position.x + x) - position.x) + Mathf.Abs(Position.z + z - position.z);

                if (manhattan < nearestDistance)
                {
                    nearestDistance = manhattan;
                }
            }
        }

        return nearestDistance;
    }

    /// <summary>
    /// 指定した座標がユニットによって占有されているかを調べる
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool HasBlockAt(Vector3Int position)
    {
        var checkIndex = position - Position;

        //そもそも範囲外ならそこには何もない
        if (checkIndex.x < 0 || checkIndex.y < 0 || checkIndex.z < 0 ||
            checkIndex.x >= UnitWidth || checkIndex.y >= UnitHeight || checkIndex.z >= UnitDepth) return false;

        return UnitShape[checkIndex.x, checkIndex.y, checkIndex.z];
    }
}