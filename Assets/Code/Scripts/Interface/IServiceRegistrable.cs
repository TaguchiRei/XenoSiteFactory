namespace Interface
{
    /// <summary>
    /// サービスロケーターに登録することを保証する
    /// </summary>
    public interface IServiceRegistrable
    {
        /// <summary>
        /// サービスロケーターに登録するためのメソッド
        /// </summary>
        void Register();
    }
}
