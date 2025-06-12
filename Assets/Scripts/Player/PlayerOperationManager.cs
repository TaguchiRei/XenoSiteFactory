using System;
using DIContainer;
using Interface;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerOperationManager : MonoBehaviour, IManager, InputSystem_Actions.IPlayerActions
    {
        private InputSystem_Actions inputSystemActions;

        private void OnEnable()
        {
            DiContainer.Instance.Register(this);
        }

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