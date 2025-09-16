using GridSystem;
using Interface;
using Player;
using ServiceManagement;
using UnityEngine;

namespace PlayerSystem
{
    public class SaveDataInitializer : IDataLayer
    {
        public void InitializeSaveData(XenositeSaveData  xenositeSaveData)
        {
            // GridPlacedDataおよびPlayerDataをサービスロケーターに登録する
            ServiceLocateManager.Instance.RegisterData(xenositeSaveData.PlayerData);
            ServiceLocateManager.Instance.RegisterData(xenositeSaveData.PlacedObjectData);
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