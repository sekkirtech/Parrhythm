using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;
using DG.Tweening;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine.UI;

public class TitelButton : MonoBehaviour
{
    [SerializeField]
    private Image _buttonTextRTF;// �e�r�ǋL�@�{�^���̃e�L�X�g
    [SerializeField]
    private GameObject _licensePanel;// �e�r�ǋL�@���C�Z���X�p�l��
    [SerializeField]
    private Scrollbar _scrollbar;

    [SerializeField]
    private float _fadeTime = 1.5f;
    [SerializeField]
    private float _fadeValue = 0f;

    private Vector2 _tempRStickValue = Vector2.zero;

   private CompositeDisposable disposables = new CompositeDisposable();
    private void Start()
    {
        ControllerManager.Instance.SouthButtonObservable.Subscribe(x =>
        {
            FadeManager.Instance.LoadScene("StageSelect", 1.0f);
            SoundManager.Instance.PlaySE(SESoundData.SE.other);
        }).AddTo(disposables); // �e�r�C���@x�{�^����

        ControllerManager.Instance.TouchPadObservable.Subscribe(x =>
        {
            if(_licensePanel.activeSelf)
            {
                _licensePanel.SetActive(false);
                SoundManager.Instance.PlaySE(SESoundData.SE.other);
            }
            else
            {
                _licensePanel.SetActive(true);
                SoundManager.Instance.PlaySE(SESoundData.SE.other);
            }
        }).AddTo(disposables); // �e�r�ǋL�@���C�Z���X�\�L�̒ǉ�

        ControllerManager.Instance.RStickObservable.Subscribe(x =>
        {
            Debug.Log(x);
            _tempRStickValue = x;
        }).AddTo(disposables); // �e�r�ǋL�@�E�X�e�B�b�N�ŗV�ѕ���

        
        SoundManager.Instance.PlayBGM(BGMSoundData.BGM.Title);
        _buttonTextRTF.DOFade(_fadeValue, _fadeTime).SetLoops(-1, LoopType.Yoyo);// �e�r�ǋL�@�{�^���̃e�L�X�g��_�ł�����
    }

    private void Update()
    {
        if(_tempRStickValue.y > 0.5f)
        {
            if(_scrollbar.value >= 1.0f) return;
            _scrollbar.value += 0.1f * Time.deltaTime;
        }
        else if(_tempRStickValue.y < -0.5f)
        {
            if(_scrollbar.value <= 0.0f) return;
            _scrollbar.value -= 0.1f * Time.deltaTime; 
        }
    }

    private void OnDestroy()
    {
        disposables.Dispose();
    }

}
