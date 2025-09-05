using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace XenositeFramework.Editor
{
    [InitializeOnLoad]
    public class SceneEnumGenerate
    {
        private const string OutputPath = "Assets/Code/AutoGenerate/SceneEnum.cs";
        private const string ENUM_SCRIPT = "public enum SceneName\n{\n";
        private const string ENUM_SCRIPT_END = "}\n";
        
        static SceneEnumGenerate()
        {
            EditorBuildSettings.sceneListChanged += Generate;
            Generate();
        }
        
        private static void Generate()
        {
            var scenes = EditorBuildSettings.scenes
                .Where(s => s.enabled)
                .Select(s => Path.GetFileNameWithoutExtension(s.path))
                .Distinct()
                .ToArray();

            if (scenes.Length == 0) return;

            for (int i = 0; i < scenes.Length; i++)
            {
                scenes[i] = Regex.Replace(scenes[i], @"[^a-zA-Z0-9_]", "_");
            }

            StringBuilder code = new StringBuilder();
            code.Append(ENUM_SCRIPT);
            foreach (var scene in scenes)
            {
                code.Append($"    {scene},\n");
            }
            code.Append(ENUM_SCRIPT_END);
            
            string dir = Path.GetDirectoryName(OutputPath);
            if (dir == null)
            {
                CreateFolder();
                Debug.Log($"Directory Created : {OutputPath}");
                return;
            }

            Directory.CreateDirectory(dir);
            File.WriteAllText(OutputPath, code.ToString());
            AssetDatabase.Refresh();
        }
        
        private static void CreateFolder()
        {
            string relativePath = "Code/AutoGenerate"; // Assets以下の相対パス
            string fullPath = Path.Combine(Application.dataPath, relativePath);

            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
                Debug.Log($"フォルダ作成: {fullPath}");
            }
            else
            {
                Debug.Log("フォルダはすでに存在します: " + fullPath);
            }
        }
    }
}
