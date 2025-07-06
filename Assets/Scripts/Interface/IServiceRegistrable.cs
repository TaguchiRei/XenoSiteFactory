using DIContainer;

namespace Interface
{
    /// <summary>
    /// マネージャークラスであることを保証し、DIContainerに登録する
    /// </summary>
    public interface IServiceRegistrable
    {
        /// <summary>
        /// DIContainerに登録
        /// </summary>
        void Register();
        
        void Initialize();
    }
}
