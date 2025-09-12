using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Player;
using UnityEngine;
using XenositeFramework.SaveSystem;

namespace OutGameSystem.Dev
{
    public class OutGameDev : MonoBehaviour
    {
        [SerializeField] private string _saveDataPath;

        List<PlayerData> _dataList = new();

        private async void Start()
        {
            var allData = await SaveDataSupporter.LoadAll<PlayerData>(Application.persistentDataPath);
            _dataList = allData.ToList();
        }

        public void GameStart()
        {
        }
    }
}