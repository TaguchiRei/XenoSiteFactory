using System.Collections.Generic;
using GamesKeystoneFramework.KeyDebug.KeyLog;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class UILayerSort : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private List<GameObject> _uiElements = new();

    private List<GameObject> _sortedUIElements = new();

    private void Start()
    {
        _sortedUIElements = new(_uiElements);
    }

    /// <summary>
    /// Canvasをソートする
    /// </summary>
    private void SortCanvas()
    {
        for (int i = 0; i < _uiElements.Count; i++)
        {
            _sortedUIElements[i].transform.SetAsLastSibling();
        }
    }

    /// <summary>
    /// Canvas上でのレイヤーを設定する
    /// </summary>
    /// <param name="objectIndex">一番前に設定する要素のインデックス番号</param>
    public void SetFront(int objectIndex)
    {
        if (_sortedUIElements.Remove(_uiElements[objectIndex]))
        {
            _sortedUIElements.Add(_uiElements[objectIndex]);
            SortCanvas();
            KeyLogger.Log(objectIndex + " set to front");
        }
    }
}