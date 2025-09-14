using System;
using GamesKeystoneFramework.KeyDebug.KeyLog;
using Interface;
using Manager;
using ServiceManagement;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

namespace PlayerSystem
{
    public class PlayerOperationManager : IPresentationLayer
        , InputSystem_Actions.IPlayerActions, InputSystem_Actions.IUIActions, IInitializable
    {
        public Action<InputAction.CallbackContext> OnMoveAction => _onMoveAction;
        private Action<InputAction.CallbackContext> _onMoveAction;
        public Action<InputAction.CallbackContext> OnInteractAction  => _onInteractAction;
        private Action<InputAction.CallbackContext> _onInteractAction;
        public Action<InputAction.CallbackContext> OnPreviousAction  => _onPreviousAction;
        private Action<InputAction.CallbackContext> _onPreviousAction;
        public Action<InputAction.CallbackContext> OnNextAction    => _onNextAction;
        private Action<InputAction.CallbackContext> _onNextAction;
        public Action<InputAction.CallbackContext> OnMouseMoveAction  => _onMouseMoveAction;
        private Action<InputAction.CallbackContext> _onMouseMoveAction;
        public Action<InputAction.CallbackContext> OnAnyKeyAction => _onAnyKeyAction;
        private Action<InputAction.CallbackContext> _onAnyKeyAction;
        

        private InputSystem_Actions _inputSystemActions;
        private InGameManager _inGameManager;

        #region Player

        public void OnMove(InputAction.CallbackContext context)
        {
            KeyLogger.Log("OnMove Input", this);
            if (_inGameManager == null || (int)_inGameManager.DayState < 2)
            {
                _onMoveAction?.Invoke(context);
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            KeyLogger.Log("OnInteract Input", this);
            if (_inGameManager == null || (int)_inGameManager.DayState < 2)
            {
                _onInteractAction?.Invoke(context);
            }
        }

        public void OnPrevious(InputAction.CallbackContext context)
        {
            KeyLogger.Log("OnPrevious Input", this);
            if (_inGameManager == null || (int)_inGameManager.DayState < 2)
            {
                _onPreviousAction?.Invoke(context);
            }
        }

        public void OnNext(InputAction.CallbackContext context)
        {
            KeyLogger.Log("OnNext Input", this);
            if (_inGameManager == null || (int)_inGameManager.DayState < 2)
            {
                _onNextAction?.Invoke(context);
            }
        }

        public void OnMouseMove(InputAction.CallbackContext context)
        {
            if (_inGameManager == null || (int)_inGameManager.DayState < 2)
            {
                _onMouseMoveAction?.Invoke(context);
            }
        }
        
        public void OnAnyKey(InputAction.CallbackContext context)
        {
            
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

        public void Initialize()
        {
            _inputSystemActions = new InputSystem_Actions();
            _inputSystemActions.Player.SetCallbacks(this);
            _inputSystemActions.Enable();
        }

        private void OnDisable()
        {
            Dispose();
        }

        public void Dispose()
        {
            _inputSystemActions?.Dispose();
            ServiceLocateManager.Instance.UnRegisterPresentation(this);
        }

        public void RegisterPresentation()
        {
            ServiceLocateManager.Instance.RegisterPresentation(this);
        }

        public bool GetDomain<T>(out T instance) where T : class, IDomainLayer
        {
            if (ServiceLocateManager.Instance.TryGetDomainLayer<T>(out var domainInstance))
            {
                instance = domainInstance;
                return true;
            }
            else
            {
                instance = default;
                return false;
            }
        }
    }
}