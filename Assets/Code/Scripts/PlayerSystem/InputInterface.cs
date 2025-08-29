using UnityEngine.InputSystem;

namespace PlayerSystem
{
    public interface IButtonInputReceiver
    {
        public void OnButtonInput(InputAction.CallbackContext context, ButtonKinds buttonKind);
    }

    public interface ISwipeInputReceiver
    {
        public void OnSwipeInput(InputAction.CallbackContext context);
    }

    public interface ITapInputReceiver
    {
        public void OnTapInput(InputAction.CallbackContext context);
    }

    public enum ButtonKinds
    {
        MenuButton,
        
    }
}
