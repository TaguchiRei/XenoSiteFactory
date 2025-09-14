using UnityEngine.InputSystem;

namespace PlayerSystemInterface
{
    /// <summary>
    /// 何らかの操作をすることを保証するインターフェース
    /// </summary>
    public interface IOperation { }

    /// <summary>
    /// 移動入力を受け取ったときに実行する
    /// </summary>
    public interface IMoveAction : IOperation
    {
        void OnMoveOperation(InputAction.CallbackContext context);
    }

    /// <summary>
    /// インタラクト入力を受け取ったときに実行する
    /// </summary>
    public interface IInteractAction : IOperation
    {
        void OnInteractOperation(InputAction.CallbackContext context);
    }

    /// <summary>
    /// 戻る入力を受け取ったときに実行する
    /// </summary>
    public interface IPreviousAction : IOperation
    {
        void OnPreviousOperation(InputAction.CallbackContext context);
    }

    /// <summary>
    /// 次に進む入力を受け取ったときに実行する
    /// </summary>
    public interface INextAction : IOperation
    {
        void OnNextOperation(InputAction.CallbackContext context);
    }

    /// <summary>
    /// マウスを動かしたときの入力を受け取ったときに実行する
    /// </summary>
    public interface IMouseMoveAction : IOperation
    {
        void OnMouseMoveOperation(InputAction.CallbackContext context);
    }

    /// <summary>
    /// 何らかのキーの入力を受け取ったときに実行する
    /// </summary>
    public interface IAnyKeyAction : IOperation
    {
        void OnAnyKeyOperation(InputAction.CallbackContext context);
    }
}