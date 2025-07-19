using Interface;
using UnityEngine;

namespace StaticObject
{
    public class WallObject : MonoBehaviour, IDestructible
    {
        public WallObject(int maxHitPoint)
        {
            MaxHitPoint = maxHitPoint;
        }

        public bool IsDead { get; private set; }
        public int HitPoint { get; private set; }
        public int MaxHitPoint { get; }

        public void HitPointChange(int point)
        {
            HitPoint += point;
            if (HitPoint >= MaxHitPoint)
            {
                IsDead = true;
            }
        }

        public void OnDestroyed()
        {
            
        }
    }
}
