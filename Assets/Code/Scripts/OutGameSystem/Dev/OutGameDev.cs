using Cysharp.Threading.Tasks;
using GamesKeystoneFramework.KeyDebug.KeyLog;
using Player;
using PlayerSystem;
using ServiceManagement;
using TMPro;
using UnityEngine;
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
            var allData = await SaveDataSupporter.LoadAll<XenositeSaveData>(Application.persistentDataPath);
            ServiceLocateManager.Instance.TryGetApplicationLayer(out _sceneFlowManager);
            
            foreach (var data in allData)
            {
                var obj = Instantiate(_dataPrehab, _dataGroup.transform, true);
                var tmp = obj.GetComponentInChildren<TextMeshProUGUI>();
                var button = obj.GetComponent<Button>();
                tmp.text = $"{data.PlayerData.PlayerName}\nDay:{data.PlayerData.Days}";
                button.onClick.AddListener(() => LoadPlayerData(data).Forget());
            }
        }

        private async UniTask LoadPlayerData(XenositeSaveData xenositeSaveData)
        {
            KeyLogger.Log($"LoadPlayerData {xenositeSaveData.PlayerData.PlayerName}");
            if (ServiceLocateManager.Instance.TryGetDataLayer(out SaveDataInitializer saveDataInitializer))
            {
                saveDataInitializer.InitializeSaveData(xenositeSaveData);
            }

            await _sceneFlowManager.LoadSubSceneAsync(SceneName.ManagementScene);
        }
    }
}