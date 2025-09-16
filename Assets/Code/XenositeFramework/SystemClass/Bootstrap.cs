using System.Collections;
using PlayerSystem;
using UnityEngine;
using XenositeFramework.SceneSystem;

namespace XenositeFramework.SystemClass
{
    /// <summary>
    /// システムクラスの初期化を行う。
    /// </summary>
    public class Bootstrap : MonoBehaviour
    {
        private async void Start()
        {
            PlayerOperationManager playerOperationManager = new();
            SaveDataInitializer saveDataInitializer = new();
            SceneFlowManager sceneFlowManager = new();
            playerOperationManager.RegisterPresentation();
            saveDataInitializer.RegisterData();
            sceneFlowManager.RegisterApplication();
            await sceneFlowManager.LoadMainSceneAsync(SceneName.StartScene);
        }
    }
}