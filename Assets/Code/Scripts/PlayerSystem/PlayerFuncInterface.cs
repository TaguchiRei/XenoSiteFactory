using UnityEngine.InputSystem;

namespace PlayerSystemInterface
{
    /// <summary>
    /// 何らかの操作をすることを保証するインターフェース
    /// </summary>
    public interface IOperation
    {
        void OnOperation(InputAction.CallbackContext context);
    }

    /// <summary>
    /// 移動入力を受け取ったときに実行する
    /// </summary>
    public interface IMoveAction : IOperation { }

    /// <summary>
    /// インタラクト入力を受け取ったときに実行する
    /// </summary>
    public interface IInteractAction : IOperation { }
    
    /// <summary>
    /// 戻る入力を受け取ったときに実行する
    /// </summary>
    public interface IPreviousAction : IOperation { }

    /// <summary>
    /// 次に進む入力を受け取ったときに実行する
    /// </summary>
    public interface INextAction : IOperation { }

    /// <summary>
    /// マウスを動かしたときの入力を受け取ったときに実行する
    /// </summary>
    public interface IMouseMoveAction : IOperation { }

    /// <summary>
    /// 何らかのキーの入力を受け取ったときに実行する
    /// </summary>
    public interface IAnyKeyAction : IOperation { }
}