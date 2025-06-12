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
        public Action<Vector2> OnMoveAction;
        public Action OnInteractAction;
        public Action OnPreviousAction;
        public Action OnNextAction;
        public Action<Vector2> OnMouseMoveAction;
        
        private InputSystem_Actions inputSystemActions;
        private InGameManager inGameManager;

        private void OnEnable()
        {
            DiContainer.Instance.Register(this);
        }

        #region Player
        public void OnMove(InputAction.CallbackContext context)
        {
            if (inGameManager == null || (int)inGameManager.DayState < 2)
            {
                OnMoveAction?.Invoke(context.ReadValue<Vector2>());
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (inGameManager == null || (int)inGameManager.DayState < 2)
            {
                OnInteractAction?.Invoke();
            }
        }

        public void OnPrevious(InputAction.CallbackContext context)
        {
            if (inGameManager == null || (int)inGameManager.DayState < 2)
            {
                OnPreviousAction?.Invoke();
            }
        }

        public void OnNext(InputAction.CallbackContext context)
        {
            if (inGameManager == null || (int)inGameManager.DayState < 2)
            {
                OnNextAction?.Invoke();
            }
        }

        public void OnMouseMove(InputAction.CallbackContext context)
        {
            if (inGameManager == null || (int)inGameManager.DayState < 2)
            {
                OnMouseMoveAction?.Invoke(context.ReadValue<Vector2>());
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

            if (DiContainer.Instance.TryGet(out inGameManager))
            {
                KeyLogger.Log("inGameManager is already set", this);
            }
            else
            {
                KeyLogger.Log("inGameManager is not find");
            }
        }
    }
}