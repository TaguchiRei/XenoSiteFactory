using System;
using DIContainer;
using UnityEngine;

namespace Player
{
    public class UnitAttacheDetaching : MonoBehaviour
    {
        private PlayerOperationManager _playerOperationManager;
        private void Start()
        {
            DiContainer.Instance.TryGet(out _playerOperationManager);
        }

        private void OnInteract()
        {
            //オブジェクトを設置する処理を書く
            
        }
    }
}
