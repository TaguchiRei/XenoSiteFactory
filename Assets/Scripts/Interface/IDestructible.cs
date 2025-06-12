using UnityEngine;

namespace Interface
{
    public interface IDestructible
    {
        bool IsDead { get; }
        int HitPoint { get; }
        int MaxHitPoint { get; }

        /// <summary>
        /// 体力を減らす（ダメージを受ける）
        /// </summary>
        void HitPointChange(int point);
        /// <summary>
        /// 破壊時に呼ばれる処理
        /// </summary>
        void OnDestroyed();
    }
}