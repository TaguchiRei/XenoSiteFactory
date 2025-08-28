using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using GamesKeystoneFramework.Save;
using UnityEditor;
using UnityEngine;

namespace XenositeFramework.Editor
{
    public class SaveDataGenerator : EditorWindow
    {
        private Dictionary<SaveDataEnum,Type> _saveData = new();
        private SaveDataEnum _saveDataEnum;
        private FieldInfo[] _fieldInfos;
        private List<(SerializedObject serializedObject, SerializedProperty serializedProperty)> _serialized = new();
        
        [MenuItem("Window/XenositeFramework/SaveDataGenerator")]
        public static void ShowWindow()
        {
            GetWindow<SaveDataGenerator>("SaveDataGenerator");
        }

        public void OnGUI()
        {
            if (GUILayout.Button("FindSaveData"))
            {
                _saveData.Clear();
                _serialized.Clear();
                var types = GetAllSaveData();
                string[] classNames = types.Select(t => t.Name).ToArray();
                GenerateEnum(classNames);
                foreach (var type in types)
                {
                    SaveDataEnum enumValue = (SaveDataEnum)Enum.Parse(typeof(SaveDataEnum), type.Name);
                    _saveData.Add(enumValue, type);
                }
                Type GenerateType = _saveData[_saveDataEnum];
                _fieldInfos = GetAllSaveDataFields(GenerateType);
                foreach (var info in _fieldInfos)
                {
                    
                }
            }

            if (_saveData != null && _saveData.Count > 0)
            {
                _saveDataEnum =  (SaveDataEnum)EditorGUILayout.EnumPopup("Select SaveDataType", _saveDataEnum);

                if (_saveDataEnum != SaveDataEnum.None)
                {
                    
                }
            }
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
    }
}
