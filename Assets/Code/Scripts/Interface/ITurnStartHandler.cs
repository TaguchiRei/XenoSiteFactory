using UnityEngine;

namespace Interface
{
    /// <summary>
    /// 各ターン開始時に行う処理があることを保証する
    /// </summary>
    interface ITurnStartHandler
    {
        /// <summary>
        /// ターン開始時の処理
        /// </summary>
        void TurnStart();
    }

    /// <summary>
    /// 各ターン中に行う処理があることを保証する
    /// </summary>
    interface ITurnUpdateHandler
    {
        /// <summary>
        /// ターン中の処理
        /// </summary>
        void TurnUpdate();
    }

    /// <summary>
    /// 各ターン終了時に行う処理があることを保証する
    /// </summary>
    interface ITurnEndHandler
    {
        /// <summary>
        /// ターン終了時の処理
        /// </summary>
        void TurnEnd();
    }
}
