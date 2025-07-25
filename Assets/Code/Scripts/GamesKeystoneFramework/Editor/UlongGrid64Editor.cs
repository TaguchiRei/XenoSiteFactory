using UnityEngine;
using UnityEditor;

public class UlongGrid64Editor : EditorWindow
{
    private const int Layers = 4;
    private const int Rows = 4;
    private const int Cols = 4;

    private const int SquareSize = 22;
    private const int Gap = 2;
    private const int StartX = 10;
    private const int StartY = 80;

    private bool[,,] boolGrid = new bool[Layers, Rows, Cols];
    private ulong ulongValue = 0;
    private string ulongInput = "0";

    private readonly Color trueColor = new Color(0f, 0.5f, 1f, 1f);
    private readonly Color falseColor = new Color(0.6f, 0.6f, 0.6f, 1f);

    [MenuItem("Window/BoolGrid64Editor")]
    public static void ShowWindow()
    {
        GetWindow<UlongGrid64Editor>("BoolGrid64Editor");
    }

    private void OnEnable()
    {
        UpdateBoolGridFromUlong();
    }

    private void OnGUI()
    {
        GUILayout.Label("64ビット(4x4x4)のboolグリッド", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("ulong値入力:", GUILayout.Width(80));
        ulongInput = EditorGUILayout.TextField(ulongInput);
        if (GUILayout.Button("反映", GUILayout.Width(60)))
        {
            if (ulong.TryParse(ulongInput, out ulong parsed))
            {
                ulongValue = parsed;
                UpdateBoolGridFromUlong();
            }
            else
            {
                Debug.LogWarning("ulongの数値として正しくありません");
            }
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);

        for (int layer = 0; layer < Layers; layer++)
        {
            float layerOffsetY = StartY + layer * (Rows * (SquareSize + Gap) + 24);
            GUI.Label(new Rect(StartX, layerOffsetY - 20, 100, 20), $"Layer {layer}", EditorStyles.boldLabel);

            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Cols; col++)
                {
                    float x = StartX + col * (SquareSize + Gap);
                    float y = layerOffsetY + row * (SquareSize + Gap);

                    Rect squareRect = new Rect(x, y, SquareSize, SquareSize);
                    bool state = boolGrid[layer, row, col];

                    EditorGUI.DrawRect(squareRect, state ? trueColor : falseColor);

                    if (Event.current.type == EventType.MouseDown && squareRect.Contains(Event.current.mousePosition))
                    {
                        boolGrid[layer, row, col] = !state;
                        UpdateUlongFromBoolGrid();
                        ulongInput = ulongValue.ToString();
                        Event.current.Use();
                        Repaint();
                    }
                }
            }
        }

        GUILayout.Space(Layers * (Rows * (SquareSize + Gap) + 30));

        if (GUILayout.Button("ulong値をDebug.Logに出力"))
        {
            UpdateUlongFromBoolGrid();
            Debug.Log($"ulong値: {ulongValue}");
            ulongInput = ulongValue.ToString();
        }
    }

    private void UpdateBoolGridFromUlong()
    {
        for (int layer = 0; layer < Layers; layer++)
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Cols; col++)
                {
                    int displayIndex = GetBitIndexFromDisplayPosition(layer, row, col);
                    boolGrid[layer, row, col] = (ulongValue & (1UL << displayIndex)) != 0;
                }
            }
        }
        Repaint();
    }

    private void UpdateUlongFromBoolGrid()
    {
        ulong result = 0;
        for (int layer = 0; layer < Layers; layer++)
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Cols; col++)
                {
                    if (boolGrid[layer, row, col])
                    {
                        int displayIndex = GetBitIndexFromDisplayPosition(layer, row, col);
                        result |= (1UL << displayIndex);
                    }
                }
            }
        }
        ulongValue = result;
    }

    /// <summary>
    /// 見た目のrow,colから、ビットインデックスを得る（13,14,15,16 → 0,1,2,3 になるように）
    /// </summary>
    private int GetBitIndexFromDisplayPosition(int layer, int row, int col)
    {
        // 表示上：row 0 は最上段 → ビット上では +12
        int invertedRow = 3 - row;
        int bitInLayer = invertedRow * 4 + col;
        return layer * 16 + bitInLayer;
    }
}
