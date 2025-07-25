using System.Collections.Generic;
using Interface;
using Manager;

namespace TurnSystem
{
    public class TurnManager : ManagerBase<TurnManager>, IPauseable
    {
        public bool IsPaused { get; set; }

        private List<ITurnStartHandler> _turnStartHandlers;
        private List<ITurnUpdateHandler> _turnUpdateHandlers;
        private List<ITurnEndHandler> _turnEndHandlers;
        
        private int _turnCount;

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
            _turnCount = 0;
        }
        
        /// <summary>
        /// 次のターンを再生するメソッド
        /// </summary>
        private void NextTurn()
        {
            _turnCount++;
        }
    }
}