using System.Collections.Generic;
using DIContainer;
using GamesKeystoneFramework.KeyDebug.KeyLog;
using Interface;
using UnityEngine;

namespace Manager
{
    public class PauseManager : MonoBehaviour, IManager
    {
        
        private List<IPauseable> _pausables = new();
        
        

        public void AddPauseObject(IPauseable pausable)
        {
            _pausables.Add(pausable);
        }

        public void Pause()
        {
            foreach (var pausable in _pausables)
            {
                pausable.Pause();
            }
        }

        public void Resume()
        {
            foreach (var pausable in _pausables)
            {
                pausable.Resume();
            }
        }

        public void Register()
        {
            DiContainer.Instance.Register(this);
        }

        public void Initialize()
        {
            
        }

        private void Awake()
        {
            Register();
        }
    }
}
