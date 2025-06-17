using DIContainer;
using Interface;
using UnityEngine;

namespace Manager
{
    public class UnitInstallManager : MonoBehaviour, IManager
    {
        public void Register()
        {
            DiContainer.Instance.Register<IManager>(this);
        }

        public void Initialize()
        {
            Register();
        }
    }
}
