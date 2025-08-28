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

        public PlayerData(
            string playerName = "",
            int days = 0,
            int unit = 0,
            int xenosite = 0, 
            int items = 0,
            int money = 0,
            List<PutUnitData> putUnitDataList = null)
        {
            PlayerName = playerName;
            Days = days;
            Unit = unit;
            Xenosite = xenosite;
            Items = items;
            Money = money;
            _putUnitDataList = putUnitDataList;
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
            var initializedPlayerData = new PlayerData
                (
                    playerName:"Sinner",
                    days: 0,
                    unit: 0,
                    xenosite: 0,
                    items: 0,
                    money: 0,
                    new()
                    );
            return initializedPlayerData;
        }
    }
}
