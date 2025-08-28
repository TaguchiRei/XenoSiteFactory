using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using GamesKeystoneFramework.Save;
using UnityEditor;
using UnityEngine;
using XenositeFramework.SaveSystem;

namespace XenositeFramework.Editor
{
    public class SaveDataGenerator : EditorWindow
    {
        private Dictionary<SaveDataEnum, Type> _saveData = new();
        private SaveDataEnum _saveDataEnum;
        private FieldInfo[] _fieldInfos;
        private SerializedObject _serializedObject;
        private ScriptableSaveData _scriptableSaveData;
        [SerializeField] private string[] _addUsings;
        private SerializedObject _usingSerializedObject;
        private SerializedProperty _usingSerializedProperty;
        private Vector2 _scrollPosition;
        private int _testSaveDataNumber;

        [MenuItem("Window/XenositeFramework/SaveDataGenerator")]
        public static void ShowWindow()
        {
            GetWindow<SaveDataGenerator>("SaveDataGenerator");
        }

        private void OnEnable()
        {
            _usingSerializedObject = new SerializedObject(this);
            _usingSerializedProperty = _usingSerializedObject.FindProperty("_addUsings");
        }

        public void OnGUI()
        {
            if (GUILayout.Button("FindSaveData"))
            {
                _saveData.Clear();
                var types = GetAllSaveData();
                string[] classNames = types.Select(t => t.Name).ToArray();
                GenerateEnum(classNames);
                foreach (var type in types)
                {
                    SaveDataEnum enumValue = (SaveDataEnum)Enum.Parse(typeof(SaveDataEnum), type.Name);
                    _saveData.Add(enumValue, type);
                }

                if (_saveDataEnum == SaveDataEnum.None) return;
                Type GenerateType = _saveData[_saveDataEnum];
                _fieldInfos = GetAllSaveDataFields(GenerateType);
                if (_fieldInfos != null && _fieldInfos.Length > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string className = GenerateType.Name + "TestData";

                    sb.Append("using UnityEngine;\n");
                    sb.Append("using System;\n");
                    sb.Append("using System.Collections.Generic;\n\n");
                    foreach (var addUsing in _addUsings)
                    {
                        sb.Append($"using {addUsing};\n");
                    }

                    sb.Append("\nnamespace XenositeFramework.Editor\n{\n");
                    sb.Append($"[CreateAssetMenu(menuName = \"XenositeFramework/Editor/{className}\")]");
                    sb.Append("    public class " + className + " : ScriptableSaveData\n    {\n");

                    foreach (var info in _fieldInfos)
                    {
                        string typeName = GetFriendlyTypeName(info.FieldType);
                        string fieldName = info.Name;
                        sb.Append($"        public {typeName} {fieldName};\n");
                    }

                    sb.Append("    }\n");
                    sb.Append("}\n");

                    string path = $"Assets/Code/Editor/TestSaveData/{className}.cs";
                    string dir = Path.GetDirectoryName(path);
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    File.WriteAllText(path, sb.ToString());
                    AssetDatabase.Refresh();
                    Debug.Log($"{className}.cs を生成しました。");
                }
            }

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.Height(position.height - 180));
            _usingSerializedObject.Update();
            EditorGUILayout.PropertyField(_usingSerializedProperty, new GUIContent("必要なusingを入力"), true);
            _usingSerializedObject.ApplyModifiedProperties();
            if (_saveData != null && _saveData.Count > 0)
            {
                EditorGUILayout.BeginHorizontal();
                _testSaveDataNumber = EditorGUILayout.IntField("セーブデータ番号", _testSaveDataNumber);
                if (GUILayout.Button("テスト用セーブデータを作成"))
                {
                    string path = Path.Combine(Application.persistentDataPath, $"TestData{_testSaveDataNumber}.dat");
                    string json = JsonUtility.ToJson(_scriptableSaveData);
                    byte[] aes = AESHelper.Encrypt(json);
                    File.WriteAllBytes(path, aes);
                }

                if (GUILayout.Button("セーブデータのあるディレクトリを取得"))
                {
                    Debug.Log($"クリップボードに保存しました。: {Application.persistentDataPath}");

                    GUIUtility.systemCopyBuffer = Application.persistentDataPath;
                }

                EditorGUILayout.EndHorizontal();
                _saveDataEnum = (SaveDataEnum)EditorGUILayout.EnumPopup("Select SaveDataType", _saveDataEnum);

                if (_saveDataEnum != SaveDataEnum.None)
                {
                    _scriptableSaveData = (ScriptableSaveData)EditorGUILayout.ObjectField(
                        "セーブデータ", _scriptableSaveData, typeof(ScriptableObject), false);
                    if (_scriptableSaveData == null)
                    {
                        EditorGUILayout.EndScrollView();
                        return;
                    }
                    if (_serializedObject == null || _serializedObject.targetObject != _scriptableSaveData)
                    {
                        _serializedObject = new SerializedObject(_scriptableSaveData);
                    }

                    _serializedObject.Update();
                    SerializedProperty property = _serializedObject.GetIterator();
                    bool expanded = true;
                    while (property.NextVisible(expanded))
                    {
                        if (property.name == "m_Script") continue; // m_Scriptはスキップ
                        EditorGUILayout.PropertyField(property, true);
                        expanded = false;
                    }

                    _serializedObject.ApplyModifiedProperties();
                }
            }

            EditorGUILayout.EndScrollView();
        }

        public FieldInfo[] GetAllSaveDataFields(Type type)
        {
            return type.GetFields(BindingFlags.Public | BindingFlags.Instance);
        }

        private Type[] GetAllSaveData()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            return assemblies.SelectMany(assembly =>
            {
                try
                {
                    return assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException e)
                {
                    return e.Types.Where(t => t != null);
                }
            }).Where(t =>
            {
                var baseType = t.BaseType;
                if (baseType == null) return false;

                if (baseType.IsGenericType)
                {
                    return baseType.GetGenericTypeDefinition() == typeof(SaveDataBase<>);
                }

                return false;
            }).ToArray();
        }

        private void GenerateEnum(string[] values)
        {
            StringBuilder content = new StringBuilder();
            content.Append("namespace XenositeFramework.Editor\n{\n");
            content.Append("    public enum SaveDataEnum\n    {\n");
            content.Append("        None,\n");

            for (int i = 0; i < values.Length; i++)
            {
                content.Append("        " + values[i]);
                if (i < values.Length - 1) content.Append(",");
                content.Append("\n");
            }

            content.Append("    }\n");
            content.Append("}");

            File.WriteAllText("Assets/Code/XenositeFramework/Enums/SaveDataEnum.cs", content.ToString());
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// C#らしい表記に型名を変換する
        /// </summary>
        private string GetFriendlyTypeName(Type type)
        {
            if (type == typeof(int)) return "int";
            if (type == typeof(float)) return "float";
            if (type == typeof(double)) return "double";
            if (type == typeof(bool)) return "bool";
            if (type == typeof(string)) return "string";

            // ジェネリック型（List<T>など）
            if (type.IsGenericType)
            {
                string typeName = type.GetGenericTypeDefinition().Name;
                typeName = typeName.Substring(0, typeName.IndexOf('`')); // List`1 → List
                Type[] genericArgs = type.GetGenericArguments();
                string genericArgsString = string.Join(", ", genericArgs.Select(t => GetFriendlyTypeName(t)));
                return $"{typeName}<{genericArgsString}>";
            }

            return type.Name; // その他はそのまま
        }
    }
}