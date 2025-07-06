using System;
using System.Collections;
using Interface;
using UnityEngine;
using DIContainer;
using GamesKeystoneFramework.KeyDebug.KeyLog;
using Player;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Manager
{
    public class InGameManager : MonoBehaviour, IManager
    { 
        public int Day { get; private set; }
        public InGameState DayState { get; private set; }
        
        
        private PauseManager _pauseManager;
        private GridManager _gridManager;
        private PlayerOperationManager _playerOperationManager;
        private UnitResourceManager _unitResourceManager;
        private IEnumerator _oneDayCycleEnumerator;
        [SerializeField] private string _inGameSceneName;
        [SerializeField] private UnityEvent _dayStartEvent = new UnityEvent();
        [SerializeField] private UnityEvent _OpenMenuEvent = new UnityEvent();
        [SerializeField] private UnityEvent _CloseMenuEvent = new UnityEvent();
        [SerializeField] private UnityEvent _DayEndEvent = new UnityEvent();
        
        public bool PutMode{ get; private set; }

        private void Start()
        {
            Initialize();
            SceneManager.LoadScene(_inGameSceneName, LoadSceneMode.Additive);
        }
        
        IEnumerator OneDayCycle()
        {
            DayState = InGameState.DayStart;
            
            yield return null;
            DayState = InGameState.Observe;

            yield return null;
            DayState = InGameState.DayEnd;
        }
        
        public void Register()
        {
            DiContainer.Instance.Register(this);
        }

        public void OpenMenu()
        {
            DayState = InGameState.Menu;
            _pauseManager.Pause();
        }

        public void CloseMenu()
        {
            DayState = InGameState.Observe;
            _pauseManager.Resume();
        }

        public void PutModeChange()
        {
            PutMode = !PutMode;
            if (PutMode)
            {
                _pauseManager.Pause();
            }
            else
            {
                _pauseManager.Resume();
            }
        }

        public void Initialize()
        {
            if (DiContainer.Instance.TryGetClass(out _pauseManager) && 
                DiContainer.Instance.TryGetClass(out _gridManager) &&
                DiContainer.Instance.TryGetClass(out _playerOperationManager) &&
                DiContainer.Instance.TryGetClass(out _unitResourceManager))
            {
                _pauseManager.Initialize();
                _gridManager.Initialize();
                _playerOperationManager.Initialize();
                _unitResourceManager.Initialize();
                KeyLogger.Log("Initialize Success", this);
            }   
            else
            {
                KeyLogger.Log("Initialize Failed", this);
            }
            _oneDayCycleEnumerator = OneDayCycle();
            _oneDayCycleEnumerator.MoveNext();
            DayState = InGameState.Observe;
        }

        public void Awake()
        {
            Register();
        }

        /// <summary>
        /// インゲームでの状態を管理する。
        /// 0以外は一時停止を行い、2以上は操作モードがメニューになる。
        /// </summary>
        public enum InGameState
        {
            /// <summary>
            /// 何もしていない状態。メニューを開いたり、画面内のオブジェクトをクリックすることで切り替わる
            /// </summary>
            Observe = 0,
            /// <summary>
            /// 生産ラインを作るための状態。終わるとObserveに切り替わる　　一時停止される
            /// </summary>
            BuildMode = 1,
            /// <summary>
            /// メニューを開いているときの状態。メニューを閉じるとObserveに切り替わる。　　一時停止される
            /// </summary>
            Menu = 2,
            /// <summary>
            /// 一日が始まったばかりの状態。処理を終えた後にObserveに切り替わる。　一時停止される
            /// </summary>
            DayStart = 3,
            /// <summary>
            /// 一日が終了したときに状態。処理を終えた後にセーブを行い、次の日に向かう。　　一時停止される
            /// </summary>
            DayEnd = 4,
        }
    }
}
