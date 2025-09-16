using System;
using GamesKeystoneFramework.KeyDebug.KeyLog;
using GridSystem;
using InGameSystemInterface;
using Interface;
using Player;
using ServiceManagement;
using UnityEngine;
using UnityEngine.Playables;
using XenositeFramework.SceneSystem;

namespace InGameSystem
{
    public class InGameManager : MonoBehaviour, IDomainLayer, ITurnAllEndAction
    {
        private InGameState _inGameState = InGameState.LoadedInGame;
        [SerializeField] private PlayableDirector _startDirector;
        [SerializeField] private PlayableDirector _endDirector;

        private SceneFlowManager _sceneFlowManager;
        private TurnManager _turnManager;
        private GridManager _gridManager;

        #region パブリックメソッド

        /// <summary>
        /// 実際に遊ぶ状態を開始する
        /// </summary>
        public void PlayModeStart()
        {
            ServiceLocateManager.Instance.RegisterFunc<ITurnAllEndAction>(this);
            _turnManager.Initialize();
        }

        #endregion

        #region プライベートメソッド

        private async void Start()
        {
            var gridManager = await ServiceLocateManager.Instance.TryGetDomainLayerAsync<GridManager>();
            var sceneFlow = await ServiceLocateManager.Instance.TryGetApplicationLayerAsync<SceneFlowManager>();
            if (gridManager.Item1 && sceneFlow.Item1)
            {
                _gridManager = gridManager.Item2;
                _sceneFlowManager = sceneFlow.Item2;
                await _sceneFlowManager.LoadMainSceneAsync(SceneName.InGame);
                KeyLogger.Log("Initialized", this);
                _gridManager.Initialize();
                _startDirector.Play();
            }
            else
            {
                KeyLogger.Log("Initialize Failed", this);
            }
        }

        /// <summary>
        /// インゲーム開始時の演出など
        /// </summary>
        private void StartInGame()
        {
            _inGameState = InGameState.StartInGame;
        }

        /// <summary>
        /// タイムラインが終了したとき
        /// </summary>
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

        public void AllEndTurn()
        {
            _endDirector.Play();
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