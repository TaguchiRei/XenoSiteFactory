using System;
using System.Collections;
using Interface;
using UnityEngine;
using DIContainer;
using UnityEngine.Events;

namespace Manager
{
    public class InGameManager : MonoBehaviour, IManager
    { 
        public int Day { get; private set; }
        public InGameState DayState { get; private set; }
        
        
        private IEnumerator _oneDayCycleEnumerator;
        [SerializeField] private UnityEvent _dayStartEvent = new UnityEvent();

        public void Register()
        {
            DiContainer.Instance.Register(this);
        }

        public void OnEnable()
        {
            Register();
        }
        


        IEnumerator OneDayCycle()
        {
            //各種マネージャーを初期化する
            DayState = InGameState.DayStart;
            
            yield return null;
        }

        /// <summary>
        /// インゲームでの状態を管理する
        /// </summary>
        public enum InGameState
        {
            /// <summary>
            /// 何もしていない状態。メニューを開いたり、画面内のオブジェクトをクリックすることで切り替わる
            /// </summary>
            Observe = 0,
            /// <summary>
            /// 一日が始まったばかりの状態。処理を終えた後にObserveに切り替わる。　一時停止される
            /// </summary>
            DayStart = 1,
            /// <summary>
            /// 生産ラインを作るための状態。終わるとObserveに切り替わる　　一時停止される
            /// </summary>
            BuildMode = 2,
            /// <summary>
            /// メニューを開いているときの状態。メニューを閉じるとObserveに切り替わる。　　一時停止される
            /// </summary>
            Menu = 3,
            /// <summary>
            /// 一日が終了したときに状態。処理を終えた後にセーブを行い、次の日に向かう。　　一時停止される
            /// </summary>
            DayEnd = 4,
        }
    }
}
