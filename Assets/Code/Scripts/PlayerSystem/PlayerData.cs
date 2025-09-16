using System;
using Interface;
using ServiceManagement;

namespace PlayerSystem
{
    [Serializable]
    public class PlayerData : IDataLayer
    {
        public string PlayerName;
        public int Days;
        public int Unit;
        public int Xenosite;
        public int Items;
        public int Money;
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
