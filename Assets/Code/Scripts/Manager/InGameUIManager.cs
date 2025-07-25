using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Service;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Manager
{
    /// <summary>
    /// UIの動き、UIに対してアクションをした時の処理を記述する
    /// </summary>
    public class InGameUIManager : ManagerBase<InGameUIManager>
    {
        private static readonly int SetMode = Animator.StringToHash("SetMode");

        [SerializeField] private GameObject _inGameUI;
        [SerializeField] private GameObject _inputArea;
        private Animator _inGameUIAnimator;
        private InGameManager _inGameManager;
        private GraphicRaycaster _raycaster;
        private Button[] _uiElements;

        public override void Initialize()
        {
            _inGameUIAnimator = _inGameUI.GetComponent<Animator>();
            ServiceLocatorL.Instance.TryGetClass(out _inGameManager);
            _raycaster = _inGameUI.GetComponent<GraphicRaycaster>();
            _uiElements = _inGameUI.GetComponentsInChildren<Button>();
        }

        /// <summary>
        /// 設置モードに入るボタンを制御
        /// </summary>
        public void PutModeButton()
        {
            _inGameManager.PutModeChange();
            _inGameUIAnimator.SetBool(SetMode, _inGameManager.PutMode);
            ModeChanging().Forget();
        }

        /// <summary>
        /// インプットフィールド上にマウスポインターがあるかを調べる
        /// </summary>
        /// <returns></returns>
        public bool CheckOnInputArea()
        {
            Vector2 pointerPos = Mouse.current.position.ReadValue();
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = pointerPos
            };
            List<RaycastResult> results = new();
            _raycaster.Raycast(pointerData, results);
            if (results.Count > 0 && results[0].gameObject == _inputArea)
            {
                return true;
            }

            return false;
        }

        private async UniTask ModeChanging()
        {
            // 遷移が開始されるまで待機（すでに遷移中なら即座に進む）
            while (!_inGameUIAnimator.IsInTransition(0))
            {
                await UniTask.Yield();
            }
            foreach (var button in _uiElements)
            {
                button.enabled = false;
            }
            // 遷移が終わるまで待機
            while (_inGameUIAnimator.IsInTransition(0))
            {
                await UniTask.Yield();
            }
            foreach (var button in _uiElements)
            {
                button.enabled = true;
            }
        } 
    }
}