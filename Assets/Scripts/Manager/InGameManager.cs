using System;
using Interface;
using UnityEngine;
using DIContainer;

namespace Manager
{
    public class InGameManager : MonoBehaviour, IManager
    { 
        void IManager.Register()
        {
            DiContainer.Instance.Register(this);
        }
    }
}
