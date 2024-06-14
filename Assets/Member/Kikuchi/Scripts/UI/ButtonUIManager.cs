using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using DG.Tweening;

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
    private float _scaleRate = 1.2f;
    [SerializeField]
    private float _scaleTime = 0.25f;

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

        // コントローラーの入力に対してイベントの登録
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
    /// オブジェクト破棄時に購読を解除
    /// </summary>
    private void OnDestroy()
    {
        _disposables.Dispose();
        _buttons.ForEach(button => button.DOKill());
    }

    /// <summary>
    /// スティックの垂直方向の入力をチェックする
    /// </summary>
    /// <param name="vec2">入力ベクトル</param>
    private void CheckStickVertical(Vector2 vec2)
    {
        if (vec2.x < 0 && _index > 0) OnPrev();
        else if (vec2.x > 0 && _index < _buttons.Count - 1) OnNext();
    }

    /// <summary>
    /// 現在のボタンを選択する
    /// </summary>
    private void OnSelect()
    {
        _buttons[_index].GetComponent<ButtonUIUtil>().OnNext();
        SoundManager.Instance.PlaySE(SESoundData.SE.other);
    }

    /// <summary>
    /// 次のボタンを選択する
    /// </summary>
    private void OnNext()
    {
        SoundManager.Instance.PlaySE(SESoundData.SE.other2);
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
        SoundManager.Instance.PlaySE(SESoundData.SE.other2);
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
            button.DOKill();
            button.localScale = new Vector3(_defaultSize.x, _defaultSize.y, 1);
        }
    }

    /// <summary>
    /// ボタンUIの更新
    /// </summary>
    private void UIUpdate()
    {
        UIInit();
        _buttons[_index].localScale = new Vector3(_defaultSize.x * _scaleRate, _defaultSize.y * _scaleRate, 1);
        _buttons[_index].DOScale(new Vector3(_defaultSize.x, _defaultSize.y, 1), _scaleTime)
            .SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }
}
