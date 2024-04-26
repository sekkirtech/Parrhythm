using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SelectableUI : MonoBehaviour
{
    //ボタンUIのリスト
    [SerializeField] 
    private List<RectTransform> _buttons = new List<RectTransform>();

    //拡大率
    [SerializeField]
    private float scaleRate;

    //ボタンのデフォルトのサイズ
    private Vector2 _defaultSize;

    //現在選択しているボタンのインデックス
    private int _index = 0;


    private void Start()
    {
        _defaultSize = _buttons[0].sizeDelta;
        UIUpdate();
    }

    private void Update()
    {
        //Debug用
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            OnNext();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            OnPrev();
        }
        //////////////////////////
    }

    /// <summary>
    /// 次のボタンを選択する
    /// </summary>
    public void OnNext()
    {
        if(_index < _buttons.Count - 1)_index++;
        UIUpdate();
    }

    /// <summary>
    /// 前のボタンを選択する
    /// </summary>
    public void OnPrev()
    {
        if(_index > 0)_index--;
        UIUpdate();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void UIInit()
    {
        foreach (var button in _buttons)
        {
            button.sizeDelta = _defaultSize;
        }
    }

    /// <summary>
    /// UIの更新
    /// </summary>
    private void UIUpdate()
    {
        UIInit();
        _buttons[_index].sizeDelta = _defaultSize * scaleRate;
    }
}
