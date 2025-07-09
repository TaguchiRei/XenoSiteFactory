using Manager;
using Service;
using UnityEngine;

namespace Manager
{
    /// <summary>
    /// UIの動き、UIに対してアクションをした時の処理を記述する
    /// </summary>
    public class InGameUIManager : ManagerBase<InGameUIManager>
    {
        [SerializeField] private GameObject _inGameUI;
        private Animator _inGameUIAnimator;
        private InGameManager _inGameManager;
        
        public override void Initialize()
        {
            _inGameUIAnimator = _inGameUI.GetComponent<Animator>();
            ServiceLocator.Instance.TryGetClass(out _inGameManager);
        }

        private void PutModeButton()
        {
            _inGameManager.PutModeChange();
        }
    }
}