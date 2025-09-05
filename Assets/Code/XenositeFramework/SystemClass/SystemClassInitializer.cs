using PlayerSystem;
using Service;
using UnityEngine;
using UnityEngine.SceneManagement;
using XenositeFramework.SceneSystem;

namespace XenositeFramework.SystemClass
{
    public class SystemClassInitializer : MonoBehaviour
    {
        private void Awake()
        {
            PlayerOperationManager playerOperationManager = new();
            SceneFlowManager  sceneFlowManager = new();
            playerOperationManager.RegisterPresentation();
            sceneFlowManager.RegisterInfrastructure();
            Destroy(gameObject);
        }
    }
}
