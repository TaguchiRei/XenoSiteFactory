using System;
using System.Collections;
using GamesKeystoneFramework.Attributes;
using GamesKeystoneFramework.KeyDebug.KeyLog;
using InGameSystemInterface;
using Interface;
using ServiceManagement;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace InGameSystem
{
    public class TurnManager : MonoBehaviour, IApplicationLayer, IManagementFunc<IUseTurnAction>, IInitializable, IPauseable
    {
        [KeyReadOnly] private int _turnNumber = 0;
        [SerializeField] private int _maxTurn;
        [SerializeField] private float _turnTime;
        private event Action StartTurnAction;
        private event Action OnTurnAction;
        private event Action EndTurnAction;
        private event Action EndAllTurnAction;

        private void Awake()
        {
            RegisterApplication();
            RegisterManagementFunc();
        }

        private void Start()
        {
            ServiceLocateManager.Instance.RegisterFunc<IPauseable>(this);
        }

        #region 非公開メソッド

        /// <summary>
        /// ターンの進行を管理するコルーチン
        /// </summary>
        /// <returns></returns>
        private IEnumerator TurnFlow()
        {
            for (int i = 0; i < _maxTurn; i++)
            {
                yield return new WaitForSeconds(_turnTime);
                if (IsPaused)
                {
                    yield return new WaitUntil(() => !IsPaused);
                    yield return new WaitForSeconds(_turnTime);
                }
                _turnNumber++;
                StartTurnAction?.Invoke();
                OnTurnAction?.Invoke();
                EndTurnAction?.Invoke();
            }

            EndAllTurnAction?.Invoke();
        }

        #endregion

        #region インターフェース実装

        public void Initialize()
        {
            _turnNumber = 0;
            StartCoroutine(TurnFlow());
        }

        public void RegisterManagementFunc()
        {
            ServiceLocateManager.Instance.RegisterManagementFunc<IUseTurnAction>(this);
            ServiceLocateManager.Instance.RegisterManagementFunc(this);
        }

        public void RegisterFunc(IUseTurnAction instance)
        {
            switch (instance)
            {
                case ITurnStartAction startAction:
                    StartTurnAction += startAction.StartTurn;
                    break;
                case IOnTurnAction onTurnAction:
                    OnTurnAction += onTurnAction.OnTurn;
                    break;
                case ITurnEndAction endAction:
                    EndTurnAction += endAction.EndTurn;
                    break;
                case ITurnAllEndAction allEndAction:
                    EndAllTurnAction += allEndAction.AllEndTurn;
                    break;
            }
            KeyLogger.Log("アクション登録",this);
        }

        public void UnregisterFunc(IUseTurnAction instance)
        {
            switch (instance)
            {
                case ITurnStartAction startAction:
                    StartTurnAction -= startAction.StartTurn;
                    break;
                case IOnTurnAction onTurnAction:
                    OnTurnAction -= onTurnAction.OnTurn;
                    break;
                case ITurnEndAction endAction:
                    EndTurnAction -= endAction.EndTurn;
                    break;
                case ITurnAllEndAction allEndAction:
                    EndAllTurnAction -= allEndAction.AllEndTurn;
                    break;
            }
        }

        public void Dispose()
        {
            ServiceLocateManager.Instance.UnRegisterApplication(this);
            ServiceLocateManager.Instance.UnRegisterManagementFunc(this);
        }

        public void RegisterApplication()
        {
            ServiceLocateManager.Instance.RegisterApplication(this);
        }

        public bool GetDomain<T>(out T instance) where T : class, IDomainLayer
        {
            if (ServiceLocateManager.Instance.TryGetDomainLayer(out T domainInstance))
            {
                instance = domainInstance;
                return true;
            }

            instance = null;
            return false;
        }

        #endregion


        public enum TurnPhase
        {
            /// <summary>
            /// ターン開始時
            /// </summary>
            StartTurn,

            /// <summary>
            /// ターン中
            /// </summary>
            OnTurn,

            /// <summary>
            /// ターン終了時
            /// </summary>
            EndTurn,

            /// <summary>
            /// すべてのターンが終了したとき
            /// </summary>
            EndAllTurn,
        }

        public bool IsPaused { get; private set; }
        public void Pause()
        {
            IsPaused = true;
        }

        public void Resume()
        {
            IsPaused = false;
        }
    }
}