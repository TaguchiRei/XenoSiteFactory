using System;
using System.Collections.Generic;
using GamesKeystoneFramework.Save;
using GridSystem;
using Interface;
using PlayerSystem;
using ServiceManagement;
using UnitInfo;

namespace Player
{
    [Serializable]
    public class XenositeSaveData: SaveDataBase<XenositeSaveData>, IDataLayer
    {
        public PlayerData PlayerData;
        public PlacedObjectData PlacedObjectData;

        public XenositeSaveData(PlayerData playerData, PlacedObjectData placedObjectData)
        {
            PlayerData = playerData;
            PlacedObjectData = placedObjectData;
        }
        
        public void Dispose()
        {
            ServiceLocateManager.Instance.UnRegisterData(this);
        }

        public void RegisterData()
        {
            ServiceLocateManager.Instance.RegisterData(this);
        }

        protected override XenositeSaveData Initialize()
        {
            var initializedPlayerData = new XenositeSaveData(new(), new());
            return initializedPlayerData;
        }
    }
}
