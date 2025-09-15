using Interface;
using ServiceManagement;

namespace PlayerSystem
{
    public class PlayerData : IDataLayer
    {
        public void Dispose()
        {
            ServiceLocateManager.Instance.UnRegisterData(this);
        }

        public void RegisterData()
        {
            ServiceLocateManager.Instance.RegisterData(this);
        }
    }
}
