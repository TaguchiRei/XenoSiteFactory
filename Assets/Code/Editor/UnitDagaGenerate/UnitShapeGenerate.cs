using UnityEngine;
using UnityEditor;

// UnitData 用の PropertyDrawer
[CustomPropertyDrawer(typeof(UnitData))]
public class UnitDataDrawer : PropertyDrawer
{
    private const float ToggleSize = 18f;
    private const float Spacing = 2f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty nameProp = property.FindPropertyRelative("Name");
        SerializedProperty infoProp = property.FindPropertyRelative("Info");
        SerializedProperty texProp = property.FindPropertyRelative("UnitTexture");
        SerializedProperty prefabProp = property.FindPropertyRelative("UnitPrefab");
        SerializedProperty shapeProp = property.FindPropertyRelative("UnitShape");
        SerializedProperty widthProp = property.FindPropertyRelative("UnitWidth");
        SerializedProperty depthProp = property.FindPropertyRelative("UniDepth");
        SerializedProperty heightProp = property.FindPropertyRelative("UnitHeight");

        float y = position.y;

        // 基本情報表示
        EditorGUI.PropertyField(new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight), nameProp);
        y += EditorGUIUtility.singleLineHeight + Spacing;
        EditorGUI.PropertyField(new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight), infoProp);
        y += EditorGUIUtility.singleLineHeight + Spacing;
        EditorGUI.PropertyField(new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight), texProp);
        y += EditorGUIUtility.singleLineHeight + Spacing;
        EditorGUI.PropertyField(new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight), prefabProp);
        y += EditorGUIUtility.singleLineHeight + Spacing;

        // サイズ設定
        EditorGUI.PropertyField(new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight), widthProp);
        y += EditorGUIUtility.singleLineHeight + Spacing;
        EditorGUI.PropertyField(new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight), depthProp);
        y += EditorGUIUtility.singleLineHeight + Spacing;
        EditorGUI.PropertyField(new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight), heightProp);
        y += EditorGUIUtility.singleLineHeight + Spacing;
        
        widthProp.intValue  = Mathf.Max(1, widthProp.intValue);
        depthProp.intValue  = Mathf.Max(1, depthProp.intValue);
        heightProp.intValue = Mathf.Max(1, heightProp.intValue);

        int width = widthProp.intValue;
        int depth = depthProp.intValue;
        int height = heightProp.intValue;

        if (shapeProp.arraySize != width * depth * height)
        {
            shapeProp.arraySize = width * depth * height;
        }

        // UnitShape の描画
        for (int h = 0; h < height; h++)
        {
            Rect foldoutRect = new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight);
            bool showLayer = EditorGUI.Foldout(foldoutRect, true, $"Layer {h}");
            y += EditorGUIUtility.singleLineHeight + Spacing;

            if (showLayer)
            {
                for (int d = 0; d < depth; d++)
                {
                    float rowX = position.x;
                    for (int w = 0; w < width; w++)
                    {
                        int index = h * width * depth + d * width + w;
                        Rect toggleRect = new Rect(rowX, y, ToggleSize, ToggleSize);
                        shapeProp.GetArrayElementAtIndex(index).boolValue =
                            EditorGUI.Toggle(toggleRect, shapeProp.GetArrayElementAtIndex(index).boolValue);
                        rowX += ToggleSize + Spacing;
                    }

                    y += ToggleSize + Spacing;
                }

                y += Spacing; // レイヤー間余白
            }
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty depthProp = property.FindPropertyRelative("UniDepth");
        SerializedProperty heightProp = property.FindPropertyRelative("UnitHeight");

        int depth = depthProp.intValue;
        int height = heightProp.intValue;

        float baseHeight = (4 + 3) * (EditorGUIUtility.singleLineHeight + 2f); // 基本フィールドの高さ
        float shapeHeight = (depth * 20 + 22) * height;
        return baseHeight + shapeHeight;
    }
}