using System.Collections.Generic;
using DIContainer;
using Interface;
using UnityEngine;

namespace Manager
{
    public class PauseManager : MonoBehaviour, IManager
    {
        
        private List<IPauseable> Pausables = new();
        
        
        void IManager.Register()
        {
            DiContainer.Instance.Register<IManager>(this);
        }
        
        

        private void Pause()
        {
            foreach (var pauseable in Pausables)
            {
                pauseable.Pause();
            }
        }

        private void Resume()
        {
            foreach (var pauseable in Pausables)
            {
                pauseable.Resume();
            }
        }
    }
}
