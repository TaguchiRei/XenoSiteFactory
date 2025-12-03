using System;
using UnityEngine.InputSystem;

public class IInputDispatcher
{
    public void RegisterAction(InputType inputType, Action<InputAction.CallbackContext> action)
    {
    }

    public void UnregisterAction(InputType inputType, Action<InputAction.CallbackContext> action)
    {
    }
}