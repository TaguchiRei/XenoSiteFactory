using System;
using Service;

namespace Interface
{
    //Presentation層　　→　ドメイン層およびアプリケーション層を取得可能、ユーザーのアクションからドメイン層およびアプリケーション層にアクセスして実行を手助けする。
    //Application層　　→どこの参照も持たずサービスロケーター越しに機能を管理する
    //Domain層　　　　→データ層を取得可能、ビジネスロジックを担当。どのようにロジックが使われるのかを知らない
    //Data層　　　　→データの在り方とその中身を保持、どこの参照も持たない

    /// <summary>
    /// 各種機能を管理するクラスが継承する。
    /// </summary>
    /// <typeparam name="T">インターフェースを指定</typeparam>
    public interface IManagementFunc<in T>
    {
        void RegisterManagementFunc();

        void RegisterFunc(T instance);

        void UnregisterFunc(T instance);
    }

    /// <summary>
    /// どこにも参照されず、ドメイン層およびアプリケーション層を参照する。
    /// </summary>
    public interface IPresentationLayer : IDisposable
    {
        void RegisterPresentation();

        bool GetDomain<T>(out T instance) where T : class, IDomainLayer;
        bool GetApplication<T>(out T instance) where T : class, IApplicationLayer;
    }

    /// <summary>
    /// プレゼンテーション層に参照され、インフラ層およびデータ層を参照する
    /// </summary>
    public interface IDomainLayer : IDisposable
    {
        void RegisterDomain();

        public bool GetData<T>(out T instance) where T : class, IDataLayer;
        public bool GetApplication<T>(out T instance) where T : class, IApplicationLayer;
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