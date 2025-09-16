using System;
using GamesKeystoneFramework.KeyDebug.KeyLog;
using InGameSystem;
using Interface;
using ServiceManagement;
using UnityEngine;
using UnityEngine.UI;

namespace UnitSystem
{
    public class InGameUIManager : MonoBehaviour, IPresentationLayer
    {
        private static readonly int UIState = Animator.StringToHash("UIState");

        [SerializeField] private Animator _animator;

        [Header("UI")] 
        [SerializeField] private Image _pauseButton;
        [SerializeField] private Image _speedButton;
        
        [Header("UI Image")]
        [SerializeField] private Sprite[] _pauseSprite;
        [SerializeField] private Sprite[] _speedSprite;

        private PauseManager _pauseManager;
        private TurnManager _turnManager;
        private bool _isOpenMenu;
        private bool _isOpenSelectField;
        private bool _isDoubleSpeed;
        private int _tabNumber;

        private void Start()
        {
            if (!GetApplication(out _pauseManager) &&
                !GetApplication(out  _turnManager))
            {
                KeyLogger.Log($"初期化に失敗", this);
            }
        }

        #region パブリックメソッド

        /// <summary>
        /// ポーズボタンを押したときの挙動
        /// </summary>
        public void PauseButton()
        {
            if (_pauseManager.IsPause)
            {
                _pauseManager.Resume();
                _pauseButton.sprite = _pauseSprite[0];
            }
            else
            {
                _pauseManager.Pause();
                _pauseButton.sprite = _pauseSprite[1];
            }
        }

        /// <summary>
        /// スピードボタンを押したときの挙動
        /// </summary>
        public void SpeedButton()
        {
            if (_isDoubleSpeed)
            {
                _isDoubleSpeed= false;
                _speedButton.sprite = _speedSprite[0];
            }
            else
            {
                _isDoubleSpeed = true;
                _speedButton.sprite = _speedSprite[1];
            }
        }

        /// <summary>
        /// メニューを開く
        /// </summary>
        public void MenuOpenClose()
        {
            _isOpenMenu = !_isOpenMenu;
            if (_isOpenMenu)
            {
                _animator.SetInteger(UIState, 0);
                _pauseManager.Resume();
            }
            else
            {
                _animator.SetInteger(UIState, 2);
                _pauseManager.Pause();
            }
        }

        public void TabChange(int tabNumber)
        {
            if (!_isOpenSelectField && _tabNumber == tabNumber)
            {
                _animator.SetInteger(UIState, 0);
            }
            else
            {
                _animator.SetInteger(UIState, 1);
            }

            _tabNumber = tabNumber;
        }

        #endregion

        #region インターフェース実装

        public void Dispose()
        {
            ServiceLocateManager.Instance.UnRegisterPresentation(this);
        }

        public void RegisterPresentation()
        {
            ServiceLocateManager.Instance.RegisterPresentation(this);
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

        public bool GetApplication<T>(out T instance) where T : class, IApplicationLayer
        {
            if (ServiceLocateManager.Instance.TryGetApplicationLayer(out T applicationInstance))
            {
                instance = applicationInstance;
                return true;
            }

            instance = null;
            return false;
        }

        #endregion
    }
}