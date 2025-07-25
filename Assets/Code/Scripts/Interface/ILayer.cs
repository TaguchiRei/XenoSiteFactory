using UnityEngine;

namespace Interface
{
    public interface IPresentationLayer
    {
        void RegisterPresentation();
    }

    public interface IDomainLayer
    {
        void RegisterDomain();
    }

    public interface IDataLayer
    {
        void RegisterData();
    }
}
