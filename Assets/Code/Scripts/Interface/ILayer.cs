using System;
using Service;

namespace Interface
{
    /// <summary>
    /// 各種機能を管理するクラスが継承する。
    /// </summary>
    /// <typeparam name="T">インターフェースを指定</typeparam>
    public interface IManagementFunc<in T>
    {
        void RegisterFunc(T instance);
        
        void UnregisterFunc(T instance);
    }
    /// <summary>
    /// どこにも参照されず、ドメイン層を参照する。
    /// </summary>
    public interface IPresentationLayer : IDisposable
    {
        void RegisterPresentation();

        bool GetDomain<T>(out T instance) where T : class, IDomainLayer;
    }

    /// <summary>
    /// プレゼンテーション層に参照され、インフラ層およびデータ層を参照する
    /// </summary>
    public interface IDomainLayer : IDisposable
    {
        void RegisterDomain();

        public bool GetData<T>(out T instance) where T : class, IDataLayer;
        public bool GetInfrastructure<T>(out T instance) where T : class, IApplicationLayer;
    }

    /// <summary>
    /// どこの参照も持たず、ドメイン層の参照を持つ。
    /// またはサービスロケーターからメソッドなどの注入を受ける
    /// </summary>
    public interface IApplicationLayer : IDisposable
    {
        void RegisterApplication();
    }

    /// <summary>
    /// どこの参照も持たず、ドメイン層に参照される。
    /// </summary>
    public interface IDataLayer : IDisposable
    {
        void RegisterData();
    }
}