using System;
using GamesKeystoneFramework.KeyDebug.KeyLog;
using Interface;
using Manager;
using Service;
using UnityEngine.InputSystem;

namespace PlayerSystem
{
    public class PlayerOperationManager : ManagerBase<PlayerOperationManager>,
        InputSystem_Actions.IPlayerActions,
        InputSystem_Actions.IUIActions,
        IPresentationLayer
    {
        private Action<InputAction.CallbackContext> _onMoveAction;
        private Action<InputAction.CallbackContext> _onInteractAction;
        private Action<InputAction.CallbackContext> _onPreviousAction;
        private Action<InputAction.CallbackContext> _onNextAction;
        private Action<InputAction.CallbackContext> _onMouseMoveAction;
        private Action<InputAction.CallbackContext> _onAnyKeyInputAction;

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

        public override void Initialize()
        {
            _inputSystemActions = new InputSystem_Actions();
            _inputSystemActions.Player.SetCallbacks(this);
            _inputSystemActions.Enable();
            if (LayeredServiceLocator.Instance.TryGetAllFuncDomainLayer<IMoveInputReceiver>(
                    out var moveInputReceivers))
            {
                foreach (var receiver in moveInputReceivers)
                {
                    _onMoveAction += receiver.OnMoveInput;
                }
            }

            if (LayeredServiceLocator.Instance.TryGetAllFuncDomainLayer<IInteractInputReceiver>(
                    out var interactInputReceivers))
            {
                foreach (var receiver in interactInputReceivers)
                {
                    _onInteractAction += receiver.OnInteractInput;
                }
            }

            if (LayeredServiceLocator.Instance.TryGetAllFuncDomainLayer<IPreviousInputReceiver>(
                    out var previousInputReceivers))
            {
                foreach (var receiver in previousInputReceivers)
                {
                    _onPreviousAction += receiver.OnPreviousInput;
                }
            }

            if (LayeredServiceLocator.Instance.TryGetAllFuncDomainLayer<INextInputReceiver>(
                    out var nextInputReceivers))
            {
                foreach (var receiver in nextInputReceivers)
                {
                    _onNextAction += receiver.OnNextInput;
                }
            }

            if (LayeredServiceLocator.Instance.TryGetAllFuncDomainLayer<IMouseMoveInputReceiver>(
                    out var mouseMoveInputReceivers))
            {
                foreach (var receiver in mouseMoveInputReceivers)
                {
                    _onMouseMoveAction += receiver.OnMouseMoveInput;
                }
            }

            if (LayeredServiceLocator.Instance.TryGetAllFuncDomainLayer<IAnyKeyInputReceiver>(
                    out var anyKeyInputReceivers))
            {
                foreach (var receiver in anyKeyInputReceivers)
                {
                    _onAnyKeyInputAction += receiver.OnAnyKeyInput;
                }
            }
        }

        private void OnDisable()
        {
            
            _inputSystemActions.Disable();
        }

        public void Dispose()
        {
            _inputSystemActions?.Dispose();
        }

        public void RegisterPresentation()
        {
            LayeredServiceLocator.Instance.RegisterPresentation(this);
        }

        public bool GetDomain<T>(out T instance) where T : class, IDomainLayer
        {
            if (LayeredServiceLocator.Instance.TryGetDomainLayer<T>(out var domainInstance))
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