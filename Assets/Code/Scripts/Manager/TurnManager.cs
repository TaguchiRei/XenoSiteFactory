using System;
using System.Collections.Generic;
using Interface;
using Manager;
using UnityEngine;

namespace TurnSystem
{
    public class TurnManager : ManagerBase<TurnManager>, IPauseable
    {
        public bool IsPaused { get; set; }
        public int TurnCount { get; private set; }

        [SerializeField] private float _turnDefaultInterval = 1;
        [SerializeField] private float _turnSpeedMultiplier = 1;
        
        private List<ITurnStartHandler> _turnStartHandlers;
        private List<ITurnUpdateHandler> _turnUpdateHandlers;
        private List<ITurnEndHandler> _turnEndHandlers;
        

        private float _turnTimer;

        private void Update()
        {
            if(IsPaused) return;
            
            _turnTimer += Time.deltaTime * _turnSpeedMultiplier;
            if (_turnTimer >= _turnDefaultInterval)
            {
                TurnProcess();
                _turnTimer = 0;
            }
        }

        public void AddTurnStartHandler(ITurnStartHandler handler)
        {
            _turnStartHandlers.Add(handler);
        }

        public void AddTurnUpdateHandler(ITurnUpdateHandler handler)
        {
            _turnUpdateHandlers.Add(handler);
        }

        public void AddTurnEndHandler(ITurnEndHandler handler)
        {
            _turnEndHandlers.Add(handler);
        }

        private void TurnProcess()
        {
            foreach (var handler in _turnStartHandlers)
            {
                handler.TurnStart();
            }

            foreach (var handler in _turnUpdateHandlers)
            {
                handler.TurnUpdate();
            }

            foreach (var handler in _turnEndHandlers)
            {
                handler.TurnEnd();
            }
        }
        
        /// <summary>
        /// 次のターンを再生するメソッド
        /// </summary>
        private void NextTurn()
        {
            TurnCount++;
        }

        public void Pause()
        {
            IsPaused = true;
        }

        public void Resume()
        {
            IsPaused = false;
        }
        
        public override void Initialize()
        {
            TurnCount = 0;
        }
    }
}