using System;
using GridSystem;
using Interface;
using Player;
using ServiceManagement;
using UnityEngine;
using UnityEngine.Playables;
using XenositeFramework.SceneSystem;

namespace InGameSystem
{
    public class InGameManager : MonoBehaviour, IDomainLayer
    {
        private InGameState _inGameState = InGameState.LoadedInGame;
        [SerializeField] private PlayableDirector _startDirector;
        [SerializeField] private PlayableDirector _endDirector;

        private SceneFlowManager _sceneFlowManager;
        private GridManager _gridManager;
        private XenositeSaveData _xenositeSaveData;

        #region パブリックメソッド

        /// <summary>
        /// 実際に遊ぶ状態を開始する
        /// </summary>
        public void PlayModeStart()
        {
        }

        #endregion

        #region プライベートメソッド

        private async void Start()
        {
            if (ServiceLocateManager.Instance.TryGetDomainLayer(out _gridManager) &&
                ServiceLocateManager.Instance.TryGetApplicationLayer(out _sceneFlowManager) &&
                ServiceLocateManager.Instance.TryGetDataLayer(out _xenositeSaveData))
            {
                await _sceneFlowManager.LoadMainSceneAsync(SceneName.InGame);
                
            }
        }

        /// <summary>
        /// インゲーム開始時の演出など
        /// </summary>
        private void StartInGame()
        {
            _inGameState = InGameState.StartInGame;
        }

        private void EndInGame()
        {
        }

        #endregion

        #region インターフェース実装

        public void Dispose()
        {
        }

        public bool GetDomain<T>(out T instance) where T : class, IDomainLayer
        {
            if (ServiceLocateManager.Instance.TryGetDomainLayer(out T domainInstance))
            {
                instance = domainInstance;
                return true;
            }
            else
            {
                instance = null;
                return false;
            }
        }

        public void RegisterDomain()
        {
            ServiceLocateManager.Instance.RegisterDomain(this);
        }

        public bool GetData<T>(out T instance) where T : class, IDataLayer
        {
            if (ServiceLocateManager.Instance.TryGetDataLayer(out T dataInstance))
            {
                instance = dataInstance;
                return true;
            }
            else
            {
                instance = null;
                return false;
            }
        }

        public bool GetApplication<T>(out T instance) where T : class, IApplicationLayer
        {
            if (ServiceLocateManager.Instance.TryGetApplicationLayer(out T applicationInstance))
            {
                instance = applicationInstance;
                return true;
            }
            else
            {
                instance = null;
                return false;
            }
        }

        #endregion
    }

    public enum InGameState
    {
        /// <summary> インゲームシーンを読み込んだ直後 </summary>
        LoadedInGame,

        /// <summary> インゲームシーン開始時のムービー </summary>
        StartInGame,

        /// <summary> 実際に遊んでいる状態 </summary>
        PlayMode,

        /// <summary> インゲームシーン終了時のムービー </summary>
        EndInGame
    }
}