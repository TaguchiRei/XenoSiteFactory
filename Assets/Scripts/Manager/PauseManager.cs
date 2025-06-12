using System.Collections.Generic;
using DIContainer;
using Interface;
using UnityEngine;

namespace Manager
{
    public class PauseManager : MonoBehaviour, IManager
    {
        
        private List<IPauseable> _pausables = new();
        
        
        void IManager.Register()
        {
            DiContainer.Instance.Register<IManager>(this);
        }

        public void AddPauseObject(IPauseable pausable)
        {
            _pausables.Add(pausable);
        }

        private void Pause()
        {
            foreach (var pausable in _pausables)
            {
                pausable.Pause();
            }
        }

        private void Resume()
        {
            foreach (var pausable in _pausables)
            {
                pausable.Resume();
            }
        }
    }
}
