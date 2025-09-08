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
        private void Start()
        {
            PlayerOperationManager playerOperationManager = new();
            SceneFlowManager  sceneFlowManager = new();
            playerOperationManager.RegisterPresentation();
            sceneFlowManager.RegisterInfrastructure();
            sceneFlowManager.LoadMainScene(SceneName.StartScene);
        }
    }
}
