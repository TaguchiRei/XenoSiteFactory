using System;
using GamesKeystoneFramework.Save;
using UnityEditor;
using UnityEngine;

namespace GamesKeystoneFramework.Editor
{
    public class SaveDataGenerator : EditorWindow
    {
        
        
        [MenuItem("Window/XenositeFramework/SaveDataGenerator")]
        public static void ShowWindow()
        {
            GetWindow<SaveDataGenerator>("SaveDataGenerator");
        }

        public void OnGUI()
        {
            
        }
        
    }
}
