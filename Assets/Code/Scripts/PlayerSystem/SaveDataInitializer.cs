using Interface;
using Player;
using ServiceManagement;
using UnityEngine;

namespace PlayerSystem
{
    public class SaveDataInitializer : IDataLayer
    {
        public void InitializeSaveData(SaveData  saveData)
        {
            
        }

        #region インターフェース実装

        public void Dispose()
        {
            ServiceLocateManager.Instance.UnRegisterData(this);
        }

        public void RegisterData()
        {
            ServiceLocateManager.Instance.RegisterData(this);
        }

        #endregion
    }
}