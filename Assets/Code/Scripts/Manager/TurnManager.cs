using Interface;
using Manager;

namespace TurnSystem
{
    public class TurnManager : ManagerBase<TurnManager>, IPauseable
    {
        public bool IsPaused { get; set; }

        private int _turnCount;

        public void Pause()
        {
            
        }

        public void Resume()
        {
            
        }

        /// <summary>
        /// 次のターンを再生するメソッド
        /// </summary>
        private void NextTurn()
        {
            _turnCount++;
        }

        public override void Initialize()
        {
            _turnCount = 0;
        }
    }
}