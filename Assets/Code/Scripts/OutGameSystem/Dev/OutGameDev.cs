using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GamesKeystoneFramework.KeyDebug.KeyLog;
using Player;
using Service;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XenositeFramework.SaveSystem;
using XenositeFramework.SceneSystem;

namespace OutGameSystem.Dev
{
    public class OutGameDev : MonoBehaviour
    {
        [SerializeField, Header("データ表示用プレハブ")] private GameObject _dataPrehab;
        [SerializeField, Header("データグループオブジェクト")] private GameObject _dataGroup;
        
        private SceneFlowManager _sceneFlowManager;

        private async void Start()
        {
            var allData = await SaveDataSupporter.LoadAll<PlayerData>(Application.persistentDataPath);
            LayeredServiceLocator.Instance.TryGetInfrastructureLayer(out _sceneFlowManager);
            
            foreach (var data in allData)
            {
                var obj = Instantiate(_dataPrehab, _dataGroup.transform, true);
                var tmp = obj.GetComponentInChildren<TextMeshProUGUI>();
                var button = obj.GetComponent<Button>();
                tmp.text = $"{data.PlayerName}\nDay:{data.Days}";
                button.onClick.AddListener(() => LoadPlayerData(data).Forget());
            }
        }

        private async UniTask LoadPlayerData(PlayerData playerData)
        {
            KeyLogger.Log($"LoadPlayerData {playerData.PlayerName}");
            LayeredServiceLocator.Instance.RegisterData(playerData);
            await _sceneFlowManager.LoadMainSceneAsync(SceneName.InGameDev);
        }
    }
}