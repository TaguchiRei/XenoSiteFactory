using Interface;
using UnityEngine.InputSystem;

namespace PlayerSystem
{
    public interface IMoveInputReceiver : IDomainLayer
    {
        public void OnMoveInput(InputAction.CallbackContext context);
    }

    public interface IInteractInputReceiver : IDomainLayer
    {
        public void OnInteractInput(InputAction.CallbackContext context);
    }

    public interface IPreviousInputReceiver : IDomainLayer
    {
        public void OnPreviousInput(InputAction.CallbackContext context);
    }

    public interface INextInputReceiver : IDomainLayer
    {
        public void OnNextInput(InputAction.CallbackContext context);
    }

    public interface IMouseMoveInputReceiver : IDomainLayer
    {
        public void OnMouseMoveInput(InputAction.CallbackContext context);
    }

    public interface IAnyKeyInputReceiver : IDomainLayer
    {
        public void OnAnyKeyInput(InputAction.CallbackContext context);
    }

    public interface IScrollWheelInputReceiver : IDomainLayer
    {
        public void OnScrollWheelInput(InputAction.CallbackContext context);
    }
}