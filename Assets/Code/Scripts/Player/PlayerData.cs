using System;
using System.Collections.Generic;
using GamesKeystoneFramework.Save;
using Interface;
using Service;
using UnitInfo;

namespace Player
{
    [Serializable]
    public class PlayerData: SaveDataBase<PlayerData>, IDataLayer
    {
        public string PlayerName;
        public int Days;
        public int Unit;
        public int Xenosite;
        public int Items;
        public int Money;
        public List<PutUnitData> _putUnitDataList;

        public PlayerData()
        {
            PlayerName = "Sinner";
            Days = 0;
            Unit = 0;
            Xenosite = 0;
            Items = 0;
            Money = 0;
            _putUnitDataList = new List<PutUnitData>();
        }
        
        public void Dispose()
        {
            LayeredServiceLocator.Instance.UnRegisterData(this);
        }

        public void RegisterData()
        {
            LayeredServiceLocator.Instance.RegisterData(this);
        }

        protected override PlayerData Initialize()
        {
            var initializedPlayerData = new PlayerData();
            initializedPlayerData.PlayerName = "Sinner";
            initializedPlayerData.Days = 0;
            initializedPlayerData.Unit = 0;
            initializedPlayerData.Xenosite = 0;
            initializedPlayerData.Items = 0;
            initializedPlayerData.Money = 0;
            _putUnitDataList = new List<PutUnitData>();
            return initializedPlayerData;
        }
    }
}
