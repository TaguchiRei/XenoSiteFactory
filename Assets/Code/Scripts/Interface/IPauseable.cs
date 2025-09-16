using UnityEngine;

namespace Interface
{
    public interface IPauseable
    {
        bool IsPaused { get; }
        /// <summary>
        /// ポーズする処理をすべてここに書く
        /// </summary>
        void Pause();
        /// <summary>
        /// ポーズ解除時の処理をすべてここに書く
        /// </summary>
        void Resume();
    }
}
