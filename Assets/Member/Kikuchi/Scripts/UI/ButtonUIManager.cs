using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

public class ButtonUIManager : MonoBehaviour
{
    // ボタンUIのリスト
    [SerializeField]
    private List<GameObject> _buttonUIs = new List<GameObject>();

    // ボタンUIのリストのプロパティ
    public List<GameObject> ButtonUIList => _buttonUIs;

    // ボタンUIのRectTransformのリスト
    private List<RectTransform> _buttons = new List<RectTransform>();

    // 拡大率
    [SerializeField]
    private float scaleRate = 1.2f;

    // ボタンのデフォルトのサイズ
    private Vector2 _defaultSize;

    // 現在選択しているボタンのインデックス
    private int _index = 0;

    // 最後に呼び出された時間
    private float _lastCallTime = 0;
    [SerializeField]
    private float _cursorWaitTime = 0.25f;

    // IDisposableをまとめるためのCompositeDisposable
    private CompositeDisposable _disposables = new CompositeDisposable();

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
        if (_buttonUIs.Count == 0)
        {
            foreach (Transform child in transform)
            {
                _buttonUIs.Add(child.gameObject);
            }
        }
        _buttons = _buttonUIs.Select(button => button.GetComponent<RectTransform>()).ToList();
        _defaultSize = _buttons[0].localScale;
        UIUpdate();

        ControllerManager.Instance.RStickObservable
            .Subscribe(CheckStickVertical)
            .AddTo(_disposables);
        ControllerManager.Instance.LStickObservable
            .Subscribe(CheckStickVertical)
            .AddTo(_disposables);
        ControllerManager.Instance.DPadObservable
            .Subscribe(CheckStickVertical)
            .AddTo(_disposables);

        ControllerManager.Instance.SouthButtonObservable
            .Subscribe(_ => OnSelect())
            .AddTo(_disposables);
    }

    /// <summary>
    /// デバッグ用の更新処理
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            OnNext();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            OnPrev();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _buttons[_index].GetComponent<ButtonUIUtil>().OnNext();
        }
    }

    /// <summary>
    /// オブジェクト破棄時に購読を解除
    /// </summary>
    private void OnDestroy()
    {
        _disposables.Dispose();
    }

    /// <summary>
    /// スティックの垂直方向の入力をチェックする
    /// </summary>
    /// <param name="vec2">入力ベクトル</param>
    private void CheckStickVertical(Vector2 vec2)
    {
        if (vec2.x < 0) OnPrev();
        else OnNext();
    }

    /// <summary>
    /// 現在のボタンを選択する
    /// </summary>
    private void OnSelect()
    {
        _buttons[_index].GetComponent<ButtonUIUtil>().OnNext();
    }

    /// <summary>
    /// 次のボタンを選択する
    /// </summary>
    private void OnNext()
    {
        if (_lastCallTime + _cursorWaitTime > Time.time) return;
        if (_index < _buttons.Count - 1) _index++;
        UIUpdate();
        _lastCallTime = Time.time;
    }

    /// <summary>
    /// 前のボタンを選択する
    /// </summary>
    private void OnPrev()
    {
        if (_lastCallTime + _cursorWaitTime > Time.time) return;
        if (_index > 0) _index--;
        UIUpdate();
        _lastCallTime = Time.time;
    }

    /// <summary>
    /// ボタンUIの初期化
    /// </summary>
    private void UIInit()
    {
        foreach (var button in _buttons)
        {
            button.localScale = new Vector3(_defaultSize.x, _defaultSize.y, 1);
        }
    }

    /// <summary>
    /// ボタンUIの更新
    /// </summary>
    private void UIUpdate()
    {
        UIInit();
        _buttons[_index].localScale = new Vector3(_defaultSize.x * scaleRate, _defaultSize.y * scaleRate, 1);
    }
}
