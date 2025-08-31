using System.IO;
using Cysharp.Threading.Tasks;
using GamesKeystoneFramework.KeyDebug.KeyLog;
using GamesKeystoneFramework.Save;
using UnityEngine;

namespace XenositeFramework.SaveSystem
{
    public static class SaveDataSupporter
    {
        public static async UniTask<T> Load<T>(string filePath) where T : class
        {
            if (File.Exists(filePath))
            {
#if UNITY_EDITOR
                KeyLogger.Log("File Exists");
#endif
                var encrypted = await File.ReadAllBytesAsync(filePath);
                return JsonUtility.FromJson<T>(AESHelper.Decrypt(encrypted));
            }
#if UNITY_EDITOR
            KeyLogger.Log("File Not Exists");
#endif
            return null;
        }
    }
}
