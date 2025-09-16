using Interface;
using UnityEngine;

namespace UnitSystem
{
    public class InGameUIManager : MonoBehaviour, IPresentationLayer
    {
        public void Dispose()
        {
            // TODO マネージリソースをここで解放します
        }

        public void RegisterPresentation()
        {
            throw new System.NotImplementedException();
        }

        public bool GetDomain<T>(out T instance) where T : class, IDomainLayer
        {
            throw new System.NotImplementedException();
        }
    }
}
