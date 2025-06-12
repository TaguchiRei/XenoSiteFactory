using System;
using DIContainer;
using Interface;
using UnityEngine;

namespace Manager
{
    public class PauseManager : MonoBehaviour, IManager
    {
        void IManager.Register()
        {
            DiContainer.Instance.Register<IManager>(this);
        }

        void Pouse()
        {
            
        }
    }
}
