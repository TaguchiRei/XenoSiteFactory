using System;
using InGameSystem;
using Interface;
using ServiceManagement;
using UnityEngine;

namespace UnitSystem
{
    public class InGameUIManager : MonoBehaviour, IPresentationLayer
    {
        private static readonly int UIState = Animator.StringToHash("UIState");
        [SerializeField] private Animator _animator;
        private PauseManager _pauseManager;

        private bool _isOpenMenu;
        private bool _isOpenSelectField;
        private int _tabNumber;

        private void Start()
        {
            _pauseManager = GetComponent<PauseManager>();
        }

        #region パブリックメソッド

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
                _pauseManager.Resume();
            }
            else
            {
                _animator.SetInteger(UIState, 1);
                _pauseManager.Pause();
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