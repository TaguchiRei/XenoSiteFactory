using System;
using System.IO;
using System.Linq;
using System.Reflection;
using GamesKeystoneFramework.Save;
using UnityEditor;
using UnityEngine;

namespace XenositeFramework.Editor
{
    public class SaveDataGenerator : EditorWindow
    {
        private Type[] _types;
        private SaveDataEnum saveDataEnum;
        
        [MenuItem("Window/XenositeFramework/SaveDataGenerator")]
        public static void ShowWindow()
        {
            GetWindow<SaveDataGenerator>("SaveDataGenerator");
        }

        public void OnGUI()
        {
            if (GUILayout.Button("FindSaveData"))
            {
                _types = GetAllSaveData();
                string[] classNames = _types.Select(t => t.Name).ToArray();
                GenerateEnum(classNames);
            }

            if (_types != null && _types.Length > 0)
            {
                
            }
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

        public static void GenerateEnum(string[] values)
        {        
            string content = "namespace XenositeFramework.Editor\n{\n";
            content += "    public enum SaveDataEnum\n    {\n";

            for (int i = 0; i < values.Length; i++)
            {
                content += "        " + values[i];
                if (i < values.Length - 1) content += ",";
                content += "\n";
            }

            content += "    }\n";
            content += "}";

            File.WriteAllText("Assets/Code/XenositeFramework/Enums/SaveDataEnum.cs", content);
            AssetDatabase.Refresh();
        }
    }
}
