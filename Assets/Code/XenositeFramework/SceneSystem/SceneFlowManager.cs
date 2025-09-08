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
        private (SceneName sceneName, Scene scene) _mainScene;
        private readonly Dictionary<SceneName, Scene> _subScenes = new();

        /// <summary>
        /// メインシーンをロードする
        /// </summary>
        /// <param name="sceneName"></param>
        public void LoadMainScene(SceneName sceneName)
        {
            SceneManager.LoadScene(sceneName.ToString(), LoadSceneMode.Single);
            _mainScene = (sceneName, SceneManager.GetSceneByName(sceneName.ToString()));
        }

        /// <summary>
        /// 非同期でメインシーンをロードする
        /// </summary>
        /// <param name="sceneName"></param>
        public async UniTask LoadMainSceneAsync(SceneName sceneName)
        {
            await SceneManager.LoadSceneAsync(sceneName.ToString(), LoadSceneMode.Single);
            _mainScene = (sceneName, SceneManager.GetSceneByName(sceneName.ToString()));
        }

        /// <summary>
        /// サブシーンをロードする
        /// </summary>
        /// <param name="sceneName"></param>
        public void LoadSubScene(SceneName sceneName)
        {
            SceneManager.LoadScene(sceneName.ToString(), LoadSceneMode.Additive);
            _subScenes[sceneName] = SceneManager.GetSceneByName(sceneName.ToString());
        }

        /// <summary>
        /// サブシーンを非同期でロードする
        /// </summary>
        /// <param name="sceneName"></param>
        public async UniTask LoadSubSceneAsync(SceneName sceneName)
        {
            await SceneManager.LoadSceneAsync(sceneName.ToString(), LoadSceneMode.Additive);
            _subScenes[sceneName] = SceneManager.GetSceneByName(sceneName.ToString());
        }

        /// <summary>
        /// GameObjectを別のシーンに移動させる
        /// </summary>
        /// <param name="targetObj"></param>
        /// <param name="targetScene"></param>
        /// <returns></returns>
        public bool TryMoveObjectSceneToScene(GameObject targetObj, SceneName targetScene)
        {
            if (targetObj == null || !_subScenes.TryGetValue(targetScene, out var scene))
            {
                KeyLogger.Log(targetObj ? $"scene {targetScene} not found" : $"targetObj can not be null", this);
                return false;
            }
            SceneManager.MoveGameObjectToScene(targetObj, scene);
            return true;
        }

        /// <summary>
        /// GameObjectをメインシーンに移動させる
        /// </summary>
        /// <param name="targetObj"></param>
        public void MoveObjectSubToMain(GameObject targetObj)
        {
            if (targetObj == null)
            {
                KeyLogger.Log("targetObj can not be null", this);
                return;
            }
            SceneManager.MoveGameObjectToScene(targetObj, _mainScene.Item2);
        }
        
        /// <summary>
        /// 開いているメインシーンを取得する
        /// </summary>
        /// <returns></returns>
        public SceneName GetMainScene()
        {
            return _mainScene.sceneName;
        }

        /// <summary>
        /// 開いているサブシーンを取得する
        /// </summary>
        /// <returns></returns>
        public List<SceneName> GetLoadedSubScene()
        {
            return new List<SceneName>(_subScenes.Keys);
        }

        /// <summary>
        /// サブシーンをアンロードする
        /// </summary>
        /// <param name="sceneName"></param>
        public async UniTask UnLoadSubSceneAsync(SceneName sceneName)
        {
            if (_subScenes.TryGetValue(sceneName, out var scene))
            {
                await SceneManager.UnloadSceneAsync(scene);
            }
        }
        
        public void Dispose()
        {
            LayeredServiceLocator.Instance.UnRegisterInfrastructure(this);
        }

        public void RegisterInfrastructure()
        {
            LayeredServiceLocator.Instance.RegisterInfrastructure(this);
        }
    }
}