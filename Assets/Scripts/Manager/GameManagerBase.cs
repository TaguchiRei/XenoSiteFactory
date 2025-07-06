using DIContainer;
using Interface;
using UnityEngine;

public abstract class GameManagerBase : MonoBehaviour, IServiceRegistrable
{
    public void Register()
    {
        ServiceLocator.Instance.Register(this);
    }

    private void Awake()
    {
        Register();
    }

    public void Initialize()
    {
    }
}