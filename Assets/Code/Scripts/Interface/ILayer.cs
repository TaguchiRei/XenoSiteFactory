using Service;
using UnityEngine;

namespace Interface
{
    public interface IPresentationLayer
    {
        void RegisterPresentation();

        bool GetDomain<T>(out T instance) where T : IDomainLayer
        {
            return LayeredServiceLocator.Instance.TryGetDomainLayer(out instance);
        }
    }

    public interface IDomainLayer
    {
        void RegisterDomain();

        bool GetData<T>(out T instance) where T : IDataLayer
        {
            return LayeredServiceLocator.Instance.TryGetDataLayer(out instance);
        }
    }

    public interface IDataLayer
    {
        void RegisterData();
    }

}
