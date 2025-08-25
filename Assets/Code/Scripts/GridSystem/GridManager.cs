using GamesKeystoneFramework.KeyDebug.KeyLog;
using Interface;
using Service;
using UnitInfo;
using UnityEngine;

namespace GridSystem
{
    public class GridManager : MonoBehaviour, IDomainLayer
    {
        private GridExistData _gridExistData;
        private GridDistanceData _gridDistanceData;
        private PlacedObjectData _placedObjectData;


        #region テスト用スクリプト

        private void GenerateTestData()
        {
            KeyLogger.LogWarning("This is Test Only Method");
            _gridExistData = new ();
            _gridDistanceData = new(10);
            _placedObjectData = new();
            _gridDistanceData.RegisterData();
            _gridDistanceData.RegisterData();
            _placedObjectData.RegisterData();
        }

        #endregion


        
        public void Dispose()
        {
            LayeredServiceLocator.Instance.UnRegisterDomain(this);
        }

        public void RegisterDomain()
        {
            LayeredServiceLocator.Instance.RegisterDomain(this);
        }
    }
}
