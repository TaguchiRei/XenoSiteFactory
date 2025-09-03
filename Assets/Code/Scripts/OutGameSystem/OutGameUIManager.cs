using Interface;
using Service;
using UnityEngine;

namespace OutGameSystem
{
    public class OutGameUIManager : MonoBehaviour, IPresentationLayer
    {

        private void OnAnyKeyInput()
        {
            
        }
        public void Dispose()
        {
            LayeredServiceLocator.Instance.UnRegisterPresentation(this);
        }

        public void RegisterPresentation()
        {
            LayeredServiceLocator.Instance.RegisterPresentation(this);
        }

        public bool GetDomain<T>(out T instance) where T : class, IDomainLayer
        {
            if (LayeredServiceLocator.Instance.TryGetDomainLayer(out T instanceDomain))
            {
                instance = instanceDomain;
                return true;
            }
            instance = null;
            return false;
        }
    }
}
