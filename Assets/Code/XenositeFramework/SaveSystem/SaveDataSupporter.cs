using System.Collections.Generic;
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

        /// <summary>
        /// セーブデータをすべて読み込み配列の形式で返す。
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async UniTask<T[]> LoadAll<T>(string directoryPath) where T : class
        {
            if (Directory.Exists(directoryPath))
            {
                var files = Directory.GetFiles(directoryPath);
                if (files.Length > 0)
                {
                    var data = new T[files.Length];
                    for (int i = 0; i < files.Length; i++)
                    {
                        var encrypted = await File.ReadAllBytesAsync(files[i]);
                        data[i] = JsonUtility.FromJson<T>(AESHelper.Decrypt(encrypted));
                    }

                    return data;
                }
            }

            return null;
        }
    }
}