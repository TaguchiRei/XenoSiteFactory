using System;
using DIContainer;
using GamesKeystoneFramework.KeyDebug.KeyLog;
using Interface;
using Manager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerOperationManager : MonoBehaviour, IManager, InputSystem_Actions.IPlayerActions,
        InputSystem_Actions.IUIActions
    {
        public Action<InputAction.CallbackContext> OnMoveAction;
        public Action<InputAction.CallbackContext> OnInteractAction;
        public Action<InputAction.CallbackContext> OnPreviousAction;
        public Action<InputAction.CallbackContext> OnNextAction;
        public Action<InputAction.CallbackContext> OnMouseMoveAction;
        
        private InputSystem_Actions inputSystemActions;
        private InGameManager inGameManager;

        private void Awake()
        {
            DiContainer.Instance.Register(this);
        }

        #region Player
        public void OnMove(InputAction.CallbackContext context)
        {
            if (inGameManager == null || (int)inGameManager.DayState < 2)
            {
                OnMoveAction?.Invoke(context);
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (inGameManager == null || (int)inGameManager.DayState < 2)
            {
                OnInteractAction?.Invoke(context);
            }
        }

        public void OnPrevious(InputAction.CallbackContext context)
        {
            if (inGameManager == null || (int)inGameManager.DayState < 2)
            {
                OnPreviousAction?.Invoke(context);
            }
        }

        public void OnNext(InputAction.CallbackContext context)
        {
            if (inGameManager == null || (int)inGameManager.DayState < 2)
            {
                OnNextAction?.Invoke(context);
            }
        }

        public void OnMouseMove(InputAction.CallbackContext context)
        {
            if (inGameManager == null || (int)inGameManager.DayState < 2)
            {
                OnMouseMoveAction?.Invoke(context);
            }
        }

        #endregion
        
        #region UI
        public void OnNavigate(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        #endregion

        public void Register()
        {
            DiContainer.Instance.Register(this);
        }

        public void Initialize()
        {
            inputSystemActions = new InputSystem_Actions();
            inputSystemActions.Player.SetCallbacks(this);
            inputSystemActions.Enable();
        }
    }
}