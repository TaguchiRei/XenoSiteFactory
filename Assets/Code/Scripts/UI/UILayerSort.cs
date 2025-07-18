using System;
using System.Collections.Generic;
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
    
    private void SortCanvas()
    {
        for (int i = _uiElements.Count - 1; i >= 0; i--)
        {
            _uiElements[i].transform.SetAsLastSibling();
        }
    }

    /// <summary>
    /// Canvas上でのレイヤーを設定する
    /// </summary>
    /// <param name="objectIndex">一番前に設定する要素のインデックス番号</param>
    public void SetFront(int objectIndex)
    {
        if (_sortedUIElements.Remove(_uiElements[objectIndex]))
            _sortedUIElements.Add(_uiElements[objectIndex]);
    }
}
