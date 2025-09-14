using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GamesKeystoneFramework.KeyDebug.KeyLog;
using Interface;
using ServiceManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace XenositeFramework.SceneSystem
{
    public class SceneFlowManager : IApplicationLayer
    {
        private (SceneName sceneName, Scene scene) _mainScene = (Enum.Parse<SceneName>(SceneManager.GetActiveScene().name),SceneManager.GetActiveScene());
        private readonly Dictionary<SceneName, Scene> _subScenes = new();

        /// <summary>
        /// シーン読み込み完了を保証する
        /// </summary>
        /// <param name="loadSceneFunc"></param>
        public async UniTask SceneLoadedGuarantee(Func<UniTask> loadSceneFunc)
        {
            await loadSceneFunc();
            //１フレーム待つことでStartおよびAwakeの実行を待機する。
            await UniTask.Yield();
        }

        /// <summary>
        /// シーン読み込み完了を保証する
        /// </summary>
        /// <param name="loadSceneTask"></param>
        /// <param name="pastSceneRetention"></param>
        public async UniTask SceneLoadedGuarantee(Func<bool, UniTask> loadSceneTask, bool pastSceneRetention = true)
        {
            await loadSceneTask(pastSceneRetention);
            //１フレーム待つことでStartおよびAwakeの実行を待機する
            await UniTask.Yield();
        }

        /// <summary>
        /// 非同期でメインシーンをロードする
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="pastSceneRetention">サブシーンを保持するかどうか。デフォルトはtrue</param>
        public async UniTask LoadMainSceneAsync(SceneName sceneName, bool pastSceneRetention = true)
        {
            if (pastSceneRetention)
            {
                await SceneManager.LoadSceneAsync(sceneName.ToString(), LoadSceneMode.Additive);
                await SceneManager.UnloadSceneAsync(_mainScene.scene);
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName.ToString()));
            }
            else
            {
                await SceneManager.LoadSceneAsync(sceneName.ToString(), LoadSceneMode.Single);
            }

            _mainScene = (sceneName, SceneManager.GetSceneByName(sceneName.ToString()));
        }

        /// <summary>
        /// サブシーンを非同期でロードする
        /// </summary>
        /// <param name="sceneName"></param>
        public async UniTask LoadSubSceneAsync(SceneName sceneName)
        {
            if (_subScenes.ContainsKey(sceneName))
            {
                KeyLogger.LogWarning($"シーン[{sceneName.ToString()}]はすでに読み込まれています。");
                return;
            }

            await SceneManager.LoadSceneAsync(sceneName.ToString(), LoadSceneMode.Additive);
            _subScenes[sceneName] = SceneManager.GetSceneByName(sceneName.ToString());
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
                _subScenes.Remove(sceneName);
            }
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

        public void Dispose()
        {
            ServiceLocateManager.Instance.UnRegisterInfrastructure(this);
        }

        public void RegisterApplication()
        {
            ServiceLocateManager.Instance.RegisterApplication(this);
        }
    }
}