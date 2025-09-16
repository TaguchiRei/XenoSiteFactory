using System;
using Interface;
using ServiceManagement;
using UnityEngine;

namespace InGameSystem
{
    public class PauseManager : MonoBehaviour, IManagementFunc<IPauseable>, IApplicationLayer
    {
        private Action _pauseAction;
        private Action _resumeAction;

        private void Awake()
        {
            RegisterApplication();
            RegisterManagementFunc();
        }

        public void Pause()
        {
            _pauseAction?.Invoke();
        }

        public void Resume()
        {
            _resumeAction?.Invoke();
        }

        #region インターフェース実装

        public void RegisterManagementFunc()
        {
            ServiceLocateManager.Instance.RegisterManagementFunc(this);
        }

        public void RegisterFunc(IPauseable instance)
        {
            _pauseAction += instance.Pause;
            _resumeAction += instance.Resume;
        }

        public void UnregisterFunc(IPauseable instance)
        {
            _pauseAction -= instance.Pause;
            _resumeAction -= instance.Resume;
        }

        public void Dispose()
        {
            ServiceLocateManager.Instance.UnRegisterManagementFunc(this);
            ServiceLocateManager.Instance.UnRegisterApplication(this);
        }

        public void RegisterApplication()
        {
            ServiceLocateManager.Instance.RegisterApplication(this);
        }

        #endregion
    }
}