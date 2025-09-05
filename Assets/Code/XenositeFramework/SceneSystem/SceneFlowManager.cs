using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GamesKeystoneFramework.KeyDebug.KeyLog;
using Interface;
using Service;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace XenositeFramework.SceneSystem
{
    public class SceneFlowManager : IInfrastructure
    {
        private readonly Dictionary<SceneName, Scene> _scenes = new();

        public void LoadMainScene(SceneName sceneName)
        {
            SceneManager.LoadScene(sceneName.ToString(), LoadSceneMode.Single);
            _scenes[sceneName] = SceneManager.GetSceneByName(sceneName.ToString());
        }

        public async UniTask LoadMainSceneAsync(SceneName sceneName)
        {
            await SceneManager.LoadSceneAsync(sceneName.ToString(), LoadSceneMode.Single);
            _scenes[sceneName] = SceneManager.GetSceneByName(sceneName.ToString());
        }

        public void LoadSubScene(SceneName sceneName)
        {
            SceneManager.LoadScene(sceneName.ToString(), LoadSceneMode.Additive);
            _scenes[sceneName] = SceneManager.GetSceneByName(sceneName.ToString());
        }

        public async UniTask LoadSubSceneAsync(SceneName sceneName)
        {
            await SceneManager.LoadSceneAsync(sceneName.ToString(), LoadSceneMode.Additive);
            _scenes[sceneName] = SceneManager.GetSceneByName(sceneName.ToString());
        }

        public bool TryMoveObjectSceneToScene(GameObject targetObj, SceneName targetScene)
        {
            if (targetObj == null || !_scenes.TryGetValue(targetScene, out var scene))
            {
                KeyLogger.Log(targetObj ? $"scene {targetScene} not found" : $"targetObj can not null");
                return false;
            }
            SceneManager.MoveGameObjectToScene(targetObj, scene);
            return true;
        }

        public void Dispose()
        {
            
        }

        public void RegisterInfrastructure()
        {
            LayeredServiceLocator.Instance.RegisterInfrastructure(this);
        }
    }
}