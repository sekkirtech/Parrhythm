using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using DG.Tweening;

public class ButtonUIManager : MonoBehaviour
{
    // �{�^��UI�̃��X�g
    [SerializeField]
    private List<GameObject> _buttonUIs = new List<GameObject>();

    // �{�^��UI�̃��X�g�̃v���p�e�B
    public List<GameObject> ButtonUIList => _buttonUIs;

    // �{�^��UI��RectTransform�̃��X�g
    private List<RectTransform> _buttons = new List<RectTransform>();

    // �g�嗦
    [SerializeField]
    private float _scaleRate = 1.2f;
    [SerializeField]
    private float _scaleTime = 0.25f;

    // �{�^���̃f�t�H���g�̃T�C�Y
    private Vector2 _defaultSize;

    // ���ݑI�����Ă���{�^���̃C���f�b�N�X
    private int _index = 0;

    // �Ō�ɌĂяo���ꂽ����
    private float _lastCallTime = 0;
    [SerializeField]
    private float _cursorWaitTime = 0.25f;

    // IDisposable���܂Ƃ߂邽�߂�CompositeDisposable
    private CompositeDisposable _disposables = new CompositeDisposable();

    /// <summary>
    /// ����������
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

        // �R���g���[���[�̓��͂ɑ΂��ăC�x���g�̓o�^
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
    /// �I�u�W�F�N�g�j�����ɍw�ǂ�����
    /// </summary>
    private void OnDestroy()
    {
        _disposables.Dispose();
        _buttons.ForEach(button => button.DOKill());
    }

    /// <summary>
    /// �X�e�B�b�N�̐��������̓��͂��`�F�b�N����
    /// </summary>
    /// <param name="vec2">���̓x�N�g��</param>
    private void CheckStickVertical(Vector2 vec2)
    {
        if (vec2.x < 0 && _index > 0) OnPrev();
        else if (vec2.x > 0 && _index < _buttons.Count - 1) OnNext();
    }

    /// <summary>
    /// ���݂̃{�^����I������
    /// </summary>
    private void OnSelect()
    {
        _buttons[_index].GetComponent<ButtonUIUtil>().OnNext();
        SoundManager.Instance.PlaySE(SESoundData.SE.other);
    }

    /// <summary>
    /// ���̃{�^����I������
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
    /// �O�̃{�^����I������
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
    /// �{�^��UI�̏�����
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
    /// �{�^��UI�̍X�V
    /// </summary>
    private void UIUpdate()
    {
        UIInit();
        _buttons[_index].localScale = new Vector3(_defaultSize.x * _scaleRate, _defaultSize.y * _scaleRate, 1);
        _buttons[_index].DOScale(new Vector3(_defaultSize.x, _defaultSize.y, 1), _scaleTime)
            .SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }
}
