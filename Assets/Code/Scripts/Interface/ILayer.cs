using System;
using Service;

namespace Interface
{
    public interface IPresentationLayer : IDisposable
    {
        void RegisterPresentation();

        bool GetDomain<T>(out T instance) where T : IDomainLayer
        {
            return LayeredServiceLocator.Instance.TryGetDomainLayer(out instance);
        }
    }

    public interface IDomainLayer : IDisposable
    {
        void RegisterDomain();

        bool GetData<T>(out T instance) where T : IDataLayer
        {
            return LayeredServiceLocator.Instance.TryGetDataLayer(out instance);
        }
    }

    public interface IDataLayer : IDisposable
    {
        void RegisterData();
    }
}