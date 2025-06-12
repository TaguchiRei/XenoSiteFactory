using DIContainer;

namespace Interface
{
    /// <summary>
    /// マネージャークラスであることを保証し、DIContainerに登録する
    /// </summary>
    public interface IManager
    {
        /// <summary>
        /// DIContainerに登録
        /// </summary>
        public void Register();
    }
}
