using System;
using UnityEngine.InputSystem;

public interface IInputDispatcher
{
    /// <summary>
    /// Actionの登録を行う
    /// </summary>
    /// <param name="actionMap">ActionMapsをstringにパースする</param>
    /// <param name="actionName">ActionMap + Actions　のEnumをstringにパースして使う</param>
    /// <param name="action"></param>
    public void RegisterAction(string actionMap, string actionName, Action<InputAction.CallbackContext> action);

    /// <summary>
    /// Actionの登録解除を行う
    /// </summary>
    /// <param name="actionMap">ActionMapsをstringにパースする</param>
    /// <param name="actionName">ActionMap + Actions　のEnumをstringにパースして使う</param>
    /// <param name="action"></param>
    public void UnRegisterAction(string actionMap, string actionName, Action<InputAction.CallbackContext> action);

    /// <summary>
    /// ActionMapを変える
    /// </summary>
    /// <param name="actionMap">ActionMapsをstringにパースする</param>
    public void SwitchActionMap(string actionMap);
    
    /// <summary>
    /// 現在有効なActionMapのIDを取得する
    /// </summary>
    /// <returns>ActionMapにパースする</returns>
    public int GetActiveActionMap();
}