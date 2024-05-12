using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SelectableUI : MonoBehaviour
{
    //�{�^��UI�̃��X�g
    [SerializeField] 
    private List<GameObject> _buttonUIs = new List<GameObject>();

    //�{�^��UI�̃��X�g�̃v���p�e�B
    public List<GameObject> ButtonUIs { get => _buttonUIs; }

    //�{�^��UI��RectTransform�̃��X�g
    private List<RectTransform> _buttons = new List<RectTransform>();

    //�g�嗦
    [SerializeField]
    private float scaleRate = 1.2f;

    //�{�^���̃f�t�H���g�̃T�C�Y
    private Vector2 _defaultSize;

    //���ݑI�����Ă���{�^���̃C���f�b�N�X
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
        //Debug�p
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
    /// ���̃{�^����I������
    /// </summary>
    public void OnNext()
    {
        if(_index < _buttons.Count - 1)_index++;
        UIUpdate();
    }

    /// <summary>
    /// �O�̃{�^����I������
    /// </summary>
    public void OnPrev()
    {
        if(_index > 0)_index--;
        UIUpdate();
    }

    /// <summary>
    /// ������
    /// </summary>
    private void UIInit()
    {
        foreach (var button in _buttons)
        {
            button.localScale = _defaultSize;
        }
    }

    /// <summary>
    /// UI�̍X�V
    /// </summary>
    private void UIUpdate()
    {
        UIInit();
        _buttons[_index].localScale = _defaultSize * scaleRate;
    }
}
