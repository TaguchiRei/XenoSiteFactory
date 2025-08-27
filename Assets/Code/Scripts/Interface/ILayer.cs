using System;
using Service;

namespace Interface
{
    public interface IPresentationLayer : IDisposable
    {
        void RegisterPresentation();

        bool GetDomain<T>(out T instance) where T : IDomainLayer;
    }

    public interface IDomainLayer : IDisposable
    {
        void RegisterDomain();

        public bool GetData<T>(out T instance) where T : IDataLayer;
    }

    public interface IDataLayer : IDisposable
    {
        void RegisterData();
    }
}