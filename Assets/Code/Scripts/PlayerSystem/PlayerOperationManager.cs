using System;
using GamesKeystoneFramework.KeyDebug.KeyLog;
using Interface;
using Manager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerSystem
{
    public class PlayerOperationManager : ManagerBase<PlayerOperationManager>,
        InputSystem_Actions.IPlayerActions,
        InputSystem_Actions.IUIActions
    {
        public Action<InputAction.CallbackContext> OnMoveAction;
        public Action<InputAction.CallbackContext> OnInteractAction;
        public Action<InputAction.CallbackContext> OnPreviousAction;
        public Action<InputAction.CallbackContext> OnNextAction;
        public Action<InputAction.CallbackContext> OnMouseMoveAction;
        
        private InputSystem_Actions inputSystemActions;
        private InGameManager inGameManager;

        #region Player
        public void OnMove(InputAction.CallbackContext context)
        {
            KeyLogger.Log("OnMove Input", this);
            if (inGameManager == null || (int)inGameManager.DayState < 2)
            {
                OnMoveAction?.Invoke(context);
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            KeyLogger.Log("OnInteract Input", this);
            if (inGameManager == null || (int)inGameManager.DayState < 2)
            {
                OnInteractAction?.Invoke(context);
            }
        }

        public void OnPrevious(InputAction.CallbackContext context)
        {
            KeyLogger.Log("OnPrevious Input", this);
            if (inGameManager == null || (int)inGameManager.DayState < 2)
            {
                OnPreviousAction?.Invoke(context);
            }
        }

        public void OnNext(InputAction.CallbackContext context)
        {
            KeyLogger.Log("OnNext Input", this);
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
            KeyLogger.Log("OnNavigate Input", this);
            throw new NotImplementedException();
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            KeyLogger.Log("OnSubmit Input", this);
            throw new NotImplementedException();
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            KeyLogger.Log("OnCancel Input", this);
            throw new NotImplementedException();
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
            KeyLogger.Log("OnPoint Input", this);
            throw new NotImplementedException();
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            KeyLogger.Log("OnClick Input", this);
            throw new NotImplementedException();
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
            KeyLogger.Log("OnRightClick Input", this);
            throw new NotImplementedException();
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
            KeyLogger.Log("OnMiddleClick Input", this);
            throw new NotImplementedException();
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
            KeyLogger.Log("OnScrollWheel Input", this);
            throw new NotImplementedException();
        }

        #endregion

        public override void Initialize()
        {
            inputSystemActions = new InputSystem_Actions();
            inputSystemActions.Player.SetCallbacks(this);
            inputSystemActions.Enable();
        }

        private void OnDisable()
        {
            inputSystemActions.Disable();
        }
    }
}