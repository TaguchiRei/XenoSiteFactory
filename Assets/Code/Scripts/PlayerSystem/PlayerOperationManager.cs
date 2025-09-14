using System;
using GamesKeystoneFramework.KeyDebug.KeyLog;
using Interface;
using ServiceManagement;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

namespace PlayerSystem
{
    public class PlayerOperationManager : IPresentationLayer
        , InputSystem_Actions.IPlayerActions, InputSystem_Actions.IUIActions, IInitializable
    {
        public event Action<InputAction.CallbackContext> OnMoveAction;
        public event Action<InputAction.CallbackContext> OnInteractAction;
        public event Action<InputAction.CallbackContext> OnPreviousAction;
        public event Action<InputAction.CallbackContext> OnNextAction;
        public event Action<InputAction.CallbackContext> OnMouseMoveAction;
        public event Action<InputAction.CallbackContext> OnAnyKeyAction;

        private InputSystem_Actions _inputSystemActions;

        #region Player

        public void OnMove(InputAction.CallbackContext context)
        {
            KeyLogger.Log("OnMove Input", this);
            OnMoveAction?.Invoke(context);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            KeyLogger.Log("OnInteract Input", this);
            OnInteractAction?.Invoke(context);
        }

        public void OnPrevious(InputAction.CallbackContext context)
        {
            KeyLogger.Log("OnPrevious Input", this);
            OnPreviousAction?.Invoke(context);
        }

        public void OnNext(InputAction.CallbackContext context)
        {
            KeyLogger.Log("OnNext Input", this);
            OnNextAction?.Invoke(context);
        }

        public void OnMouseMove(InputAction.CallbackContext context)
        {
            KeyLogger.Log("OnMouseMove Input", this);
            OnMouseMoveAction?.Invoke(context);
        }

        public void OnAnyKey(InputAction.CallbackContext context)
        {
            KeyLogger.Log("OnAnyKey Input", this);
            OnAnyKeyAction?.Invoke(context);
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