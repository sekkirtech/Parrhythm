using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SelectableUI : MonoBehaviour
{
    //ボタンUIのリスト
    [SerializeField] 
    private List<GameObject> _buttonUIs = new List<GameObject>();

    //ボタンUIのリストのプロパティ
    public List<GameObject> ButtonUIs { get => _buttonUIs; }

    //ボタンUIのRectTransformのリスト
    private List<RectTransform> _buttons = new List<RectTransform>();

    //拡大率
    [SerializeField]
    private float scaleRate = 1.2f;

    //ボタンのデフォルトのサイズ
    private Vector2 _defaultSize;

    //現在選択しているボタンのインデックス
    private int _index = 0;


    private void Start()
    {
        if(_buttonUIs.Count == 0)
        {
            foreach (Transform child in transform)
            {
                _buttonUIs.Add(child.gameObject);
            }
        }
        _buttons = _buttonUIs.Select(button => button.GetComponent<RectTransform>()).ToList();
        _defaultSize = _buttons[0].localScale;
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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _buttons[_index].GetComponent<UIUtility>().OnNext();
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
            button.localScale = _defaultSize;
        }
    }

    /// <summary>
    /// UIの更新
    /// </summary>
    private void UIUpdate()
    {
        UIInit();
        _buttons[_index].localScale = _defaultSize * scaleRate;
    }
}
