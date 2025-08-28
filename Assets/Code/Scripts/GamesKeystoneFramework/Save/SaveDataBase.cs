using System;
using System.IO;
using UnityEngine;
using Cysharp.Threading.Tasks;
using GamesKeystoneFramework.KeyDebug.KeyLog;
using XenositeFramework.SaveSystem;

namespace GamesKeystoneFramework.Save
{
    /// <summary>
    /// セーブデータはここを継承したクラスに変数を作って保存する
    /// </summary>
    [Serializable]
    public abstract class SaveDataBase<T>where T : SaveDataBase<T>
    {
        /// <summary>
        /// セーブする際はこれを呼び出す
        /// </summary>
        /// <param name="dataNumber"></param>
        /// <param name="fileName"></param>
        public async UniTask Save(int dataNumber, string fileName = "SaveData")
        {
            string path = Application.persistentDataPath + $"/{fileName + dataNumber}.dat";
#if UNITY_EDITOR
            KeyLogger.Log(File.Exists(path) ? "File Exists" : "File Not Exists");
#endif
            await  File.WriteAllBytesAsync(path, AESHelper.Encrypt(JsonUtility.ToJson(this)));
        }
        
        public async UniTask<T> Load(int dataNumber, string fileName = "SaveData")
        {
            string path = Application.persistentDataPath + $"/{fileName + dataNumber}.dat";
            if (File.Exists(path))
            {
#if UNITY_EDITOR
                KeyLogger.Log("File Exists");
#endif
                var encrypted = await File.ReadAllBytesAsync(path);
                return JsonUtility.FromJson<T>(AESHelper.Decrypt(encrypted));
            }
#if UNITY_EDITOR
            KeyLogger.Log("File Not Exists");
#endif
            return null;
        }
        
        /// <summary>
        /// セーブデータの初期化を行う
        /// </summary>
        /// <param name="dataNumber">データの番号</param>
        /// <param name="fileName"></param>
        public async UniTask ResetData(int dataNumber, string fileName = "SaveData")
        {
            string path = Application.persistentDataPath + $"/{fileName + dataNumber}.dat";
            if (File.Exists(path))
            {
#if UNITY_EDITOR
                KeyLogger.Log("File Exists");
#endif
            }
            else
            {
#if UNITY_EDITOR
                KeyLogger.Log("File Not Exists");
#endif
            }
            await File.WriteAllBytesAsync(path, AESHelper.Encrypt(JsonUtility.ToJson(Initialize())));
        }

        /// <summary>
        /// 初期状態のセーブデータを戻り値に設定してください。
        /// </summary>
        /// <returns></returns>
        protected abstract T Initialize();
    }
}