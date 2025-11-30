using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CodeGenerator : EditorWindow
{
    private const string _folderPath = "Assets";
    private string _codeName = "NewCode";
    private string _code;
    private Vector2 _scrollPosition;

    private bool _showSimpleClass;
    private bool _showOthers;

    private bool _isPushButton;
    private bool _isEmptyName;

    private Func<string, string> _generateCodeFunc;

    [MenuItem("Window/UsefulTools/Code Generator")]
    public static void ShowWindow()
    {
        GetWindow<CodeGenerator>("Code Generator");
    }

    private void OnEnable()
    {
        _generateCodeFunc = GetSimpleCsCode;
    }

    public void OnGUI()
    {
        GUILayout.Label("Name");
        EditorGUI.BeginChangeCheck();
        _codeName = EditorGUILayout.TextField(_codeName);
        if (EditorGUI.EndChangeCheck())
        {
            _isPushButton = true;
        }

        GUILayout.Label("Code Preview");
        //コードプレビュー
        if (_isPushButton)
        {
            _code = _generateCodeFunc(_codeName);
            _isPushButton = false;
        }

        _code = EditorGUILayout.TextArea(_code, GUILayout.Height(position.height / 2f));

        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);

        #region SimpleCodes

        _showSimpleClass = EditorGUILayout.Foldout(_showSimpleClass, new GUIContent("Simple Class"));
        if (_showSimpleClass)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("SimpleCS"))
            {
                _generateCodeFunc = GetSimpleCsCode;
                _isPushButton = true;
            }

            if (GUILayout.Button("MonoBehaviour"))
            {
                _generateCodeFunc = GetSimpleMonoBehaviourScript;
                _isPushButton = true;
            }

            if (GUILayout.Button("ScriptableObject"))
            {
                _generateCodeFunc = GetSimpleScriptableObjectScript;
                _isPushButton = true;
            }

            if (GUILayout.Button("EditorWindow"))
            {
                _generateCodeFunc = GetSimpleEditorWindowScript;
                _isPushButton = true;
            }

            GUILayout.EndHorizontal();
        }

        #endregion

        _showOthers = EditorGUILayout.Foldout(_showOthers, new GUIContent("Others"));
        if (_showOthers)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Struct"))
            {
                _generateCodeFunc = GetStructCode;
                _isPushButton = true;
            }

            if (GUILayout.Button("Enum"))
            {
                _generateCodeFunc = GetEnumCode;
                _isPushButton = true;
            }

            if (GUILayout.Button("Interface"))
            {
                _generateCodeFunc = GetInterfaceCode;
                _isPushButton = true;
            }

            GUILayout.EndHorizontal();
        }


        GUILayout.EndScrollView();

        if (string.IsNullOrEmpty(_codeName))
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.red;
            style.fontStyle = FontStyle.Bold;
            GUILayout.Label("No Code name provided", style);
        }
        else if (GUILayout.Button("GenerateCode"))
        {
            GenerateCode(_code);
        }
    }

    #region GenerateCode

    private void GenerateCode(string code)
    {
        string selectedPath = EditorUtility.OpenFolderPanel("Select Folder", _folderPath, _codeName);
        if (!string.IsNullOrEmpty(selectedPath))
        {
            if (selectedPath.StartsWith(Application.dataPath))
            {
                var folderPath = "Assets" + selectedPath.Substring(Application.dataPath.Length);
                GenerateCsFile(folderPath, code);
            }
            else
            {
                Debug.LogWarning("Assetsフォルダ内を選択してください");
            }
        }
    }

    /// <summary>
    /// CSファイルを生成する。
    /// </summary>
    /// <param name="path"></param>
    /// <param name="code"></param>
    private void GenerateCsFile(string path, string code)
    {
        File.WriteAllText(path, code);
        AssetDatabase.Refresh();

        Debug.Log("EditorWindowスクリプトを生成しました: " + path);
    }

    #endregion

    #region GetCode

    private string GetSimpleCsCode(string className)
    {
        return $@"public class {className}
{{
    
}}";
    }

    private string GetSimpleMonoBehaviourScript(string className)
    {
        return $@"using UnityEngine;

public class {className} : MonoBehaviour
{{
    
}}

";
    }

    private string GetSimpleScriptableObjectScript(string className)
    {
        return $@"using UnityEngine;

[CreateAssetMenu(fileName = ""{className}"", menuName = ""ScriptableObjects/{className}"")]
public class {className} : ScriptableObject
{{
    
}}
";
    }

    private string GetSimpleEditorWindowScript(string className)
    {
        return $@"using UnityEngine;
using UnityEditor;

public class {name} : EditorWindow
{{
    [MenuItem(""Window/UsefulTools/{name}"")]
    public static void ShowWindow()
    {{
        GetWindow<{name}>(""{name}"");
    }}

    private void OnGUI()
    {{

    }}
}}";
    }

    private string GetStructCode(string structName)
    {
        return $@"public struct {structName}
{{

    public {structName}()
    {{

    }}
}}";
    }

    private string GetEnumCode(string enumName)
    {
        return $@"public enum {enumName} 
{{
    
}}";
    }

    private string GetInterfaceCode(string interfaceName)
    {
        return $@"public  interface {interfaceName}
{{
    
}}";
    }

    #endregion
}