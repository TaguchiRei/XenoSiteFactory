using System;
using UnityEditor;
using UnityEngine;

public class SceneLoader : EditorWindow
{
    private string[] onListScenes;
    private string[] outListScenes;
    private Vector2 onListScroll;
    private Vector2 outListScroll;

    [MenuItem("Window/UsefulTools/Scene Loader")]
    public static void ShowWindow()
    {
        GetWindow<SceneLoader>("Scene Loader");
    }


    private void OnGUI()
    {
        if (GUILayout.Button("シーンリストを更新"))
        {
            onListScenes = Enum.GetNames(typeof(InListSceneName));
            outListScenes = Enum.GetNames(typeof(OutListSceneName));
        }

        if (onListScenes == null) return;
        // 左側
        EditorGUILayout.LabelField("On List Scenes", EditorStyles.boldLabel);
        onListScroll = EditorGUILayout.BeginScrollView(onListScroll);

        if (onListScenes != null)
        {
            foreach (var scene in onListScenes)
            {
                if (GUILayout.Button(scene))
                {
                    UnityEditor.SceneManagement.EditorSceneManager.OpenScene(FindScenePathUnderLevelScenes(scene));
                }
            }
        }

        EditorGUILayout.EndScrollView();

        // 右側
        EditorGUILayout.LabelField("Out List Scenes", EditorStyles.boldLabel);
        outListScroll = EditorGUILayout.BeginScrollView(outListScroll);

        if (outListScenes != null)
        {
            foreach (var scene in outListScenes)
            {
                if (GUILayout.Button(scene))
                {
                    UnityEditor.SceneManagement.EditorSceneManager.OpenScene(FindScenePathUnderLevelScenes(scene));
                }
            }
        }

        EditorGUILayout.EndScrollView();
    }


    static string FindScenePathUnderLevelScenes(string sceneName)
    {
        // "t:Scene" でシーンのみ検索、フォルダ指定で範囲を限定
        var guids = AssetDatabase.FindAssets($"t:Scene {sceneName}", new[] { "Assets/Level/Scenes" });

        if (guids == null || guids.Length == 0)
            return null;

        // 同名シーンが複数ある可能性があるため最初の一致を返す
        // 必要なら名前完全一致に絞る処理を追加可能
        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var file = System.IO.Path.GetFileNameWithoutExtension(path);

            if (file == sceneName)
                return path;
        }

        return null;
    }
}