using DIContainer;
using Interface;
using UnityEngine;

namespace Manager
{
    /// <summary>
    /// マネージャークラスのベースクラス。
    /// Awakeでサービスロケーターに登録する
    /// </summary>
    public abstract class ManagerBase : MonoBehaviour, IServiceRegistrable
    {
        public void Register()
        {
            ServiceLocator.Instance.Register(this);
        }

        private void Awake()
        {
            Register();
        }

        /// <summary>
        /// 変数の初期化などを行う
        /// </summary>
        public abstract void Initialize();
    }
}