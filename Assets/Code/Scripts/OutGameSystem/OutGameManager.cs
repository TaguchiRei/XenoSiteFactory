using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Player;
using UnityEngine;
using XenositeFramework.SaveSystem;

namespace OutGame
{
    public class OutGameManager : MonoBehaviour
    {
        //[SerializeField] private 
        private List<SaveData> playerDatas;
        public void ShowAllSaveData()
        {
            
        }

        private async UniTask FindAllSaveData()
        {
            playerDatas = new List<SaveData>();
            string[] allPath = Directory.GetFiles(Application.persistentDataPath, "*.dat", SearchOption.AllDirectories);
            foreach (var path in allPath)
            {
                playerDatas.Add(await SaveDataSupporter.Load<SaveData>(path));
            }
        }
    }
}
