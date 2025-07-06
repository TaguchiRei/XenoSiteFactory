using DIContainer;
using Interface;
using UnityEngine;

namespace Manager
{
    public abstract class ManagerBase : MonoBehaviour, IServiceRegistrable
    {
        public void Register()
        {
            ServiceLocator.Instance.Register(this);
        }

        private void Awake()
        {
            Register();
        }

        public abstract void Initialize();
    }
}