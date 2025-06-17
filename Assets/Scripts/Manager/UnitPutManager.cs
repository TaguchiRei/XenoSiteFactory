using DIContainer;
using Interface;
using UnityEngine;

namespace Manager
{
    public class UnitPutManager : MonoBehaviour, IManager
    {
        [SerializeField] private int layerLimit = 4;
        public int PutLayer
        {
            get;
            private set;
        }


        public void UpLayer()
        {
            PutLayer++;
            if (PutLayer > layerLimit)
            {
                PutLayer = 1;
            }
        }

        public void DownLayer()
        {
            PutLayer--;
            if (PutLayer <= 0)
            {
                PutLayer = layerLimit;
            }
        }
        
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
