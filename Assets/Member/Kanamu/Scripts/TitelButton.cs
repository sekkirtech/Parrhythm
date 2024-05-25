using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;
using DG.Tweening;

public class TitelButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _buttonTextRTF;// �e�r�ǋL�@�{�^���̃e�L�X�g

    [SerializeField]
    private float _fadeTime = 1.5f;
    [SerializeField]
    private float _fadeValue = 0f;

   private CompositeDisposable disposables = new CompositeDisposable();
    private void Start()
    {
        ControllerManager.Instance.AnyButtonObservable.Subscribe(x => FadeManager.Instance.LoadScene("StageSelect", 1.0f)).AddTo(disposables); // �e�r�C���@x�{�^������any�{�^���ɕύX
        _buttonTextRTF.DOFade(_fadeValue, _fadeTime).SetLoops(-1, LoopType.Yoyo);// �e�r�ǋL�@�{�^���̃e�L�X�g��_�ł�����
    }

    private void OnDestroy()
    {
        disposables.Dispose();
    }

}
