using System;
using UnityEngine.InputSystem;

public interface IInputDispatcher
{
    /// <summary>
    /// Actionの登録を行う
    /// </summary>
    /// <param name="actionMap">ActionMapsをintにパースする</param>
    /// <param name="actionNum">ActionMap + Actions　のEnumをintにパースして使う</param>
    /// <param name="action"></param>
    public void RegisterAction(int actionMap, int actionNum, Action<InputAction.CallbackContext> action);

    /// <summary>
    /// Actionの登録解除を行う
    /// </summary>
    /// <param name="actionMap">ActionMapsをintにパースする</param>
    /// <param name="actionNum">ActionMap + Actions　のEnumをintにパースして使う</param>
    /// <param name="action"></param>
    public void UnRegisterAction(int actionMap, int actionNum, Action<InputAction.CallbackContext> action);

    /// <summary>
    /// ActionMapを変える
    /// </summary>
    /// <param name="actionMap"></param>
    public void SwitchActionMap(int actionMap);

    /// <summary>
    /// 現在有効なActionMapを取得する
    /// </summary>
    public int GetActiveActionMap();
}