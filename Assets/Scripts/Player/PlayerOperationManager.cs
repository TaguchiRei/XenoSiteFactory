using System;
using DIContainer;
using Interface;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerOperationManager : MonoBehaviour, IManager, InputSystem_Actions.IPlayerActions,
        InputSystem_Actions.IUIActions
    {
        private InputSystem_Actions inputSystemActions;

        private void OnEnable()
        {
            DiContainer.Instance.Register(this);
        }

        #region Player
        public void OnMove(InputAction.CallbackContext context)
        {
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
        }

        public void OnPrevious(InputAction.CallbackContext context)
        {
        }

        public void OnNext(InputAction.CallbackContext context)
        {
        }

        public void OnMouseMove(InputAction.CallbackContext context)
        {
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