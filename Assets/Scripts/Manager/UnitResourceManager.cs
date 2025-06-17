using DIContainer;
using Interface;
using UnityEngine;

namespace Manager
{
    public class UnitResourceManager : MonoBehaviour,IManager
    {
        public void Register()
        {
            DiContainer.Instance.Register(this);
        }

        public void Initialize()
        {
            
        }
    }
}
