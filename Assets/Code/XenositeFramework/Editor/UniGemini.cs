using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace XenositeFramework.Editor
{
    public class UniGemini : EditorWindow
    {
        private const string USER = "User : ";
        private const string GEMINI = "UniGemini : ";
        
        private List<(string, bool)> _context;
        private Vector2 _scrollPos;
        private Vector2 _userTextScroll;
        private GeminiCmdClient _geminiCmdClient;

        private string _userText;
        private bool _sendAllContext = true;
        [Range(5, 100)] private int _sendContextLength = 20;

        private string _cmdPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "npm",
            "gemini.cmd");

        private bool _editPath;
        private bool _sendingMessage;
        private bool _shouldAutoScroll;

        private const string ContextKey = "XenositeFramework.UniGemini.Context";

        [MenuItem("Window/XenositeFramework/UniGemini")]
        public static void ShowWindow()
        {
            GetWindow<UniGemini>("UniGemini");
        }

        private void OnEnable()
        {
            LoadContext();

            if (!string.IsNullOrEmpty(_cmdPath))
            {
                _geminiCmdClient = new GeminiCmdClient(_cmdPath);
            }
        }

        void OnGUI()
        {
            var wordWrapTextArea = new GUIStyle(EditorStyles.textArea) { wordWrap = true };

            Rect chatRect = new Rect(5, 5, position.width - 10, position.height - 160);
            EditorGUI.DrawRect(chatRect, new Color(0.15f, 0.15f, 0.15f));

            GUILayout.BeginArea(chatRect);
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.Width(chatRect.width),
                GUILayout.Height(chatRect.height));
            {
                foreach (var message in _context)
                {
                    GUILayout.BeginHorizontal();
                    if (message.Item2)
                    {
                        GUILayout.FlexibleSpace();
                        GUILayout.Label("User", GUILayout.Width(50));
                        EditorGUILayout.TextArea(message.Item1, wordWrapTextArea, GUILayout.ExpandWidth(true));
                    }
                    else
                    {
                        GUILayout.Label("Gemini", GUILayout.Width(50));
                        EditorGUILayout.TextArea(message.Item1, wordWrapTextArea, GUILayout.ExpandWidth(true));
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndScrollView();
            GUILayout.EndArea();

            if (_shouldAutoScroll && Event.current.type == EventType.Layout)
            {
                _scrollPos.y = Mathf.Infinity;
                _shouldAutoScroll = false;
            }

            GUILayout.Space(chatRect.height + 10);

            EditorGUILayout.BeginHorizontal();
            _editPath = EditorGUILayout.Toggle(_editPath, GUILayout.Width(20));
            EditorGUI.BeginDisabledGroup(!_editPath);
            _cmdPath = EditorGUILayout.TextField("gemini.cmd Path", _cmdPath);
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();

            if (_geminiCmdClient == null)
            {
                if (GUILayout.Button("Connect to Gemini CLI") && !string.IsNullOrEmpty(_cmdPath))
                {
                    _geminiCmdClient = new GeminiCmdClient(_cmdPath);
                }
            }

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Get Path of Selection"))
            {
                _userText += AssetDatabase.GetAssetPath(Selection.activeObject);
                GUI.FocusControl(null);
                Repaint();
            }

            _sendAllContext = EditorGUILayout.Toggle("Send All Context", _sendAllContext);
            if (!_sendAllContext)
            {
                _sendContextLength = EditorGUILayout.IntField("Context Length", _sendContextLength);
                _sendContextLength = Mathf.Clamp(_sendContextLength, 0, 100);
            }
            GUILayout.EndHorizontal();

            EditorGUI.BeginDisabledGroup(_sendingMessage);
            float textAreaHeight = EditorStyles.textArea.lineHeight * 4 + 10;
            _userTextScroll = EditorGUILayout.BeginScrollView(_userTextScroll, GUILayout.Height(textAreaHeight));
            _userText = EditorGUILayout.TextArea(_userText, GUILayout.ExpandWidth(true));
            EditorGUILayout.EndScrollView();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Send") && !string.IsNullOrEmpty(_userText))
            {
                if (_geminiCmdClient != null)
                {
                    _context.Add((_userText, true));
                    _userText = string.Empty;
                    _shouldAutoScroll = true;
                    _sendingMessage = true;
                    HandlePromptAsync().Forget();
                }
                else
                {
                    Debug.LogWarning("GeminiCmdClient is not initialized.");
                }
            }

            if (GUILayout.Button("Delete Context"))
            {
                _context.Clear();
            }
            GUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();

            if (_sendingMessage)
            {
                EditorGUILayout.LabelField("Sending...");
            }
        }

        private string AssembleFullPrompt()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<system>
You are an AI assistant integrated into a Unity Editor window.
Your ONLY way to interact with the file system is by outputting a command in a <cmd> tag.
You MUST NOT use any other format or tool-calling syntax.

**ABSOLUTELY DO NOT USE `` or `<TOOL .../>` formats.**

The only valid format is: `<cmd=""tool_name 'argument1' 'argument2' ..."">`

Available commands:
- `read_file 'path/to/file.txt'`
- `write_file 'path/to/file.txt' 'file content'`
- `list_directory 'path/to/dir'`
- `create_directory 'path/to/new_dir'`
- `replace 'file_path' 'old_string' 'new_string'`
- `glob 'search_pattern'`
- `search 'search_string' 'target_file_pattern'`

**Example Interaction:**
User : Can you show me the contents of C:/hello.txt?
UniGemini : <cmd=""read_file 'C:/hello.txt'"">

Now, begin.
</system>");

            int send = _sendAllContext ? _context.Count : _sendContextLength;
            int startIndex = Math.Max(0, _context.Count - send);
            for (int i = startIndex; i < _context.Count; i++)
            {
                sb.Append(_context[i].Item2 ? USER : GEMINI);
                sb.AppendLine(_context[i].Item1);
            }
            return sb.ToString();
        }

        private async UniTaskVoid HandlePromptAsync()
        {
            await RunAILoop();
        }

        private async UniTask RunAILoop()
        {
            while (true)
            {
                var fullPrompt = AssembleFullPrompt();
                Debug.Log(fullPrompt);

                string result;
                try
                {
                    result = await _geminiCmdClient.SendPromptAsync(fullPrompt,"gemini-2.5-pro");
                    await UniTask.SwitchToMainThread();
                }
                catch (Exception ex)
                {
                    await UniTask.SwitchToMainThread();
                    Debug.LogError($"Gemini CLI Error: {ex}");
                    _context.Add(($"[Error] " + ex.Message, false));
                    _sendingMessage = false;
                    Repaint();
                    break;
                }

                _context.Add((result, false));

                if (result.Trim().StartsWith("<cmd"))
                {
                    ExecuteTool(result);
                }
                else
                {
                    _sendingMessage = false;
                    _shouldAutoScroll = true;
                    Repaint();
                    break;
                }
            }
        }


        private void ExecuteTool(string cmdCall)
        {
            string resultLog;
            try
            {
                // 1. Extract command line from <cmd=""...""">
                var cmdRegex = new Regex(@"<cmd=""([\s\S]*?)"">");
                var cmdMatch = cmdRegex.Match(cmdCall);
                if (!cmdMatch.Success)
                {
                    throw new ArgumentException("Invalid <cmd> format.");
                }
                var commandLine = cmdMatch.Groups[1].Value;

                // 2. Extract tool name and arguments
                var toolName = commandLine.Split(' ')[0];
                
                var argRegex = new Regex(@"'([^']*)'");
                var argMatches = argRegex.Matches(commandLine);
                var args = argMatches.Cast<Match>().Select(m => m.Groups[1].Value).ToList();

                if (!EditorUtility.DisplayDialog("Confirm Command Execution", $"AI wants to execute the command: {toolName}\n\n{commandLine}", "Allow", "Deny"))
                {
                    resultLog = "[Log] Command execution was denied by user.";
                    _context.Add((resultLog, false));
                    return;
                }

                // 3. Execute command
                switch (toolName)
                {
                    case "write_file":
                        if (args.Count < 2) throw new ArgumentException("write_file requires 2 arguments: path and content.");
                        System.IO.File.WriteAllText(args[0], args[1]);
                        resultLog = $"[Log] File written to {args[0]}";
                        break;
                    
                    case "read_file":
                        if (args.Count < 1) throw new ArgumentException("read_file requires 1 argument: path.");
                        resultLog = $"[Log] Content of {args[0]}:\n---\n{System.IO.File.ReadAllText(args[0])}\n---";
                        break;

                    case "list_directory":
                        if (args.Count < 1) throw new ArgumentException("list_directory requires 1 argument: path.");
                        var files = string.Join("\n", System.IO.Directory.GetFiles(args[0]));
                        var dirs = string.Join("\n", System.IO.Directory.GetDirectories(args[0]).Select(d => d + "/"));
                        resultLog = $"[Log] Contents of {args[0]}:\n---\n{dirs}\n{files}\n---";
                        break;

                    case "create_directory":
                        if (args.Count < 1) throw new ArgumentException("create_directory requires 1 argument: path.");
                        System.IO.Directory.CreateDirectory(args[0]);
                        resultLog = $"[Log] Directory created at {args[0]}";
                        break;

                    case "replace":
                        if (args.Count < 3) throw new ArgumentException("replace requires 3 arguments: file_path, old_string, new_string.");
                        var fileContent = System.IO.File.ReadAllText(args[0]);
                        var newContent = fileContent.Replace(args[1], args[2]);
                        System.IO.File.WriteAllText(args[0], newContent);
                        resultLog = $"[Log] Replaced string in file {args[0]}";
                        break;

                    case "glob":
                        if (args.Count < 1) throw new ArgumentException("glob requires 1 argument: search_pattern.");
                        var globFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), args[0], SearchOption.AllDirectories);
                        resultLog = $"[Log] Glob results for '{args[0]}':\n---\n{string.Join("\n", globFiles)}\n---";
                        break;

                    case "search":
                        if (args.Count < 2) throw new ArgumentException("search requires 2 arguments: search_string and target_file_pattern.");
                        var searchString = args[0];
                        var searchPattern = args[1];
                        var searchResults = new List<string>();
                        var filesToSearch = Directory.GetFiles(Directory.GetCurrentDirectory(), searchPattern, SearchOption.AllDirectories);
                        foreach (var file in filesToSearch)
                        {
                            var lines = System.IO.File.ReadAllLines(file);
                            for (int i = 0; i < lines.Length; i++)
                            {
                                if (lines[i].Contains(searchString))
                                {
                                    searchResults.Add($"{file}:{i + 1}: {lines[i]}");
                                }
                            }
                        }
                        resultLog = $"[Log] Search results for '{searchString}' in '{searchPattern}':\n---\n{string.Join("\n", searchResults)}\n---";
                        break;

                    default:
                        resultLog = $"[Error] Unknown command: {toolName}";
                        break;
                }
            }
            catch (Exception e)
            {
                resultLog = $"[Error] Command execution failed: {e.Message}";
            }
            
            _context.Add((resultLog, false));
        }

        private void OnDisable()
        {
            SaveContext();
        }

        private void SaveContext()
        {
            var wrapper = new Wrapper
            {
                Strings = _context.Select(item => item.Item1).ToArray(),
                Bools = _context.Select(item => item.Item2).ToArray()
            };
            string json = JsonUtility.ToJson(wrapper);
            EditorPrefs.SetString(ContextKey, json);
        }

        private void LoadContext()
        {
            string json = EditorPrefs.GetString(ContextKey, string.Empty);
            _context = new List<(string, bool)>();
            if (!string.IsNullOrEmpty(json))
            {
                var wrapper = JsonUtility.FromJson<Wrapper>(json);
                for (int i = 0; i < wrapper.Strings.Length; i++)
                {
                    _context.Add((wrapper.Strings[i], wrapper.Bools[i]));
                }
            }
        }
    }

    [Serializable]
    public class Wrapper
    {
        public string[] Strings;
        public bool[] Bools;
    }
}