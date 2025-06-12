using System;
using DIContainer;
using GamesKeystoneFramework.KeyDebug.KeyLog;
using Interface;
using Manager;
using UnityEngine;

namespace Player
{
    public class Pointer : MonoBehaviour, IPauseable
    {
        public bool IsPaused { get; set; }
        
        InGameManager _inGameManager;

        private void Start()
        {
            if(DiContainer.Instance.TryGet(out _inGameManager))
            {
                KeyLogger.Log("GetInGameManager");
            }
            else
            {
                KeyLogger.Log("GetInGameManager");
            }
        }

        private void Update()
        {
            if(IsPaused) return;
            
            
        }


        public void Pause()
        {
            
        }

        public void Resume()
        {
            
        }
    }
}
