using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SelectableUI : MonoBehaviour
{
    //�{�^��UI�̃��X�g
    [SerializeField] 
    private List<RectTransform> _buttons = new List<RectTransform>();

    //�g�嗦
    [SerializeField]
    private float scaleRate;

    //�{�^���̃f�t�H���g�̃T�C�Y
    private Vector2 _defaultSize;

    //���ݑI�����Ă���{�^���̃C���f�b�N�X
    private int _index = 0;


    private void Start()
    {
        _defaultSize = _buttons[0].sizeDelta;
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
            button.sizeDelta = _defaultSize;
        }
    }

    /// <summary>
    /// UI�̍X�V
    /// </summary>
    private void UIUpdate()
    {
        UIInit();
        _buttons[_index].sizeDelta = _defaultSize * scaleRate;
    }
}
