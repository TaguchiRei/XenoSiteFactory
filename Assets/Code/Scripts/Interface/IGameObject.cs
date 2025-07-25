using UnityEngine;

namespace Interface
{
    public interface IGameObject
    {
        GameObject gameObject { get; }
        Transform transform { get; }
    }
}
